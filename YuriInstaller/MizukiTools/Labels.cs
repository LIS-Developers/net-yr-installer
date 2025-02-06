using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using YuriInstaller.Properties;

namespace YuriInstaller.MizukiTools
{
    partial class Comment
    {
        /*
            这里用于存放标签控件。
         */
    }

    /// <summary>选择框选择的样式。<br />
    /// Styles of CheckBoxes.</summary>
    public static class CheckBoxStyles
    {
        public static Image[] Classic = new Image[2] { Resources.Unselected, Resources.Selected };
        public static Image[] Cross = new Image[2] { Resources.Unselect2, Resources.Selected2 };
    }

    /// <summary>这里让Label不再双击复制文本。<br />
    /// I don't want the label copy its text to my clipboard when I double click it.</summary>
    public class LRXLabel : Label
    {
        private const int WM_LBUTTONDCLICK = 0x203;
        private string clipboardText;
        public string BackupText { get; private set; }

        [DefaultValue(false)]
        [Description("Overrides default behavior of Label to copy label text to clipboard on double click")]
        public bool CopyTextOnDoubleClick { get; set; }

        public new Font Font
        {
            get => base.Font;
            set
            {
                if (base.Font != value)
                {
                    base.Font = value;
                }
            }
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                }
            }
        }

        /// <summary>备份并设置标签的文字。</summary>
        /// <param name="text">设置文字</param>
        public void Backup(string text)
        {
            BackupText = Text = text;
        }

        /// <summary>备份标签的文字。</summary>
        public void Backup()
        {
            BackupText = Text;
        }

        /// <summary>备份标签的文字。</summary>
        public void Restore()
        {
            Text = BackupText;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (!string.IsNullOrEmpty(clipboardText))
            {
                Clipboard.SetData(DataFormats.Text, clipboardText);
            }
            clipboardText = null;
            base.OnDoubleClick(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (!CopyTextOnDoubleClick)
            {
                if (m.Msg == WM_LBUTTONDCLICK)
                {
                    IDataObject d = Clipboard.GetDataObject();
                    if (d.GetDataPresent(DataFormats.Text))
                        clipboardText = (string)d.GetData(DataFormats.Text);
                }
            }
            base.WndProc(ref m);
        }

        [DefaultValue(false)]
        public new bool UseMnemonic
        {
            get => base.UseMnemonic;
            set => base.UseMnemonic = value;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(false)]
        public bool Eventing { get; set; } = false;

        public LRXLabel() : base()
        {
            UseMnemonic = false;
        }
    }

    public class ProgressLabel : LRXLabel
    {
        public ProgressLabel() : base()
        {
            Location = MizukiTool.ZeroPoint;
            TextAlign = ContentAlignment.MiddleCenter;
            Visible = true;
            BackColor = Color.Transparent;
        }
    }

    /// <summary>透明背景标签。</summary>
    public class TransparentBackColorLabel : LRXLabel
    {
        private string _otext;

        public override string Text
        {
            get => base.Text;
            set => base.Text = _otext = value;
        }

        [DefaultValue(null)]
        public string MouseUpingText { get; set; } = null;

        public TransparentBackColorLabel() : base()
        {
            BackColor = Color.Transparent;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!string.IsNullOrEmpty(MouseUpingText))
                base.Text = MouseUpingText;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.Text = _otext;
            base.OnMouseLeave(e);
        }

        public override void ResetBackColor()
        {
            BackColor = Color.Transparent;
        }

        public virtual bool ShouldSerializeBackColor()
        {
            return BackColor != Color.Transparent;
        }
    }

    public abstract class SelfPaintingLabel : TransparentBackColorLabel
    {
        protected static Color disabledColor1 = Color.Gray;
        protected static Color disabledColor2 = Color.Black;
        private bool ignoreEnabled = false;

        protected bool IgnoredP => Enabled || IgnoreEnabled;

        /// <summary>
        /// 是否无视“禁止”属性来绘画。
        /// </summary>
        [DefaultValue(false)]
        public bool IgnoreEnabled
        {
            get => ignoreEnabled;
            set
            {
                ignoreEnabled = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            {
                return;
            }

            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
        }
    }

    /// <summary>带描边的标签。<br /><br />
    /// 来自https://blog.csdn.net/linxinfa/article/details/113929095。</summary>
    public class LabelExt : SelfPaintingLabel
    {
        private Color _borderColor = Color.White;
        private int _borderWidth = 1;

        /// <summary>
        /// 边宽
        /// </summary>
        [DefaultValue(1)]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 边颜色
        /// </summary>
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawText(e.Graphics);
        }

        private void DrawText(Graphics g)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            {
                return;
            }

            using (GraphicsPath gp = new GraphicsPath())
            using (Pen outline = new Pen(IgnoredP ? BorderColor : disabledColor2, BorderWidth)
            { LineJoin = LineJoin.Round })
            using (Brush foreBrush = new SolidBrush(IgnoredP ? ForeColor : disabledColor1))
            {
                gp.AddString(Text, Font.FontFamily, (int)Font.Style,
                    Font.Size, ClientRectangle, new StringFormat());
                g.ScaleTransform(1.3f, 1.35f);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawPath(outline, gp);
                g.FillPath(foreBrush, gp);
            }
        }

        public virtual void ResetBorderColor()
        {
            BorderColor = Color.White;
        }

        public virtual bool ShouldSerializeBorderColor()
        {
            return BorderColor != Color.White;
        }
    }

    /// <summary>带阴影的标签</summary>
    public class ShadowLabel : SelfPaintingLabel
    {
        private static readonly Color _defShadow = Color.FromArgb(100, 0, 0, 0);
        private Color _shadow = _defShadow;

        public Color ShadowColor
        {
            get => _shadow;
            set
            {
                _shadow = value;
                Invalidate();
            }
        }

        public int ShadowOffset { get; set; } = 5;

        public ShadowLabel() : base()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (SolidBrush srcBrush = new SolidBrush(IgnoredP ? ForeColor : disabledColor1))
            using (SolidBrush shadowBrush = new SolidBrush(IgnoredP ? ShadowColor : disabledColor2))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                };

                // 绘制阴影和文字
                e.Graphics.DrawString(Text, Font, shadowBrush, ShadowOffset, ShadowOffset, sf);
                e.Graphics.DrawString(Text, Font, srcBrush, 0, 0, sf);
            }
        }

        public virtual void ResetShadowColor()
        {
            ShadowColor = _defShadow;
        }

        public virtual bool ShouldSerializeShadow()
        {
            return ShadowColor != _defShadow;
        }
    }

    public class HutsuuColorLabel : TransparentBackColorLabel
    {
        public HutsuuColorLabel() : base()
        {
            ForeColor = ColorBoard.HutsuuTextColor;
        }

        public override void ResetForeColor()
        {
            ForeColor = ColorBoard.HutsuuTextColor;
        }

        public virtual bool ShouldSerializeForeColor()
        {
            return ForeColor != ColorBoard.HutsuuTextColor;
        }
    }

    public class OtherColorLabel : TransparentBackColorLabel
    {
        public OtherColorLabel() : base()
        {
            ForeColor = ColorBoard.SpecialColor;
        }

        public override void ResetForeColor()
        {
            ForeColor = ColorBoard.SpecialColor;
        }

        public virtual bool ShouldSerializeForeColor()
        {
            return ForeColor != ColorBoard.SpecialColor;
        }
    }

    public class LightColorLabel : TransparentBackColorLabel
    {
        public LightColorLabel() : base()
        {
            ForeColor = ColorBoard.LightTextColor;
        }

        public override void ResetForeColor()
        {
            ForeColor = ColorBoard.LightTextColor;
        }

        public virtual bool ShouldSerializeForeColor()
        {
            return ForeColor != ColorBoard.LightTextColor;
        }
    }


    public class BottomBarLabel : LightColorLabel
    {
        /// <summary>将文字显示在底边栏，目前仅支持关联部分控件。</summary>
        public void ScrollDisplayOnBtmBar(IBBM imo, string message = null)
        {
            if (!((Control)imo).Enabled)
            {
                return;
            }

            message = message ?? imo.BtmbarTip;

            if (Program.L10N[Program.Lang].L10n.DynamicBottomBarText && !string.IsNullOrEmpty(message))
            {
                MizukiTool.MM_BeginPeriod(1);
                var sb = new StringBuilder(message.Length);
                foreach (var i in message.ToCharArray())
                {
                    if (!imo.IsMouseOn)
                    {
                        Text = string.Empty;
                        goto skip;
                    }
                    Thread.Sleep(2);
                    sb.Append(i);
                    Text = sb.ToString();
                    Update();
                }
            skip:
                MizukiTool.MM_EndPeriod(1);
                return;
            }
            Debug.WriteLine("跳过1");
        }
    }
}
