using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YuriInstaller.Properties;

namespace YuriInstaller.MizukiTools
{
    partial class Comment
    {
        /*
            这里存放按钮控件。
         */
    }

    public class MizukiButton : Button, IBBM
    {
        public event EventHandler ImageChanged;

        public new Image Image
        {
            get => base.Image;
            set
            {
                base.Image = value;
                OnImageChanged(EventArgs.Empty);
            }
        }

        /// <summary>显示提示信息的底部栏。</summary>
        public static BottomBarLabel BtmBar { get; set; }

        [DefaultValue(null)]
        public string BtmbarTip { get; set; }

        /// <summary>鼠标是否放在上面。</summary>
        public bool IsMouseOn { get; private set; } = false;

        protected virtual void OnImageChanged(EventArgs e)
        {
            ImageChanged?.Invoke(this, e);
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
    }

    /// <summary>无焦点按钮。</summary>
    public class MizukiButtonWithoutFocus : MizukiButton
    {
        protected override bool ShowFocusCues => false;

        /// <summary>点击后将焦点到哪里，避免产生焦点边框。</summary>
        public static Control FocusTo { get; set; }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get => base.TabStop;
            set => base.TabStop = value;
        }

        [DefaultValue(false)]
        public new bool UseVisualStyleBackColor
        {
            get => base.UseVisualStyleBackColor;
            set => base.UseVisualStyleBackColor = value;
        }

        public MizukiButtonWithoutFocus() : base()
        {
            UseVisualStyleBackColor = false;
            TabStop = false;
        }

        protected override void OnEnter(EventArgs e)
        {
            FocusTo?.Focus();
            base.OnEnter(e);
        }
    }

    /// <summary>平的按钮。</summary>
    public class FlatButton : MizukiButtonWithoutFocus
    {
        [DefaultValue(FlatStyle.Flat)]
        public new FlatStyle FlatStyle
        {
            get => base.FlatStyle;
            set => base.FlatStyle = value;
        }

        public FlatButton() : base()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }
    }

    /// <summary>默认显示亮色的平按钮。</summary>
    public abstract class ColorFlatButton : FlatButton
    {
        protected virtual Color _foreColor { get; }

    }

    /// <summary>默认显示亮色的平按钮。</summary>
    public class HutsuuFlatButton : ColorFlatButton
    {
        public HutsuuFlatButton() : base()
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

    /// <summary>默认显示亮色的平按钮。</summary>
    public class LightFlatButton : ColorFlatButton
    {
        public LightFlatButton() : base()
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

    /// <summary>默认显示亮色的按钮。</summary>
    public class NoticeDialogButton : LightFlatButton
    {
        /// <summary>空闲按钮图片。</summary>
        public static Image IdleImage { get; } = Resources.dialogBtn;

        /// <summary>点燃按钮图片。</summary>
        public static Image ActiveImage { get; } = Resources.dialogbtnLight;

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get => base.BackgroundImage;
            set
            {
                return;
            }
        }

        public NoticeDialogButton() : base()
        {
            BackColor = Color.Yellow;
            base.BackgroundImage = IdleImage;
        }

        public override void ResetBackColor()
        {
            BackColor = Color.Yellow;
        }

        public bool ShouldSerializeBackColor()
        {
            return BackColor != Color.Yellow;
        }

        // 有了焦点更换图片

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.BackgroundImage = ActiveImage;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.BackgroundImage = IdleImage;
            base.OnMouseLeave(e);
        }
    }

    /// <summary>用于右侧的按钮，已设好默认属性。</summary>
    public class RightBarButton : LightFlatButton, IBBM
    {
        private ButtonStates _state;

        /// <summary>设置按钮状态（显示的图片）。</summary>
        [DefaultValue(ButtonStates.Normal)]
        public ButtonStates State
        {
            get => _state;
            set
            {
                _state = value;
                BackgroundImage = ButtonTexture[(int)value];
            }
        }

        public bool CantUse => State == ButtonStates.Hide || State == ButtonStates.MoveB;

        public new bool Enabled
        {
            get => !CantUse && base.Enabled;
            set
            {
                base.Enabled = value;
                if (value)
                {
                    State = ButtonStates.Normal;
                }
            }
        }

        /// <summary>按钮的图片，0为红色正常，1为按下点亮，2是按钮灰色背景，3是黑掉的按钮。</summary>
        public static Bitmap[] ButtonTexture { get; } = new Bitmap[4]
        {
            Resources.ButtonNormal,
            Resources.ButtonLight,
            Resources.ButtonNone,
            Resources.ButtonClosed
        };

        /// <summary>按钮按下声。<br />
        /// Button click sound.</summary>
        public static MizukiWaveOutEvent ButtonSound { get; } = new MizukiWaveOutEvent(Resources.ButtonClick);

        /// <summary>按钮的ID，对应底部栏提示信息数组中的哪一行列。</summary>
        public int BtnID { get; set; } = 0;

        [DefaultValue(ImageLayout.Stretch)]
        public new ImageLayout BackgroundImageLayout
        {
            get => base.BackgroundImageLayout;
            set => base.BackgroundImageLayout = value;
        }

        [DefaultValue(TextImageRelation.ImageBeforeText)]
        public new TextImageRelation TextImageRelation
        {
            get => base.TextImageRelation;
            set => base.TextImageRelation = value;
        }

        [DefaultValue(ContentAlignment.MiddleCenter)]
        public new ContentAlignment ImageAlign
        {
            get => base.ImageAlign;
            set => base.ImageAlign = value;
        }

        [DefaultValue(ContentAlignment.MiddleCenter)]
        public new ContentAlignment TextAlign
        {
            get => base.TextAlign;
            set => base.TextAlign = value;
        }

        private static Size _buttonSize = new Size(156, 42);

        public override string Text
        {
            get => CantUse ? string.Empty : base.Text;
            set => base.Text = value;
        }

        public RightBarButton() : base()
        {
            Size = _buttonSize;
            TextImageRelation = TextImageRelation.ImageBeforeText;
            ImageAlign = ContentAlignment.MiddleCenter;
            TextAlign = ContentAlignment.MiddleCenter;
            BackgroundImageLayout = ImageLayout.Stretch;
            State = ButtonStates.Normal;
        }

        public void ResetSize()
        {
            Size = _buttonSize;
        }

        public virtual bool ShouldSerializeSize()
        {
            return Size != _buttonSize;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            ButtonSound.Replay();
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            BackgroundImage = ButtonTexture[1];
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            BackgroundImage = ButtonTexture[0];
            base.OnMouseUp(mevent);
        }

        /// <summary>按钮的文本非空时更改文本。<br />
        /// Change button text if button text is not null or empty.</summary>
        public void ReplaceText(string a)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                Text = a;
            }
        }
    }

    public class SwitchButton : RightBarButton
    {
        public event EventHandler ClickedChanged;

        private bool _clicked = false;
        private string _unclickedText;
        private string _clickedText;

        /// <summary>按钮处于开启状态。</summary>
        // [Browsable(true), Description("按钮是否已处于按下状态。"), Category("Controls")]
        [DefaultValue(false)]
        public bool Clicked
        {
            get => _clicked;
            set
            {
                _clicked = value;
                base.Text = _clicked ? _clickedText : _unclickedText;
                OnClickedChange();
            }
        }

        /// <summary>按下后显示的文本。</summary>
        [DefaultValue(null)]
        public string ClickedText
        {
            get => _clickedText;
            set
            {
                _clickedText = value;
                if (_clicked)
                {
                    base.Text = _clickedText;
                }
            }
        }

        /// <summary>弹起后显示的文本。</summary>
        [DefaultValue(null)]
        public string UnclickedText
        {
            get => _unclickedText;
            set
            {
                _unclickedText = value;
                if (!_clicked)
                {
                    base.Text = _unclickedText;
                }
            }
        }

        [Browsable(false)]
        public override string Text
        {
            get => base.Text;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    base.Text = _unclickedText = _clickedText = value;
                }
            }
        }

        public SwitchButton() : base()
        {
        }

        protected virtual void OnClickedChange()
        {
            ClickedChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Clicked = !Clicked;
            base.OnMouseClick(e);
        }
    }
}
