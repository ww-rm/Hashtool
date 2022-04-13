using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Hashtool
{
    public partial class MainWnd : Form
    {
        public MainWnd()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
            // 初始化界面
            SetStopState();

            // 默认界面设置
            cbMD5.Checked = true;
            cbSHA1.Checked = true;
            //cbSHA2_256.Checked = true;
            //cbSHA2_512.Checked = true;
            //cbSHA3_256.Checked = true;
            //cbSHA3_512.Checked = true;
            //cbSM3.Checked = true;
            cbCRC32.Checked = true;

            cbUpperCase.Checked = true;
            cbUseMultiThread.Checked = true;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "所有文件(*.*)|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BeginCompute(ofd.FileNames);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.textResult.Clear();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (this.textResult.Text.Length > 0)
            { 
                Clipboard.SetText(this.textResult.Text);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*",
                DefaultExt = ".txt",
                FileName = "HashResult.txt"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, textResult.Text);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (compTask != null && compTaskCclTokenSrc != null)
            {
                compTaskCclTokenSrc.Cancel();
                compTask.Wait();

                compTaskCclTokenSrc.Dispose(); //回收资源
                compTask = null;
                compTaskCclTokenSrc = null;
            }
        }

        private void textResult_DragDrop(object sender, DragEventArgs e)
        {
            // 获取拖动时的文件列表
            BeginCompute((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        private void textResult_DragEnter(object sender, DragEventArgs e)
        {
            // 拖动时候的鼠标效果
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
