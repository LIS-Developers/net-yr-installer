namespace YuriInstaller.ExtraWindows
{
    partial class NoticeDialog
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>Required method for Designer support - do not modify
        /// the contents of this method with the code editor.</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoticeDialog));
            this.label1 = new YuriInstaller.MizukiTools.LightColorLabel();
            this.noButton = new YuriInstaller.MizukiTools.NoticeDialogButton();
            this.yesButton = new YuriInstaller.MizukiTools.NoticeDialogButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(60, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 144);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // noButton
            // 
            this.noButton.BackColor = System.Drawing.Color.Yellow;
            this.noButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("noButton.BackgroundImage")));
            this.noButton.ForeColor = System.Drawing.Color.Yellow;
            this.noButton.Image = null;
            this.noButton.Location = new System.Drawing.Point(312, 285);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(126, 25);
            this.noButton.TabIndex = 1;
            this.noButton.Text = "Nobutton";
            this.noButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Button_MouseClick);
            // 
            // yesButton
            // 
            this.yesButton.BackColor = System.Drawing.Color.Yellow;
            this.yesButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("yesButton.BackgroundImage")));
            this.yesButton.ForeColor = System.Drawing.Color.Yellow;
            this.yesButton.Image = null;
            this.yesButton.Location = new System.Drawing.Point(312, 253);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(126, 25);
            this.yesButton.TabIndex = 0;
            this.yesButton.Text = "Yesbutton";
            this.yesButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Button_MouseClick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // NoticeDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::YuriInstaller.Properties.Resources.pudlgbgn;
            this.ClientSize = new System.Drawing.Size(452, 327);
            this.ControlBox = false;
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoticeDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ResumeLayout(false);

        }

        #endregion
        
        private YuriInstaller.MizukiTools.LightColorLabel label1;
        private YuriInstaller.MizukiTools.NoticeDialogButton noButton;
        private YuriInstaller.MizukiTools.NoticeDialogButton yesButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}