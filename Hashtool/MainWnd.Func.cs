using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Hashtool
{
    public partial class MainWnd
    {
        private Task calcTask = null;
        private CancellationTokenSource calcTaskCclToken = null;

        private object pbValueLock = new object(); // 用于下面两个值和进度条更新的锁
        private double pbValueSingle = 0; // 当前文件计算进度
        private double pbValueTotal = 0; // 总进展

        private int algCount = 0;
        private int fileCount = 0;

        private void SetRunState()
        {
            if (this.InvokeRequired)
            {
                // Invoke 可以防止死锁
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

        private List<HashAlgType> GetAlgEnabledList()
        {
            var result = new List<HashAlgType>();
            if (cbMD5.CheckState == CheckState.Checked)
            {
                result.Add(HashAlgType.MD5);
            }
            if (cbSHA1.CheckState == CheckState.Checked)
            {
                result.Add(HashAlgType.SHA1);
            }
            if (cbSHA256.CheckState == CheckState.Checked)
            {
                result.Add(HashAlgType.SHA256);
            }
            if (cbSHA3.CheckState == CheckState.Checked)
            {
                result.Add(HashAlgType.SHA3);
            }
            if (cbSM3.CheckState == CheckState.Checked)
            {
                result.Add(HashAlgType.SM3);
            }
            if (cbCRC32.CheckState == CheckState.Checked)
            {
                result.Add(HashAlgType.CRC32);
            }
            return result;
        }

        private void UpdateProgressBarValue()
        {
            int singleVal = pbSingle.Maximum;
            int totalVal = pbTotal.Maximum;
            lock (pbValueLock)
            {
                if (fileCount <= 0)
                {
                    singleVal = totalVal = 0;
                }
                else
                {
                    if (algCount <= 0)
                    {
                        singleVal = (int)Math.Round(pbValueSingle * pbSingle.Maximum);
                        totalVal = (int)Math.Round(pbValueTotal / fileCount * pbTotal.Maximum);
                    }
                    else
                    {
                        singleVal = (int)Math.Round(pbValueSingle / algCount * pbSingle.Maximum);
                        totalVal = (int)Math.Round(pbValueTotal / algCount / fileCount * pbTotal.Maximum);
                    }
                }
            }

            // 裁剪值至合法范围
            singleVal = Math.Min(Math.Max(pbSingle.Minimum, singleVal), pbSingle.Maximum);
            totalVal = Math.Min(Math.Max(pbTotal.Minimum, totalVal), pbTotal.Maximum);
            if (this.InvokeRequired)
            {
                pbSingle.BeginInvoke(new Action(() => pbSingle.Value = singleVal));
                pbSingle.BeginInvoke(new Action(() => pbTotal.Value = totalVal));
            }
            else
            {
                pbSingle.Value = singleVal;
                pbTotal.Value = totalVal;
            }
        }

        /// <summary>
        /// 开始计算哈希的过程
        /// </summary>
        private void BeginCalc(string[] paths)
        {
            calcTaskCclToken = new CancellationTokenSource();
            calcTask = new Task(
                () => TaskCalcHash(paths, calcTaskCclToken.Token),
                calcTaskCclToken.Token,
                TaskCreationOptions.LongRunning
            );
            calcTask.Start();
        }

        /// <summary>
        /// 计算哈希的任务
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="ct"></param>
        private void TaskCalcHash(string[] paths, CancellationToken ct)
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

            // 获取要计算的哈希算法
            var algEnabledList = GetAlgEnabledList();
            algCount = algEnabledList.Count;

            // 进度条清零
            lock (pbValueLock)
            {
                pbValueSingle = 0;
                pbValueTotal = 0;
            }
            UpdateProgressBarValue();

            var tasks = new Task<string>[algCount];
            foreach (var fInfo in fileInfos)
            {
                // 清空单个文件进度条
                lock (pbValueLock)
                {
                    pbValueSingle = 0;
                }

                // 开启子 Tasks 计算每一种算法的哈希
                for (int i = 0; i < algCount; i++)
                {
                    switch (algEnabledList[i])
                    {
                        case HashAlgType.MD5:
                            tasks[i] = new Task<string>(() => TaskCalcMD5(fInfo, calcTaskCclToken.Token), calcTaskCclToken.Token, TaskCreationOptions.LongRunning);
                            tasks[i].Start();
                            break;
                        case HashAlgType.SHA1:
                            tasks[i] = new Task<string>(() => TaskCalcSHA1(fInfo, calcTaskCclToken.Token), calcTaskCclToken.Token, TaskCreationOptions.LongRunning);
                            tasks[i].Start();
                            break;
                        case HashAlgType.SHA256:
                            tasks[i] = new Task<string>(() => TaskCalcSHA256(fInfo, calcTaskCclToken.Token), calcTaskCclToken.Token, TaskCreationOptions.LongRunning);
                            tasks[i].Start();
                            break;
                        case HashAlgType.SHA3:
                            tasks[i] = new Task<string>(() => TaskCalcSHA3(fInfo, calcTaskCclToken.Token), calcTaskCclToken.Token, TaskCreationOptions.LongRunning);
                            tasks[i].Start();
                            break;
                        case HashAlgType.SM3:
                            tasks[i] = new Task<string>(() => TaskCalcSM3(fInfo, calcTaskCclToken.Token), calcTaskCclToken.Token, TaskCreationOptions.LongRunning);
                            tasks[i].Start();
                            break;
                        case HashAlgType.CRC32:
                            tasks[i] = new Task<string>(() => TaskCalcCRC32(fInfo, calcTaskCclToken.Token), calcTaskCclToken.Token, TaskCreationOptions.LongRunning);
                            tasks[i].Start();
                            break;
                        default:
                            throw new Exception($"Unknown HashAlgType: {algEnabledList[i]}");
                    }
                }

                // 轮询监视
                while (true)
                {
                    // 更新进度条
                    UpdateProgressBarValue();

                    // 检查是否被取消
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }
                    // 检查子任务是否都完成
                    else
                    {
                        if (tasks.All(e => e.IsCompleted))
                        {
                            // 格式化输出当前文件的计算结果
                            var result = "";
                            result += $"路径: {fInfo.FullName}{Environment.NewLine}";
                            result += $"大小: {fInfo.Length} 字节{Environment.NewLine}";
                            result += $"最后修改时间: {fInfo.LastWriteTime}{Environment.NewLine}";
                            for (int i = 0; i < algCount; i++)
                            {
                                switch (algEnabledList[i])
                                {
                                    case HashAlgType.MD5:
                                        result += $"MD5: {tasks[i].Result}{Environment.NewLine}";
                                        break;
                                    case HashAlgType.SHA1:
                                        result += $"SHA1: {tasks[i].Result}{Environment.NewLine}";
                                        break;
                                    case HashAlgType.SHA256:
                                        result += $"SHA256: {tasks[i].Result}{Environment.NewLine}";
                                        break;
                                    case HashAlgType.SHA3:
                                        result += $"SHA3: {tasks[i].Result}{Environment.NewLine}";
                                        break;
                                    case HashAlgType.SM3:
                                        result += $"SM3: {tasks[i].Result}{Environment.NewLine}";
                                        break;
                                    case HashAlgType.CRC32:
                                        result += $"CRC32: {tasks[i].Result}{Environment.NewLine}";
                                        break;
                                    default:
                                        throw new Exception($"Unknown HashAlgType: {algEnabledList[i]}");
                                }
                            }
                            textResult.Invoke(
                                new Action(() => textResult.AppendText($"{result}{Environment.NewLine}"))
                            );
                            break; // 进入下一个文件的计算
                        }
                    }

                    // 降低 CPU 负载
                    Thread.Sleep(10);
                }

                // 处理无算法特殊情况, 直接加进度
                if (algCount <= 0)
                {
                    lock (pbValueLock)
                    {
                        pbValueSingle += 1;
                        pbValueTotal += 1;
                    }
                }

                // 更新进度条
                UpdateProgressBarValue();

                // 检查是否被取消
                if (calcTaskCclToken.IsCancellationRequested)
                {
                    // 中止后续计算工作
                    break;
                }
            }

            // 改变窗体界面显示
            SetStopState();
        }


        private string TaskCalcMD5(FileInfo fileInfo, CancellationToken ct)
        {
            var md5Obj = new MD5CryptoServiceProvider();
            md5Obj.Initialize();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                var fin = fileInfo.OpenRead();

                while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    md5Obj.TransformBlock(buffer, 0, readCount, buffer, 0);

                    // 更新进度条
                    lock (pbValueLock)
                    {
                        double completed = (double)readCount / fileInfo.Length;
                        pbValueSingle += completed;
                        pbValueTotal += completed;
                    }
                }

                fin.Close();
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            md5Obj.TransformFinalBlock(buffer, 0, readCount);
            return Byte2Str(md5Obj.Hash);
        }

        private string TaskCalcSHA1(FileInfo fileInfo, CancellationToken ct)
        {
            var sha1Obj = new SHA1CryptoServiceProvider();
            sha1Obj.Initialize();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                var fin = fileInfo.OpenRead();

                while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    sha1Obj.TransformBlock(buffer, 0, readCount, buffer, 0);

                    // 更新进度条
                    lock (pbValueLock)
                    {
                        double completed = (double)readCount / fileInfo.Length;
                        pbValueSingle += completed;
                        pbValueTotal += completed;
                    }
                }

                fin.Close();
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            sha1Obj.TransformFinalBlock(buffer, 0, readCount);
            return Byte2Str(sha1Obj.Hash);
        }

        private string TaskCalcSHA256(FileInfo fileInfo, CancellationToken ct)
        {
            var sha256Obj = new SHA256CryptoServiceProvider();
            sha256Obj.Initialize();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                var fin = fileInfo.OpenRead();

                while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    sha256Obj.TransformBlock(buffer, 0, readCount, buffer, 0);

                    // 更新进度条
                    lock (pbValueLock)
                    {
                        double completed = (double)readCount / fileInfo.Length;
                        pbValueSingle += completed;
                        pbValueTotal += completed;
                    }
                }

                fin.Close();
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            sha256Obj.TransformFinalBlock(buffer, 0, readCount);
            return Byte2Str(sha256Obj.Hash);
        }

        private string TaskCalcSHA3(FileInfo fileInfo, CancellationToken ct)
        {
            var sha3Obj = new MD5CryptoServiceProvider();
            sha3Obj.Initialize();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                var fin = fileInfo.OpenRead();

                while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    sha3Obj.TransformBlock(buffer, 0, readCount, buffer, 0);

                    // 更新进度条
                    lock (pbValueLock)
                    {
                        double completed = (double)readCount / fileInfo.Length;
                        pbValueSingle += completed;
                        pbValueTotal += completed;
                    }
                }

                fin.Close();
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            sha3Obj.TransformFinalBlock(buffer, 0, readCount);
            return Byte2Str(sha3Obj.Hash);
        }

        private string TaskCalcSM3(FileInfo fileInfo, CancellationToken ct)
        {
            var sm3Obj = new MD5CryptoServiceProvider();
            sm3Obj.Initialize();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                var fin = fileInfo.OpenRead();

                while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    sm3Obj.TransformBlock(buffer, 0, readCount, buffer, 0);

                    // 更新进度条
                    lock (pbValueLock)
                    {
                        double completed = (double)readCount / fileInfo.Length;
                        pbValueSingle += completed;
                        pbValueTotal += completed;
                    }
                }

                fin.Close();
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            sm3Obj.TransformFinalBlock(buffer, 0, readCount);
            return Byte2Str(sm3Obj.Hash);
        }

        private string TaskCalcCRC32(FileInfo fileInfo, CancellationToken ct)
        {
            var crc32Obj = new CRC32();
            crc32Obj.Init();

            var bufferSize = 10240;
            var buffer = new byte[bufferSize];
            var readCount = 0;
            if (fileInfo.Length > 0)
            {
                var fin = fileInfo.OpenRead();

                while ((readCount = fin.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    crc32Obj.Update(buffer, readCount);

                    // 更新进度条
                    lock (pbValueLock)
                    {
                        double completed = (double)readCount / fileInfo.Length;
                        pbValueSingle += completed;
                        pbValueTotal += completed;
                    }
                }

                fin.Close();
            }
            else
            {
                lock (pbValueLock)
                {
                    pbValueSingle += 1;
                    pbValueTotal += 1;
                }
            }

            crc32Obj.Final();
            return Byte2Str(crc32Obj.CRC32Code);
        }

        public static string Byte2Str(byte[] b)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < b.Length; i++)
                str.AppendFormat("{0:X2}", b[i]);

            return str.ToString();
        }
    }
}
