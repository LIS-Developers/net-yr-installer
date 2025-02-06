using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;

/*
    古希腊掌管背景的神。
    安装程序的背景，不允许用户操作，且安装程序作为模式对话框。
 */

namespace YuriInstaller.ExtraWindows
{
    /// <summary>背景窗口。</summary>
    internal sealed partial class Background : DoubleBufferedForm
    {
        /*   
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_STYLE = -16;
        public const int WS_DISABLED = 0x8000000;

        /// <summary>这样设置的控件禁用后不变灰。<br />
        /// The control won't become grey if set Enabled to false through this method.</summary>
        public static void SetControlEnabled(Control c, bool enabled)
        {
            SetWindowLong(c.Handle, GWL_STYLE,
                (enabled ? (~WS_DISABLED) : WS_DISABLED) & GetWindowLong(c.Handle, GWL_STYLE));
        }
        */

        public Background(Form form)
        {
            Show();
            InitializeComponent();
            Retext(form.Text);
            form.ShowInTaskbar = false;
            form.Owner = this;
            Enabled = false;
        }

        /// <summary>更新背景文本。</summary>
        internal void Retext(string windowTitle)
        {
            Text = windowTitle;
            if (string.IsNullOrEmpty(Program.L10N[Program.Lang].L10n.BackgroundText))
                return;
            WindowTitle.Text = Program.L10N[Program.Lang].L10n.BackgroundText;
            WindowTitle.Font = Program.L10N[Program.Lang].L10n.BackgroundFont;
        }

        /// <summary>实例化刷子，第一个参数指示上色区域，第二个和第三个参数分别渐变颜色的开始和结束，第四个参数表示颜色的方向。</summary>
        private void Background_Paint(object sender, PaintEventArgs e)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            {
                return;
            }

            WindowTitle.Size = Size;
            e.Graphics.FillRectangle(new LinearGradientBrush(ClientRectangle,
                Color.Blue, Color.Black, LinearGradientMode.Vertical), ClientRectangle);
        }

        private void Background_Shown(object sender, EventArgs e)
        {
            var ads = new LabelExt()
            {
                Name = "ads",
                Size = new Size(240, 130),
                Text = "广告位招租\n廣告位招租",
                Font = new Font("Microsoft YaHei UI", 30F, FontStyle.Bold, GraphicsUnit.Point, 134),
                IgnoreEnabled = true,
                BorderColor = Color.White
            };

            void SetAdLocation() => ads.Location = new Point((Width - ads.Width) / 2, (Height - ads.Height) / 2);
            SizeChanged += (object sender1, EventArgs e1) => SetAdLocation();
            SetAdLocation();
            Controls.Add(ads);
            ads.BringToFront();
        }
    }
}
