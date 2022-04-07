
namespace Hashtool
{
    partial class MainWnd
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
            this.textResult = new System.Windows.Forms.TextBox();
            this.cbMD5 = new System.Windows.Forms.CheckBox();
            this.cbSHA1 = new System.Windows.Forms.CheckBox();
            this.cbSHA256 = new System.Windows.Forms.CheckBox();
            this.cbSHA3 = new System.Windows.Forms.CheckBox();
            this.cbSM3 = new System.Windows.Forms.CheckBox();
            this.cbCRC32 = new System.Windows.Forms.CheckBox();
            this.algPanel = new System.Windows.Forms.Panel();
            this.btnOpen = new System.Windows.Forms.Button();
            this.labelSingle = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.pbSingle = new System.Windows.Forms.ProgressBar();
            this.pbTotal = new System.Windows.Forms.ProgressBar();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlButton = new System.Windows.Forms.TableLayoutPanel();
            this.algPanel.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // textResult
            // 
            this.textResult.AllowDrop = true;
            this.textResult.BackColor = System.Drawing.SystemColors.Window;
            this.textResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textResult.Location = new System.Drawing.Point(12, 12);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ReadOnly = true;
            this.textResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResult.Size = new System.Drawing.Size(599, 369);
            this.textResult.TabIndex = 0;
            this.textResult.WordWrap = false;
            this.textResult.DragDrop += new System.Windows.Forms.DragEventHandler(this.textResult_DragDrop);
            this.textResult.DragEnter += new System.Windows.Forms.DragEventHandler(this.textResult_DragEnter);
            // 
            // cbMD5
            // 
            this.cbMD5.AutoSize = true;
            this.cbMD5.Checked = true;
            this.cbMD5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMD5.Location = new System.Drawing.Point(0, 0);
            this.cbMD5.Name = "cbMD5";
            this.cbMD5.Size = new System.Drawing.Size(66, 26);
            this.cbMD5.TabIndex = 0;
            this.cbMD5.Text = "MD5";
            this.cbMD5.UseVisualStyleBackColor = true;
            // 
            // cbSHA1
            // 
            this.cbSHA1.AutoSize = true;
            this.cbSHA1.Checked = true;
            this.cbSHA1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSHA1.Location = new System.Drawing.Point(0, 34);
            this.cbSHA1.Name = "cbSHA1";
            this.cbSHA1.Size = new System.Drawing.Size(76, 26);
            this.cbSHA1.TabIndex = 1;
            this.cbSHA1.Text = "SHA1";
            this.cbSHA1.UseVisualStyleBackColor = true;
            // 
            // cbSHA256
            // 
            this.cbSHA256.AutoSize = true;
            this.cbSHA256.Location = new System.Drawing.Point(0, 68);
            this.cbSHA256.Name = "cbSHA256";
            this.cbSHA256.Size = new System.Drawing.Size(96, 26);
            this.cbSHA256.TabIndex = 2;
            this.cbSHA256.Text = "SHA256";
            this.cbSHA256.UseVisualStyleBackColor = true;
            // 
            // cbSHA3
            // 
            this.cbSHA3.AutoSize = true;
            this.cbSHA3.Location = new System.Drawing.Point(0, 102);
            this.cbSHA3.Name = "cbSHA3";
            this.cbSHA3.Size = new System.Drawing.Size(76, 26);
            this.cbSHA3.TabIndex = 3;
            this.cbSHA3.Text = "SHA3";
            this.cbSHA3.UseVisualStyleBackColor = true;
            // 
            // cbSM3
            // 
            this.cbSM3.AutoSize = true;
            this.cbSM3.Location = new System.Drawing.Point(0, 136);
            this.cbSM3.Name = "cbSM3";
            this.cbSM3.Size = new System.Drawing.Size(66, 26);
            this.cbSM3.TabIndex = 4;
            this.cbSM3.Text = "SM3";
            this.cbSM3.UseVisualStyleBackColor = true;
            // 
            // cbCRC32
            // 
            this.cbCRC32.AutoSize = true;
            this.cbCRC32.Checked = true;
            this.cbCRC32.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCRC32.Location = new System.Drawing.Point(0, 171);
            this.cbCRC32.Name = "cbCRC32";
            this.cbCRC32.Size = new System.Drawing.Size(86, 26);
            this.cbCRC32.TabIndex = 5;
            this.cbCRC32.Text = "CRC32";
            this.cbCRC32.UseVisualStyleBackColor = true;
            // 
            // algPanel
            // 
            this.algPanel.Controls.Add(this.cbCRC32);
            this.algPanel.Controls.Add(this.cbSM3);
            this.algPanel.Controls.Add(this.cbSHA3);
            this.algPanel.Controls.Add(this.cbSHA256);
            this.algPanel.Controls.Add(this.cbSHA1);
            this.algPanel.Controls.Add(this.cbMD5);
            this.algPanel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.algPanel.Location = new System.Drawing.Point(617, 12);
            this.algPanel.Name = "algPanel";
            this.algPanel.Size = new System.Drawing.Size(99, 369);
            this.algPanel.TabIndex = 1;
            // 
            // btnOpen
            // 
            this.btnOpen.AutoSize = true;
            this.btnOpen.Location = new System.Drawing.Point(3, 3);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(134, 34);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开&(O)";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // labelSingle
            // 
            this.labelSingle.AutoSize = true;
            this.labelSingle.Location = new System.Drawing.Point(9, 433);
            this.labelSingle.Name = "labelSingle";
            this.labelSingle.Size = new System.Drawing.Size(82, 24);
            this.labelSingle.TabIndex = 3;
            this.labelSingle.Text = "当前进度";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(9, 459);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(64, 24);
            this.labelTotal.TabIndex = 4;
            this.labelTotal.Text = "总进度";
            // 
            // pbSingle
            // 
            this.pbSingle.Location = new System.Drawing.Point(98, 433);
            this.pbSingle.MarqueeAnimationSpeed = 10;
            this.pbSingle.Name = "pbSingle";
            this.pbSingle.Size = new System.Drawing.Size(615, 23);
            this.pbSingle.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbSingle.TabIndex = 5;
            // 
            // pbTotal
            // 
            this.pbTotal.Location = new System.Drawing.Point(98, 459);
            this.pbTotal.MarqueeAnimationSpeed = 10;
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(615, 23);
            this.pbTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbTotal.TabIndex = 6;
            // 
            // btnClear
            // 
            this.btnClear.AutoSize = true;
            this.btnClear.Location = new System.Drawing.Point(143, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(134, 34);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "清除&(L)";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.AutoSize = true;
            this.btnCopy.Location = new System.Drawing.Point(283, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(134, 34);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.Text = "复制&(C)";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnStop
            // 
            this.btnStop.AutoSize = true;
            this.btnStop.Location = new System.Drawing.Point(563, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(138, 34);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "停止&(T)";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.Location = new System.Drawing.Point(423, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(134, 34);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存&(S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlButton
            // 
            this.pnlButton.ColumnCount = 5;
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlButton.Controls.Add(this.btnStop, 4, 0);
            this.pnlButton.Controls.Add(this.btnOpen, 0, 0);
            this.pnlButton.Controls.Add(this.btnSave, 3, 0);
            this.pnlButton.Controls.Add(this.btnClear, 1, 0);
            this.pnlButton.Controls.Add(this.btnCopy, 2, 0);
            this.pnlButton.Location = new System.Drawing.Point(12, 387);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.RowCount = 1;
            this.pnlButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlButton.Size = new System.Drawing.Size(704, 40);
            this.pnlButton.TabIndex = 11;
            // 
            // MainWnd
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(728, 494);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.pbTotal);
            this.Controls.Add(this.pbSingle);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelSingle);
            this.Controls.Add(this.algPanel);
            this.Controls.Add(this.textResult);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainWnd";
            this.Text = "Hashtool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.algPanel.ResumeLayout(false);
            this.algPanel.PerformLayout();
            this.pnlButton.ResumeLayout(false);
            this.pnlButton.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textResult;
        private System.Windows.Forms.CheckBox cbMD5;
        private System.Windows.Forms.CheckBox cbSHA1;
        private System.Windows.Forms.CheckBox cbSHA256;
        private System.Windows.Forms.CheckBox cbSHA3;
        private System.Windows.Forms.CheckBox cbSM3;
        private System.Windows.Forms.CheckBox cbCRC32;
        private System.Windows.Forms.Panel algPanel;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label labelSingle;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.ProgressBar pbSingle;
        private System.Windows.Forms.ProgressBar pbTotal;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel pnlButton;
    }
}

