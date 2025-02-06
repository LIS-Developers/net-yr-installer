namespace YuriInstaller.ExtraWindows
{
    partial class Background
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
            this.WindowTitle = new YuriInstaller.MizukiTools.ShadowLabel();
            this.SuspendLayout();
            // 
            // WindowTitle
            // 
            this.WindowTitle.BackColor = System.Drawing.Color.Transparent;
            this.WindowTitle.ForeColor = System.Drawing.Color.White;
            this.WindowTitle.IgnoreEnabled = true;
            this.WindowTitle.Location = new System.Drawing.Point(0, 0);
            this.WindowTitle.Name = "WindowTitle";
            this.WindowTitle.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.WindowTitle.ShadowOffset = 5;
            this.WindowTitle.Size = new System.Drawing.Size(0, 12);
            this.WindowTitle.TabIndex = 1;
            // 
            // Background
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WindowTitle);
            this.Icon = global::YuriInstaller.Properties.Resources.bgicon;
            this.Name = "Background";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.Background_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Background_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private YuriInstaller.MizukiTools.ShadowLabel WindowTitle;
    }
}