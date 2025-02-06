using System;
using System.Diagnostics;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;

/*
    古希腊掌管弹窗的神。
    使用MizukiTools.弹窗(string msg); // 函数弹出对话框，msg为显示的信息。
    使用MizukiTools.弹窗Yesno(string msg); // 返回用户点了是还是否。
    使用MizukiTools.错误弹窗(Exception ex[, string msg]); // 弹出错误，msg为显示的信息。
    使用MizukiTools.错误弹窗Yesno(Exception ex[, string msg]); // 返回用户点了是还是否。
 */

namespace YuriInstaller.ExtraWindows
{
    // 程序出现的提示信息。
    public sealed partial class NoticeDialog : DoubleBufferedForm
    {
        public static implicit operator bool(NoticeDialog nd) => nd.ToBoolean();
        private bool selection = false;

        private NoticeDialog(bool yesno)
        {
            InitializeComponent();
            Cursor = Program.ReleaseCursor;

            noButton.Font = label1.Font =
            yesButton.Font = Program.L10N[Program.Lang].L10n.GeneralFont;

            yesButton.Text = Program.L10N[Program.Lang].L10n.Button_OK;

            if (yesno)
            {
                noButton.Text = Program.L10N[Program.Lang].L10n.Button_Cancel;
                yesButton.Visible = true;
            }
            else
            {
                noButton.Text = Program.L10N[Program.Lang].L10n.Button_OK;
                yesButton.Visible = false;
            }
        }

        // 设置具体弹出的窗口。
        public NoticeDialog(string message, bool i = false) : this(i)
        {
            Debug.WriteLine(message);
            label1.Text = message;
            ShowDialog(Program.AppForm);
        }

        // 设置具体弹出的窗口。
        public NoticeDialog(Exception ex, string message, bool i = false) : this(i)
        {
            Debug.WriteLine(ex.ToString());
            label1.Text = string.IsNullOrEmpty(message) ? ex.Message : message;
            toolTip1.SetToolTip(label1, ex.ToString());
            ShowDialog(Program.AppForm);
        }

        private void Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (((Button)sender) == yesButton)
            {
                selection = true;
            }
            label1.Focus();
            Close();
        }

        public bool ToBoolean() => selection;
    }
}
