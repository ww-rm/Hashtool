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
            if (calcTask != null && calcTaskCclTokenSrc != null)
            {
                calcTaskCclTokenSrc.Cancel();
                calcTask.Wait();

                calcTaskCclTokenSrc.Dispose(); //回收资源
                calcTask = null;
                calcTaskCclTokenSrc = null;
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
            
        }
    }
}
