namespace YuriInstaller
{
    partial class _StartWindow
    {
        /// <summary>必需的设计器变量。</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>清理所有正在使用的资源。</summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_StartWindow));
            this.bottomBar = new YuriInstaller.MizukiTools.BottomBarLabel();
            this.rbtnMusicOnOff = new YuriInstaller.MizukiTools.SwitchButton();
            this.rbtnSoundOnOff = new YuriInstaller.MizukiTools.SwitchButton();
            this.bottomButton1 = new YuriInstaller.MizukiTools.RightBarButton();
            this.bottomButton2 = new YuriInstaller.MizukiTools.RightBarButton();
            this.bottomButton3 = new YuriInstaller.MizukiTools.RightBarButton();
            this.bottomButton4 = new YuriInstaller.MizukiTools.SwitchButton();
            this.rightTopLabel = new YuriInstaller.MizukiTools.LightColorLabel();
            this.licenseSider = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.lblInstallerVer = new YuriInstaller.MizukiTools.LightColorLabel();
            this.glslmd = new System.Windows.Forms.Panel();
            this.bottomInstallTip = new YuriInstaller.MizukiTools.OtherColorLabel();
            this.LoadingText = new YuriInstaller.MizukiTools.OtherColorLabel();
            this.contentTree = new YuriInstaller.MizukiTools.MizukiTreeView();
            this.pathInfo = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.InstallSetting = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.InstallSyoukai1 = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.InstallSyoukai2 = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.EndTitle = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.EndLabel1 = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.EndLabel2 = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.lblCredits = new YuriInstaller.MizukiTools.LightColorLabel();
            this.lblCreditBtn = new YuriInstaller.MizukiTools.LightColorLabel();
            this.progressBar1 = new YuriInstaller.MizukiTools.MizukiProgressBar();
            this.bottomButtonBase = new System.Windows.Forms.PictureBox();
            this.selFolderTitle = new YuriInstaller.MizukiTools.HutsuuColorLabel();
            this.bgPaints = new System.Windows.Forms.PictureBox();
            this.bgSwitchTimer = new System.Windows.Forms.Timer(this.components);
            this.glslmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomButtonBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgPaints)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomBar
            // 
            this.bottomBar.BackColor = System.Drawing.Color.Transparent;
            this.bottomBar.ForeColor = System.Drawing.Color.Yellow;
            this.bottomBar.Location = new System.Drawing.Point(12, 574);
            this.bottomBar.Name = "bottomBar";
            this.bottomBar.Size = new System.Drawing.Size(610, 24);
            this.bottomBar.TabIndex = 15;
            this.bottomBar.Text = "label1";
            this.bottomBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbtnMusicOnOff
            // 
            this.rbtnMusicOnOff.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rbtnMusicOnOff.BackgroundImage")));
            this.rbtnMusicOnOff.BtnID = 0;
            this.rbtnMusicOnOff.Clicked = true;
            this.rbtnMusicOnOff.ClickedText = "buttonMusic";
            this.rbtnMusicOnOff.ForeColor = System.Drawing.Color.Yellow;
            this.rbtnMusicOnOff.Image = null;
            this.rbtnMusicOnOff.Location = new System.Drawing.Point(644, 198);
            this.rbtnMusicOnOff.Name = "rbtnMusicOnOff";
            this.rbtnMusicOnOff.Size = new System.Drawing.Size(156, 42);
            this.rbtnMusicOnOff.TabIndex = 14;
            this.rbtnMusicOnOff.Text = "buttonMusic";
            this.rbtnMusicOnOff.UnclickedText = "buttonMusic";
            this.rbtnMusicOnOff.ClickedChanged += new System.EventHandler(this.MusicOnOff_MouseClick);
            // 
            // rbtnSoundOnOff
            // 
            this.rbtnSoundOnOff.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rbtnSoundOnOff.BackgroundImage")));
            this.rbtnSoundOnOff.BtnID = 0;
            this.rbtnSoundOnOff.Clicked = true;
            this.rbtnSoundOnOff.ClickedText = "buttonSound";
            this.rbtnSoundOnOff.ForeColor = System.Drawing.Color.Yellow;
            this.rbtnSoundOnOff.Image = null;
            this.rbtnSoundOnOff.Location = new System.Drawing.Point(644, 240);
            this.rbtnSoundOnOff.Name = "rbtnSoundOnOff";
            this.rbtnSoundOnOff.Size = new System.Drawing.Size(156, 42);
            this.rbtnSoundOnOff.TabIndex = 14;
            this.rbtnSoundOnOff.Text = "buttonSound";
            this.rbtnSoundOnOff.UnclickedText = "buttonSound";
            this.rbtnSoundOnOff.ClickedChanged += new System.EventHandler(this.SoundOnOff_MouseClick);
            // 
            // bottomButton1
            // 
            this.bottomButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bottomButton1.BackgroundImage")));
            this.bottomButton1.BtnID = 1;
            this.bottomButton1.ForeColor = System.Drawing.Color.Yellow;
            this.bottomButton1.Image = null;
            this.bottomButton1.Location = new System.Drawing.Point(645, 535);
            this.bottomButton1.Name = "bottomButton1";
            this.bottomButton1.Size = new System.Drawing.Size(156, 42);
            this.bottomButton1.TabIndex = 5;
            this.bottomButton1.Text = "button1";
            this.bottomButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BottomButton1_MouseClick);
            this.bottomButton1.MouseEnter += new System.EventHandler(this.Rbtn_MouseEnter);
            // 
            // bottomButton2
            // 
            this.bottomButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bottomButton2.BackgroundImage")));
            this.bottomButton2.BtnID = 2;
            this.bottomButton2.ForeColor = System.Drawing.Color.Yellow;
            this.bottomButton2.Image = null;
            this.bottomButton2.Location = new System.Drawing.Point(645, 493);
            this.bottomButton2.Name = "bottomButton2";
            this.bottomButton2.Size = new System.Drawing.Size(156, 42);
            this.bottomButton2.TabIndex = 4;
            this.bottomButton2.Text = "button2";
            this.bottomButton2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BottomButton2_MouseClick);
            this.bottomButton2.MouseEnter += new System.EventHandler(this.Rbtn_MouseEnter);
            // 
            // bottomButton3
            // 
            this.bottomButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bottomButton3.BackgroundImage")));
            this.bottomButton3.BtnID = 3;
            this.bottomButton3.ForeColor = System.Drawing.Color.Yellow;
            this.bottomButton3.Image = null;
            this.bottomButton3.Location = new System.Drawing.Point(645, 451);
            this.bottomButton3.Name = "bottomButton3";
            this.bottomButton3.Size = new System.Drawing.Size(156, 42);
            this.bottomButton3.State = YuriInstaller.MizukiTools.ButtonStates.Hide;
            this.bottomButton3.TabIndex = 3;
            this.bottomButton3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BottomButton3_MouseClick);
            this.bottomButton3.MouseEnter += new System.EventHandler(this.Rbtn_MouseEnter);
            // 
            // bottomButton4
            // 
            this.bottomButton4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bottomButton4.BackgroundImage")));
            this.bottomButton4.BtnID = 4;
            this.bottomButton4.ForeColor = System.Drawing.Color.Yellow;
            this.bottomButton4.Image = null;
            this.bottomButton4.Location = new System.Drawing.Point(645, 409);
            this.bottomButton4.Name = "bottomButton4";
            this.bottomButton4.Size = new System.Drawing.Size(156, 42);
            this.bottomButton4.State = YuriInstaller.MizukiTools.ButtonStates.Hide;
            this.bottomButton4.TabIndex = 2;
            this.bottomButton4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BottomButton4_MouseClick);
            this.bottomButton4.MouseEnter += new System.EventHandler(this.Rbtn_MouseEnter);
            // 
            // rightTopLabel
            // 
            this.rightTopLabel.AutoSize = true;
            this.rightTopLabel.BackColor = System.Drawing.Color.Transparent;
            this.rightTopLabel.ForeColor = System.Drawing.Color.Yellow;
            this.rightTopLabel.Location = new System.Drawing.Point(645, 39);
            this.rightTopLabel.MinimumSize = new System.Drawing.Size(144, 112);
            this.rightTopLabel.Name = "rightTopLabel";
            this.rightTopLabel.Size = new System.Drawing.Size(144, 112);
            this.rightTopLabel.TabIndex = 6;
            this.rightTopLabel.Text = "label1";
            this.rightTopLabel.SizeChanged += new System.EventHandler(this.RightTopPanel_SizeChanged);
            this.rightTopLabel.TextChanged += new System.EventHandler(this.RightTopPanel_TextChanged);
            this.rightTopLabel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RightTopLabel_MouseDoubleClick);
            // 
            // licenseSider
            // 
            this.licenseSider.AutoSize = true;
            this.licenseSider.BackColor = System.Drawing.Color.Transparent;
            this.licenseSider.Enabled = false;
            this.licenseSider.ForeColor = System.Drawing.Color.Red;
            this.licenseSider.Location = new System.Drawing.Point(265, 111);
            this.licenseSider.Name = "licenseSider";
            this.licenseSider.Size = new System.Drawing.Size(41, 12);
            this.licenseSider.TabIndex = 0;
            this.licenseSider.Text = "label1";
            this.licenseSider.Visible = false;
            // 
            // lblInstallerVer
            // 
            this.lblInstallerVer.BackColor = System.Drawing.Color.Transparent;
            this.lblInstallerVer.ForeColor = System.Drawing.Color.Yellow;
            this.lblInstallerVer.Location = new System.Drawing.Point(639, 579);
            this.lblInstallerVer.Name = "lblInstallerVer";
            this.lblInstallerVer.Size = new System.Drawing.Size(160, 20);
            this.lblInstallerVer.TabIndex = 7;
            this.lblInstallerVer.Text = "label1";
            this.lblInstallerVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // glslmd
            // 
            this.glslmd.BackgroundImage = global::YuriInstaller.Properties.Resources.glslmd;
            this.glslmd.Controls.Add(this.bottomInstallTip);
            this.glslmd.Controls.Add(this.LoadingText);
            this.glslmd.Location = new System.Drawing.Point(0, 0);
            this.glslmd.Name = "glslmd";
            this.glslmd.Size = new System.Drawing.Size(200, 100);
            this.glslmd.TabIndex = 0;
            // 
            // bottomInstallTip
            // 
            this.bottomInstallTip.BackColor = System.Drawing.Color.Transparent;
            this.bottomInstallTip.ForeColor = System.Drawing.Color.White;
            this.bottomInstallTip.Location = new System.Drawing.Point(0, 554);
            this.bottomInstallTip.Name = "bottomInstallTip";
            this.bottomInstallTip.Size = new System.Drawing.Size(800, 46);
            this.bottomInstallTip.TabIndex = 0;
            this.bottomInstallTip.Text = "label2";
            this.bottomInstallTip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadingText
            // 
            this.LoadingText.AutoSize = true;
            this.LoadingText.BackColor = System.Drawing.Color.Transparent;
            this.LoadingText.ForeColor = System.Drawing.Color.White;
            this.LoadingText.Location = new System.Drawing.Point(10, 10);
            this.LoadingText.Name = "LoadingText";
            this.LoadingText.Size = new System.Drawing.Size(41, 12);
            this.LoadingText.TabIndex = 1;
            this.LoadingText.Text = "label1";
            // 
            // contentTree
            // 
            this.contentTree.BackColor = System.Drawing.Color.Black;
            this.contentTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contentTree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.contentTree.ForeColor = System.Drawing.Color.Red;
            this.contentTree.HideSelection = false;
            this.contentTree.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.contentTree.Indent = 18;
            this.contentTree.LineColor = System.Drawing.Color.Empty;
            this.contentTree.Location = new System.Drawing.Point(0, 0);
            this.contentTree.Name = "contentTree";
            this.contentTree.Scrollable = false;
            this.contentTree.ShowLines = false;
            this.contentTree.ShowNodeToolTips = true;
            this.contentTree.ShowPlusMinus = false;
            this.contentTree.ShowRootLines = false;
            this.contentTree.Size = new System.Drawing.Size(471, 263);
            this.contentTree.TabIndex = 0;
            this.contentTree.TabStop = false;
            this.contentTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.ContentTree_AfterExpanse);
            this.contentTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ContentTree_BeforeExpand);
            this.contentTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.ContentTree_AfterExpanse);
            this.contentTree.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.ContentTree_DrawNode);
            this.contentTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ContentTree_AfterSelect);
            // 
            // pathInfo
            // 
            this.pathInfo.AutoSize = true;
            this.pathInfo.BackColor = System.Drawing.Color.Transparent;
            this.pathInfo.ForeColor = System.Drawing.Color.Red;
            this.pathInfo.Location = new System.Drawing.Point(64, 120);
            this.pathInfo.Name = "pathInfo";
            this.pathInfo.Size = new System.Drawing.Size(41, 12);
            this.pathInfo.TabIndex = 9;
            this.pathInfo.Text = "label1";
            this.pathInfo.Visible = false;
            // 
            // InstallSetting
            // 
            this.InstallSetting.AutoSize = true;
            this.InstallSetting.BackColor = System.Drawing.Color.Transparent;
            this.InstallSetting.ForeColor = System.Drawing.Color.Red;
            this.InstallSetting.Location = new System.Drawing.Point(64, 55);
            this.InstallSetting.Name = "InstallSetting";
            this.InstallSetting.Size = new System.Drawing.Size(89, 12);
            this.InstallSetting.TabIndex = 10;
            this.InstallSetting.Text = "InstallSetting";
            this.InstallSetting.Visible = false;
            // 
            // InstallSyoukai1
            // 
            this.InstallSyoukai1.AutoSize = true;
            this.InstallSyoukai1.BackColor = System.Drawing.Color.Transparent;
            this.InstallSyoukai1.ForeColor = System.Drawing.Color.Red;
            this.InstallSyoukai1.Location = new System.Drawing.Point(64, 55);
            this.InstallSyoukai1.Name = "InstallSyoukai1";
            this.InstallSyoukai1.Size = new System.Drawing.Size(95, 12);
            this.InstallSyoukai1.TabIndex = 10;
            this.InstallSyoukai1.Text = "InstallSyoukai1";
            this.InstallSyoukai1.Visible = false;
            // 
            // InstallSyoukai2
            // 
            this.InstallSyoukai2.AutoSize = true;
            this.InstallSyoukai2.BackColor = System.Drawing.Color.Transparent;
            this.InstallSyoukai2.ForeColor = System.Drawing.Color.Red;
            this.InstallSyoukai2.Location = new System.Drawing.Point(64, 55);
            this.InstallSyoukai2.Name = "InstallSyoukai2";
            this.InstallSyoukai2.Size = new System.Drawing.Size(95, 12);
            this.InstallSyoukai2.TabIndex = 10;
            this.InstallSyoukai2.Text = "InstallSyoukai2";
            this.InstallSyoukai2.Visible = false;
            // 
            // EndTitle
            // 
            this.EndTitle.AutoSize = true;
            this.EndTitle.BackColor = System.Drawing.Color.Transparent;
            this.EndTitle.ForeColor = System.Drawing.Color.Red;
            this.EndTitle.Location = new System.Drawing.Point(64, 55);
            this.EndTitle.Name = "EndTitle";
            this.EndTitle.Size = new System.Drawing.Size(53, 12);
            this.EndTitle.TabIndex = 10;
            this.EndTitle.Text = "EndTitle";
            this.EndTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EndTitle.Visible = false;
            // 
            // EndLabel1
            // 
            this.EndLabel1.AutoSize = true;
            this.EndLabel1.BackColor = System.Drawing.Color.Transparent;
            this.EndLabel1.ForeColor = System.Drawing.Color.Red;
            this.EndLabel1.Location = new System.Drawing.Point(64, 55);
            this.EndLabel1.Name = "EndLabel1";
            this.EndLabel1.Size = new System.Drawing.Size(59, 12);
            this.EndLabel1.TabIndex = 10;
            this.EndLabel1.Text = "EndLabel1";
            this.EndLabel1.Visible = false;
            // 
            // EndLabel2
            // 
            this.EndLabel2.AutoSize = true;
            this.EndLabel2.BackColor = System.Drawing.Color.Transparent;
            this.EndLabel2.ForeColor = System.Drawing.Color.Red;
            this.EndLabel2.Location = new System.Drawing.Point(64, 55);
            this.EndLabel2.Name = "EndLabel2";
            this.EndLabel2.Size = new System.Drawing.Size(59, 12);
            this.EndLabel2.TabIndex = 10;
            this.EndLabel2.Text = "EndLabel2";
            this.EndLabel2.Visible = false;
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.BackColor = System.Drawing.Color.Black;
            this.lblCredits.ForeColor = System.Drawing.Color.Yellow;
            this.lblCredits.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblCredits.Location = new System.Drawing.Point(0, 0);
            this.lblCredits.MaximumSize = new System.Drawing.Size(800, 2147483647);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(65, 12);
            this.lblCredits.TabIndex = 11;
            this.lblCredits.Text = "lblCredits";
            this.lblCredits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCredits.Visible = false;
            this.lblCredits.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Credits_MouseClick);
            // 
            // lblCreditBtn
            // 
            this.lblCreditBtn.BackColor = System.Drawing.Color.Transparent;
            this.lblCreditBtn.ForeColor = System.Drawing.Color.Yellow;
            this.lblCreditBtn.Location = new System.Drawing.Point(657, 3);
            this.lblCreditBtn.Name = "lblCreditBtn";
            this.lblCreditBtn.Size = new System.Drawing.Size(118, 15);
            this.lblCreditBtn.TabIndex = 12;
            this.lblCreditBtn.Text = "lblCreditBtn";
            this.lblCreditBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCreditBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CreditBtn_MouseClick);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.Black;
            this.progressBar1.Location = new System.Drawing.Point(7, 577);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(619, 18);
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Visible = false;
            // 
            // bottomButtonBase
            // 
            this.bottomButtonBase.BackgroundImage = global::YuriInstaller.Properties.Resources.ButtonNull;
            this.bottomButtonBase.Location = new System.Drawing.Point(645, 409);
            this.bottomButtonBase.Name = "bottomButtonBase";
            this.bottomButtonBase.Size = new System.Drawing.Size(156, 168);
            this.bottomButtonBase.TabIndex = 13;
            this.bottomButtonBase.TabStop = false;
            // 
            // selFolderTitle
            // 
            this.selFolderTitle.AutoSize = true;
            this.selFolderTitle.BackColor = System.Drawing.Color.Transparent;
            this.selFolderTitle.ForeColor = System.Drawing.Color.Red;
            this.selFolderTitle.Location = new System.Drawing.Point(64, 120);
            this.selFolderTitle.Name = "selFolderTitle";
            this.selFolderTitle.Size = new System.Drawing.Size(41, 12);
            this.selFolderTitle.TabIndex = 8;
            this.selFolderTitle.Text = "label1";
            this.selFolderTitle.Visible = false;
            // 
            // bgPaints
            // 
            this.bgPaints.BackColor = System.Drawing.Color.Transparent;
            this.bgPaints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bgPaints.Location = new System.Drawing.Point(0, 0);
            this.bgPaints.Name = "bgPaints";
            this.bgPaints.Size = new System.Drawing.Size(332, 293);
            this.bgPaints.TabIndex = 16;
            this.bgPaints.TabStop = false;
            this.bgPaints.Visible = false;
            // 
            // bgSwitchTimer
            // 
            this.bgSwitchTimer.Interval = 3000;
            this.bgSwitchTimer.Tick += new System.EventHandler(this.BgSwitchTimer_Tick);
            // 
            // _StartWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::YuriInstaller.Properties.Resources.Binary_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.glslmd);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.bottomButton4);
            this.Controls.Add(this.bottomButton3);
            this.Controls.Add(this.bottomButton2);
            this.Controls.Add(this.bottomButton1);
            this.Controls.Add(this.rightTopLabel);
            this.Controls.Add(this.lblInstallerVer);
            this.Controls.Add(this.selFolderTitle);
            this.Controls.Add(this.pathInfo);
            this.Controls.Add(this.InstallSetting);
            this.Controls.Add(this.InstallSyoukai1);
            this.Controls.Add(this.InstallSyoukai2);
            this.Controls.Add(this.EndTitle);
            this.Controls.Add(this.EndLabel1);
            this.Controls.Add(this.EndLabel2);
            this.Controls.Add(this.lblCreditBtn);
            this.Controls.Add(this.bottomButtonBase);
            this.Controls.Add(this.rbtnMusicOnOff);
            this.Controls.Add(this.rbtnSoundOnOff);
            this.Controls.Add(this.bgPaints);
            this.Controls.Add(this.bottomBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::YuriInstaller.Properties.Resources.icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "_StartWindow";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StartWindow_FormClosed);
            this.Load += new System.EventHandler(this.StartWindow_Load);
            this.Shown += new System.EventHandler(this.StartWindow_Shown);
            this.MouseEnter += new System.EventHandler(this.StartWindow_MouseEnter);
            this.glslmd.ResumeLayout(false);
            this.glslmd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomButtonBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgPaints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private YuriInstaller.MizukiTools.BottomBarLabel bottomBar;
        private YuriInstaller.MizukiTools.RightBarButton bottomButton1;
        private YuriInstaller.MizukiTools.RightBarButton bottomButton2;
        private YuriInstaller.MizukiTools.RightBarButton bottomButton3;
        private YuriInstaller.MizukiTools.SwitchButton bottomButton4;
        private YuriInstaller.MizukiTools.LightColorLabel rightTopLabel;
        private YuriInstaller.MizukiTools.LightColorLabel lblInstallerVer;
        private System.Windows.Forms.Panel glslmd;
        private YuriInstaller.MizukiTools.OtherColorLabel bottomInstallTip;
        private YuriInstaller.MizukiTools.OtherColorLabel LoadingText;
        private YuriInstaller.MizukiTools.MizukiTreeView contentTree;
        private YuriInstaller.MizukiTools.HutsuuColorLabel pathInfo;
        private YuriInstaller.MizukiTools.HutsuuColorLabel InstallSetting;
        private YuriInstaller.MizukiTools.HutsuuColorLabel InstallSyoukai1;
        private YuriInstaller.MizukiTools.HutsuuColorLabel InstallSyoukai2;
        private YuriInstaller.MizukiTools.HutsuuColorLabel EndTitle;
        private YuriInstaller.MizukiTools.HutsuuColorLabel EndLabel1;
        private YuriInstaller.MizukiTools.HutsuuColorLabel EndLabel2;
        private YuriInstaller.MizukiTools.LightColorLabel lblCredits;
        private YuriInstaller.MizukiTools.LightColorLabel lblCreditBtn;
        private YuriInstaller.MizukiTools.MizukiProgressBar progressBar1;
        private System.Windows.Forms.PictureBox bottomButtonBase;
        private YuriInstaller.MizukiTools.SwitchButton rbtnMusicOnOff;
        private YuriInstaller.MizukiTools.SwitchButton rbtnSoundOnOff;
        private YuriInstaller.MizukiTools.HutsuuColorLabel licenseSider;
        private YuriInstaller.MizukiTools.HutsuuColorLabel selFolderTitle;
        private System.Windows.Forms.PictureBox bgPaints;
        private System.Windows.Forms.Timer bgSwitchTimer;
    }
}

