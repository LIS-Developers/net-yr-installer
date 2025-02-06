using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace YuriInstaller.MizukiTools
{
    partial class Comment
    {
        /*
            这里存放文本框控件。
         */
    }

    /// <summary>用于多行的文本框（自动配色、屏蔽文本鼠标）。</summary>
    public class MizukiTextBoxBase : TextBox, IBBM
    {
        public static BottomBarLabel BtmBar { get; set; }

        [DefaultValue(null)]
        public string BtmbarTip { get; set; }

        /// <summary>鼠标是否在上面。</summary>
        [DefaultValue(false)]
        public bool IsMouseOn { get; private set; } = false;

        /// <summary>用户是否已经修改文本。</summary>
        [DefaultValue(false)]
        public bool KeyPressed { get; set; } = false;

        [DefaultValue(false)]
        public new bool TabStop
        {
            get => base.TabStop;
            set => base.TabStop = value;
        }

        [DefaultValue(ImeMode.Disable)]
        public new ImeMode ImeMode
        {
            get => base.ImeMode;
            set => base.ImeMode = value;
        }

        [DefaultValue(false)]
        public new bool ShortcutsEnabled
        {
            get => base.ShortcutsEnabled;
            set => base.ShortcutsEnabled = value;
        }

        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set => base.BorderStyle = value;
        }

        public MizukiTextBoxBase() : base()
        {
            TabStop = false;
            ImeMode = ImeMode.Disable;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.None;
            ShortcutsEnabled = false;
            KeyPress += Mtb_KeyPress;
            ForeColor = ColorBoard.HutsuuTextColor;
            Cursor = Program.ReleaseCursor;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            IsMouseOn = true;
            BtmBar?.ScrollDisplayOnBtmBar(this);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            IsMouseOn = false;
            if (BtmBar != null)
            {
                BtmBar.Text = string.Empty;
            }
            base.OnMouseLeave(e);
        }

        private void Mtb_KeyPress(object sender, EventArgs e)
        {
            if (!ReadOnly)
            {
                KeyPressed = true;
                KeyPress -= Mtb_KeyPress;
            }
        }

        public override void ResetForeColor()
        {
            ForeColor = ColorBoard.HutsuuTextColor;
        }

        public virtual bool ShouldSerializeForeColor()
        {
            return ForeColor != ColorBoard.HutsuuTextColor;
        }

        public override void ResetBackColor()
        {
            BackColor = Color.Black;
        }

        public virtual bool ShouldSerializeBackColor()
        {
            return BackColor != Color.Black;
        }
    }

    /// <summary>用于单行文本，不自动换行，且切换颜色。</summary>
    public class MizukiTextBox : MizukiTextBoxBase
    {
        [DefaultValue(false)]
        public new bool WordWrap
        {
            get => base.WordWrap;
            set => base.WordWrap = value;
        }

        public MizukiTextBox() : base()
        {
            WordWrap = false;
        }

        // 路径框得到焦点后变亮。
        protected override void OnEnter(EventArgs e)
        {
            ForeColor = ColorBoard.LightTextColor;
            base.OnEnter(e);
        }

        // 失去焦点变暗
        protected override void OnLeave(EventArgs e)
        {
            ForeColor = ColorBoard.HutsuuTextColor;
            base.OnLeave(e);
        }
    }
}
