
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
            this.textResult = new System.Windows.Forms.TextBox();
            this.cbMD5 = new System.Windows.Forms.CheckBox();
            this.cbSHA1 = new System.Windows.Forms.CheckBox();
            this.cbSHA2_256 = new System.Windows.Forms.CheckBox();
            this.cbSHA3_256 = new System.Windows.Forms.CheckBox();
            this.cbSM3 = new System.Windows.Forms.CheckBox();
            this.cbCRC32 = new System.Windows.Forms.CheckBox();
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
            this.btnAbout = new System.Windows.Forms.Button();
            this.cbSHA2_512 = new System.Windows.Forms.CheckBox();
            this.algPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelStatus = new System.Windows.Forms.TableLayoutPanel();
            this.panelSetting = new System.Windows.Forms.TableLayoutPanel();
            this.cbUpperCase = new System.Windows.Forms.CheckBox();
            this.cbUseMultiThread = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlButton.SuspendLayout();
            this.algPanel.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // textResult
            // 
            this.textResult.AllowDrop = true;
            this.textResult.BackColor = System.Drawing.SystemColors.Window;
            this.textResult.Location = new System.Drawing.Point(12, 12);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ReadOnly = true;
            this.textResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResult.Size = new System.Drawing.Size(707, 369);
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
            this.cbMD5.Location = new System.Drawing.Point(0, 1);
            this.cbMD5.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbMD5.Name = "cbMD5";
            this.cbMD5.Size = new System.Drawing.Size(76, 28);
            this.cbMD5.TabIndex = 0;
            this.cbMD5.Text = "MD5";
            this.cbMD5.UseVisualStyleBackColor = true;
            // 
            // cbSHA1
            // 
            this.cbSHA1.AutoSize = true;
            this.cbSHA1.Checked = true;
            this.cbSHA1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSHA1.Location = new System.Drawing.Point(0, 31);
            this.cbSHA1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbSHA1.Name = "cbSHA1";
            this.cbSHA1.Size = new System.Drawing.Size(83, 28);
            this.cbSHA1.TabIndex = 1;
            this.cbSHA1.Text = "SHA1";
            this.cbSHA1.UseVisualStyleBackColor = true;
            // 
            // cbSHA2_256
            // 
            this.cbSHA2_256.AutoSize = true;
            this.cbSHA2_256.Location = new System.Drawing.Point(0, 61);
            this.cbSHA2_256.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbSHA2_256.Name = "cbSHA2_256";
            this.cbSHA2_256.Size = new System.Drawing.Size(127, 28);
            this.cbSHA2_256.TabIndex = 2;
            this.cbSHA2_256.Text = "SHA2-256";
            this.cbSHA2_256.UseVisualStyleBackColor = true;
            // 
            // cbSHA3_256
            // 
            this.cbSHA3_256.AutoSize = true;
            this.cbSHA3_256.Location = new System.Drawing.Point(0, 121);
            this.cbSHA3_256.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbSHA3_256.Name = "cbSHA3_256";
            this.cbSHA3_256.Size = new System.Drawing.Size(127, 28);
            this.cbSHA3_256.TabIndex = 3;
            this.cbSHA3_256.Text = "SHA3-256";
            this.cbSHA3_256.UseVisualStyleBackColor = true;
            // 
            // cbSM3
            // 
            this.cbSM3.AutoSize = true;
            this.cbSM3.Location = new System.Drawing.Point(0, 151);
            this.cbSM3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbSM3.Name = "cbSM3";
            this.cbSM3.Size = new System.Drawing.Size(75, 28);
            this.cbSM3.TabIndex = 4;
            this.cbSM3.Text = "SM3";
            this.cbSM3.UseVisualStyleBackColor = true;
            // 
            // cbCRC32
            // 
            this.cbCRC32.AutoSize = true;
            this.cbCRC32.Checked = true;
            this.cbCRC32.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCRC32.Location = new System.Drawing.Point(0, 181);
            this.cbCRC32.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbCRC32.Name = "cbCRC32";
            this.cbCRC32.Size = new System.Drawing.Size(92, 28);
            this.cbCRC32.TabIndex = 5;
            this.cbCRC32.Text = "CRC32";
            this.cbCRC32.UseVisualStyleBackColor = true;
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
            this.labelSingle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSingle.AutoSize = true;
            this.labelSingle.Location = new System.Drawing.Point(2, 3);
            this.labelSingle.Margin = new System.Windows.Forms.Padding(0, 1, 1, 1);
            this.labelSingle.Name = "labelSingle";
            this.labelSingle.Size = new System.Drawing.Size(82, 24);
            this.labelSingle.TabIndex = 3;
            this.labelSingle.Text = "当前进度";
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(2, 29);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(0, 1, 1, 1);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(64, 24);
            this.labelTotal.TabIndex = 4;
            this.labelTotal.Text = "总进度";
            // 
            // pbSingle
            // 
            this.pbSingle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbSingle.Location = new System.Drawing.Point(86, 3);
            this.pbSingle.Margin = new System.Windows.Forms.Padding(1);
            this.pbSingle.MarqueeAnimationSpeed = 1000;
            this.pbSingle.Maximum = 10000;
            this.pbSingle.Name = "pbSingle";
            this.pbSingle.Size = new System.Drawing.Size(751, 24);
            this.pbSingle.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbSingle.TabIndex = 5;
            // 
            // pbTotal
            // 
            this.pbTotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbTotal.Location = new System.Drawing.Point(86, 29);
            this.pbTotal.Margin = new System.Windows.Forms.Padding(1);
            this.pbTotal.MarqueeAnimationSpeed = 1000;
            this.pbTotal.Maximum = 10000;
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(751, 24);
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
            this.btnStop.Size = new System.Drawing.Size(134, 34);
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
            this.pnlButton.ColumnCount = 6;
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.pnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.pnlButton.Controls.Add(this.btnOpen, 0, 0);
            this.pnlButton.Controls.Add(this.btnSave, 3, 0);
            this.pnlButton.Controls.Add(this.btnClear, 1, 0);
            this.pnlButton.Controls.Add(this.btnCopy, 2, 0);
            this.pnlButton.Controls.Add(this.btnStop, 4, 0);
            this.pnlButton.Controls.Add(this.btnAbout, 5, 0);
            this.pnlButton.Location = new System.Drawing.Point(12, 382);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.RowCount = 1;
            this.pnlButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlButton.Size = new System.Drawing.Size(840, 40);
            this.pnlButton.TabIndex = 11;
            // 
            // btnAbout
            // 
            this.btnAbout.AutoSize = true;
            this.btnAbout.Location = new System.Drawing.Point(703, 3);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(134, 34);
            this.btnAbout.TabIndex = 11;
            this.btnAbout.Text = "关于&(A)";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // cbSHA2_512
            // 
            this.cbSHA2_512.AutoSize = true;
            this.cbSHA2_512.Location = new System.Drawing.Point(0, 91);
            this.cbSHA2_512.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbSHA2_512.Name = "cbSHA2_512";
            this.cbSHA2_512.Size = new System.Drawing.Size(124, 28);
            this.cbSHA2_512.TabIndex = 6;
            this.cbSHA2_512.Text = "SHA2-512";
            this.cbSHA2_512.UseVisualStyleBackColor = true;
            // 
            // algPanel
            // 
            this.algPanel.AutoSize = true;
            this.algPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algPanel.ColumnCount = 1;
            this.algPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.algPanel.Controls.Add(this.cbMD5, 0, 0);
            this.algPanel.Controls.Add(this.cbCRC32, 0, 6);
            this.algPanel.Controls.Add(this.cbSHA2_512, 0, 3);
            this.algPanel.Controls.Add(this.cbSM3, 0, 5);
            this.algPanel.Controls.Add(this.cbSHA1, 0, 1);
            this.algPanel.Controls.Add(this.cbSHA2_256, 0, 2);
            this.algPanel.Controls.Add(this.cbSHA3_256, 0, 4);
            this.algPanel.Location = new System.Drawing.Point(725, 12);
            this.algPanel.Name = "algPanel";
            this.algPanel.RowCount = 7;
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.algPanel.Size = new System.Drawing.Size(127, 210);
            this.algPanel.TabIndex = 12;
            // 
            // panelStatus
            // 
            this.panelStatus.AutoSize = true;
            this.panelStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelStatus.ColumnCount = 2;
            this.panelStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelStatus.Controls.Add(this.labelTotal, 0, 1);
            this.panelStatus.Controls.Add(this.pbTotal, 1, 1);
            this.panelStatus.Controls.Add(this.pbSingle, 1, 0);
            this.panelStatus.Controls.Add(this.labelSingle, 0, 0);
            this.panelStatus.Location = new System.Drawing.Point(12, 425);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Padding = new System.Windows.Forms.Padding(2);
            this.panelStatus.RowCount = 2;
            this.panelStatus.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelStatus.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelStatus.Size = new System.Drawing.Size(840, 56);
            this.panelStatus.TabIndex = 13;
            // 
            // panelSetting
            // 
            this.panelSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSetting.AutoSize = true;
            this.panelSetting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSetting.ColumnCount = 1;
            this.panelSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSetting.Controls.Add(this.cbUpperCase, 0, 0);
            this.panelSetting.Controls.Add(this.cbUseMultiThread, 0, 1);
            this.panelSetting.Location = new System.Drawing.Point(725, 321);
            this.panelSetting.Name = "panelSetting";
            this.panelSetting.RowCount = 2;
            this.panelSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSetting.Size = new System.Drawing.Size(108, 60);
            this.panelSetting.TabIndex = 14;
            // 
            // cbUpperCase
            // 
            this.cbUpperCase.AutoSize = true;
            this.cbUpperCase.Checked = true;
            this.cbUpperCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUpperCase.Location = new System.Drawing.Point(0, 1);
            this.cbUpperCase.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbUpperCase.Name = "cbUpperCase";
            this.cbUpperCase.Size = new System.Drawing.Size(108, 28);
            this.cbUpperCase.TabIndex = 0;
            this.cbUpperCase.Text = "结果大写";
            this.toolTip1.SetToolTip(this.cbUpperCase, "在结果中输出大写字母的哈希值");
            this.cbUpperCase.UseVisualStyleBackColor = true;
            // 
            // cbUseMultiThread
            // 
            this.cbUseMultiThread.AutoSize = true;
            this.cbUseMultiThread.Checked = true;
            this.cbUseMultiThread.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseMultiThread.Location = new System.Drawing.Point(0, 31);
            this.cbUseMultiThread.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbUseMultiThread.Name = "cbUseMultiThread";
            this.cbUseMultiThread.Size = new System.Drawing.Size(90, 28);
            this.cbUseMultiThread.TabIndex = 1;
            this.cbUseMultiThread.Text = "多线程";
            this.toolTip1.SetToolTip(this.cbUseMultiThread, "使用多线程同时计算所有算法");
            this.cbUseMultiThread.UseVisualStyleBackColor = true;
            // 
            // MainWnd
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(864, 486);
            this.Controls.Add(this.panelSetting);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.algPanel);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.textResult);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainWnd";
            this.Text = "Hashtool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlButton.ResumeLayout(false);
            this.pnlButton.PerformLayout();
            this.algPanel.ResumeLayout(false);
            this.algPanel.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.panelSetting.ResumeLayout(false);
            this.panelSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textResult;
        private System.Windows.Forms.CheckBox cbMD5;
        private System.Windows.Forms.CheckBox cbSHA1;
        private System.Windows.Forms.CheckBox cbSHA2_256;
        private System.Windows.Forms.CheckBox cbSHA3_256;
        private System.Windows.Forms.CheckBox cbSM3;
        private System.Windows.Forms.CheckBox cbCRC32;
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
        private System.Windows.Forms.CheckBox cbSHA2_512;
        private System.Windows.Forms.TableLayoutPanel algPanel;
        private System.Windows.Forms.TableLayoutPanel panelStatus;
        private System.Windows.Forms.TableLayoutPanel panelSetting;
        private System.Windows.Forms.CheckBox cbUpperCase;
        private System.Windows.Forms.CheckBox cbUseMultiThread;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAbout;
    }
}

