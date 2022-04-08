using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Hashtool
{
    public partial class MainWnd
    {
        private Task calcTask = null;
        private CancellationTokenSource calcTaskCclTokenSrc = null;

        private object pbValueLock = new object(); // 用于下面两个值和进度条更新的锁
        private double pbValueSingle = 0; // 当前文件计算进度
        private double pbValueTotal = 0; // 总进展

        private int algCount = 0;
        private int fileCount = 0;

        private void SetRunState()
        {
            if (this.InvokeRequired)
            {
                // BeginInvoke 可以防止死锁
                this.algPanel.BeginInvoke(new Action(() => this.algPanel.Enabled = false));
                this.btnOpen.BeginInvoke(new Action(() => this.btnOpen.Enabled = false));
                this.btnClear.BeginInvoke(new Action(() => this.btnClear.Enabled = false));
                this.btnCopy.BeginInvoke(new Action(() => this.btnCopy.Enabled = false));
                this.btnSave.BeginInvoke(new Action(() => this.btnSave.Enabled = false));
                this.btnStop.BeginInvoke(new Action(() => this.btnStop.Enabled = true));
                this.textResult.BeginInvoke(new Action(() => this.textResult.AllowDrop = false));
            }
            else
            {
                this.algPanel.Enabled = false;
                this.btnOpen.Enabled = false;
                this.btnClear.Enabled = false;
                this.btnCopy.Enabled = false;
                this.btnSave.Enabled = false;
                this.btnStop.Enabled = true;
                this.textResult.AllowDrop = false;
            }
        }

        private void SetStopState()
        {
            if (this.algPanel.InvokeRequired)
            {
                this.algPanel.BeginInvoke(new Action(() => this.algPanel.Enabled = true));
                this.btnOpen.BeginInvoke(new Action(() => this.btnOpen.Enabled = true));
                this.btnClear.BeginInvoke(new Action(() => this.btnClear.Enabled = true));
                this.btnCopy.BeginInvoke(new Action(() => this.btnCopy.Enabled = true));
                this.btnSave.BeginInvoke(new Action(() => this.btnSave.Enabled = true));
                this.btnStop.BeginInvoke(new Action(() => this.btnStop.Enabled = false));
                this.textResult.BeginInvoke(new Action(() => this.textResult.AllowDrop = true));
            }
            else
            {
                this.algPanel.Enabled = true;
                this.btnOpen.Enabled = true;
                this.btnClear.Enabled = true;
                this.btnCopy.Enabled = true;
                this.btnSave.Enabled = true;
                this.btnStop.Enabled = false;
                this.textResult.AllowDrop = true;
            }
        }

        private List<HashAlgHandler> GetAlgEnabledList()
        {
            var result = new List<HashAlgHandler>();
            if (cbMD5.CheckState == CheckState.Checked)
            {
                result.Add(new HashAlgHandler(HashAlgType.MD5));
            }
            if (cbSHA1.CheckState == CheckState.Checked)
            {
                result.Add(new HashAlgHandler(HashAlgType.SHA1));
            }
            if (cbSHA256.CheckState == CheckState.Checked)
            {
                result.Add(new HashAlgHandler(HashAlgType.SHA256));
            }
            if (cbSHA3.CheckState == CheckState.Checked)
            {
                result.Add(new HashAlgHandler(HashAlgType.SHA3));
            }
            if (cbSM3.CheckState == CheckState.Checked)
            {
                result.Add(new HashAlgHandler(HashAlgType.SM3));
            }
            if (cbCRC32.CheckState == CheckState.Checked)
            {
                result.Add(new HashAlgHandler(HashAlgType.CRC32));
            }
            return result;
        }

        private void UpdateProgressBarValue()
        {
            double singleVal = pbSingle.Maximum;
            double totalVal = pbTotal.Maximum;
            lock (pbValueLock)
            {
                singleVal = pbValueSingle;
                totalVal = pbValueTotal;
            }
            if (fileCount <= 0)
            {
                singleVal = totalVal = 0;
            }
            else
            {
                if (algCount <= 0)
                {
                    singleVal = Math.Round(singleVal * pbSingle.Maximum);
                    totalVal = (int)Math.Round(totalVal / fileCount * pbTotal.Maximum);
                }
                else
                {
                    singleVal = Math.Round(singleVal / algCount * pbSingle.Maximum);
                    totalVal = Math.Round(totalVal / algCount / fileCount * pbTotal.Maximum);
                }
            }

            // 裁剪值至合法范围
            singleVal = Math.Min(Math.Max(pbSingle.Minimum, singleVal), pbSingle.Maximum);
            totalVal = Math.Min(Math.Max(pbTotal.Minimum, totalVal), pbTotal.Maximum);
            if (this.InvokeRequired)
            {
                pbSingle.BeginInvoke(new Action(() => pbSingle.Value = (int)singleVal));
                pbSingle.BeginInvoke(new Action(() => pbTotal.Value = (int)totalVal));
            }
            else
            {
                pbSingle.Value = (int)singleVal;
                pbTotal.Value = (int)totalVal;
            }
        }

        /// <summary>
        /// 开始计算哈希的过程
        /// </summary>
        private void BeginCompute(string[] paths)
        {
            calcTaskCclTokenSrc = new CancellationTokenSource();
            calcTask = new Task(
                () => TaskCompute(paths, calcTaskCclTokenSrc.Token),
                calcTaskCclTokenSrc.Token,
                TaskCreationOptions.LongRunning
            );
            calcTask.Start();
        }

        /// <summary>
        /// 计算哈希的任务
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="ct"></param>
        private void TaskCompute(string[] paths, CancellationToken ct)
        {
            // 改变窗体界面显示
            SetRunState();

            // 筛出来有效的文件路径
            var fileInfos = new List<FileInfo>();
            foreach (var p in paths)
            {
                if (File.Exists(p))
                {
                    fileInfos.Add(new FileInfo(p));
                }
            }

            fileCount = fileInfos.Count;
            if (fileCount > 0)
            {
                // 进度条清零
                lock (pbValueLock)
                {
                    pbValueSingle = 0;
                    pbValueTotal = 0;
                }
                UpdateProgressBarValue();

                // 获取要计算的哈希算法
                var algEnabledList = GetAlgEnabledList();
                algCount = algEnabledList.Count;
                foreach (var fInfo in fileInfos)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    // 清空单个文件进度条
                    lock (pbValueLock)
                    {
                        pbValueSingle = 0;
                    }
                    UpdateProgressBarValue();

                    // 输出结果
                    var result = new StringBuilder();

                    // 添加文件基本信息
                    result.Append($"路径: {fInfo.FullName}{Environment.NewLine}");
                    result.Append($"大小: {fInfo.Length} 字节{Environment.NewLine}");
                    result.Append($"修改时间: {fInfo.LastWriteTime}{Environment.NewLine}");

                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    if (algCount > 0)
                    {
                        // 开启子 Tasks 计算每一种算法的哈希
                        var tasks = new Task<string>[algCount];
                        foreach (var i in Enumerable.Range(0, algCount)) // 必须使用 foreach, 否则匿名函数传值有问题
                        {
                            tasks[i] = new Task<string>(
                                () => TaskCompHash(fInfo, algEnabledList[i].HashObj, calcTaskCclTokenSrc.Token),
                                calcTaskCclTokenSrc.Token,
                                TaskCreationOptions.LongRunning
                            );
                            tasks[i].Start();
                        }

                        // 轮询监视
                        while (true)
                        {
                            UpdateProgressBarValue();

                            if (ct.IsCancellationRequested)
                            {
                                break;
                            }

                            // 检查子任务是否都完成
                            if (tasks.All(e => e.IsCompleted))
                            {
                                for (int i = 0; i < algCount; i++)
                                {
                                    result.Append($"{algEnabledList[i].Name}: {tasks[i].Result}{Environment.NewLine}");
                                }
                                break; // 进入下一个文件的计算
                            }

                            // 降低 CPU 负载
                            Thread.Sleep(10);
                        }
                    }
                    else
                    {
                        // 更新进度条
                        lock (pbValueLock)
                        {
                            pbValueSingle += 1;
                            pbValueTotal += 1;
                        }
                        UpdateProgressBarValue();
                    }

                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    // 格式化输出当前文件的计算结果
                    result.Append(Environment.NewLine);
                    textResult.Invoke(new Action(() => textResult.AppendText(result.ToString())));

                    UpdateProgressBarValue();
                }
            }

            // 改变窗体界面显示
            SetStopState();
        }

        private string TaskCompHash(FileInfo fileInfo, HashAlgorithm hashObj, CancellationToken ct)
        {
            hashObj.Initialize();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                try
                {
                    var fin = fileInfo.OpenRead();
                    while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            return "Cancelled.";
                        }

                        hashObj.TransformBlock(buffer, 0, readCount, buffer, 0);

                        // 更新进度信息
                        double completed = (double)readCount / fileInfo.Length;
                        lock (pbValueLock)
                        {
                            pbValueSingle += completed;
                            pbValueTotal += completed;
                        }
                    }
                    fin.Close();
                }
                catch (IOException e)
                {
                    return $"IOException: {e.Message}.";
                }
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            hashObj.TransformFinalBlock(buffer, 0, 0);
            return Byte2HexStr(hashObj.Hash);
        }

        public static string Byte2HexStr(byte[] b, bool useUpperCase = true)
        {
            StringBuilder str = new StringBuilder();
            string fmt = useUpperCase ? "{0:X2}" : "{0:x2}";

            for (int i = 0; i < b.Length; i++)
            {
                str.AppendFormat(fmt, b[i]);
            }

            return str.ToString();
        }
    }
}
