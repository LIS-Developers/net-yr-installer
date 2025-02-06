using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;

/*
    古希腊掌管报错的神，也是很多红警玩家这辈子的痛。一般用在try{}catch{}上，使用弹窗()、弹窗Yesno()、错误弹窗()召唤。
 */

namespace YuriInstaller.ExtraWindows
{
    /// <summary>弹窗用窗口。</summary>
    internal sealed partial class IEWindows : DoubleBufferedForm
    {
        private static readonly string logPath = Path.Combine(Program.DesktopPath, @"except.txt");
        /// <summary>弹框报错的窗口。</summary>
        /// <param name="message">显示的消息。</param>
        public IEWindows(string message)
        {
            InitializeComponent();
            // Cursor = MizukiTools.ReleaseCursor;

            Text = _StartWindow._RegeditName;
            label1.Text = $"{_StartWindow._RegeditName} Installer has encountered an internal error and is unable to continue normally.";
            label2.Text = $"Please visit our website at {_StartWindow._AuthorHomepage} for the latest updates and technical support.";
            label2.Font = label1.Font;
            label2.BackColor = label1.BackColor;

            File.WriteAllText(logPath, message);
            ShowDialog();
        }

        /// <summary>OK按钮点击事件。</summary>
        private void Button1_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
            Process.Start(logPath);
            Application.Exit();
        }
    }
}
