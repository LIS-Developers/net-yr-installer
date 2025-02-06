using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using YuriInstaller.Properties;

namespace YuriInstaller.MizukiTools
{
    partial class Comment
    {
        /*
            这里存放容器类控件。
         */
    }

    public class MizukiProgressBar : Panel
    {
        private readonly ProgressLabel _progressDisplay;
        private readonly Panel _progressBar;
        private readonly ProgressLabel _inProgressDisplay;
        private int _progress;
        private string _progressFormat;

        [DefaultValue(0)]
        public int Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    _progressBar.Width = Width * _progress / 100;
                    Text = _progress.ToString();
                    Application.DoEvents();
                }
            }
        }

        [DefaultValue(null)]
        public string ProgressFormat
        {
            get => _progressFormat;
            set
            {
                _progressFormat = value;
                Text = _progress.ToString();
            }
        }

        public new string Text
        {
            get => _inProgressDisplay.Text;
            private set
            {
                string i = value;
                if (!string.IsNullOrWhiteSpace(_progressFormat))
                {
                    i = string.Format(_progressFormat, i);
                }
                _progressDisplay.Text = _inProgressDisplay.Text = i;
            }
        }

        /// <summary>显示进度。</summary>
        [DefaultValue(true)]
        public bool ShowText
        {
            get => _inProgressDisplay.Visible;
            set => _inProgressDisplay.Visible = _progressDisplay.Visible = value;
        }

        /// <summary>进度条的颜色。</summary>
        public Color BarColor
        {
            get => _progressBar.BackColor;
            set => _progressBar.BackColor = value;
        }

        /// <summary>进度条显示的文字颜色。</summary>
        public new Color ForeColor
        {
            get => _inProgressDisplay.ForeColor;
            set => _progressDisplay.ForeColor = _inProgressDisplay.ForeColor = value;
        }

        /// <summary>设置进度条显示的字体。</summary>
        public new Font Font
        {
            get => _inProgressDisplay.Font;
            set => _progressDisplay.Font = _inProgressDisplay.Font = value;
        }

        public MizukiProgressBar() : base()
        {
            _progressDisplay = new ProgressLabel
            {
                Name = "_progressDisplay",
                Size = Size,
            };

            _inProgressDisplay = new ProgressLabel
            {
                Name = "_inProgressDisplay",
                Size = Size,
            };

            _progressBar = new Panel();
            _progressBar.SuspendLayout();
            _progressBar.Location = MizukiTool.ZeroPoint;
            _progressBar.Name = "_progressBar";
            _progressBar.Size = new Size(0, Height);
            _progressBar.Controls.Add(_inProgressDisplay);

            BackColor = Color.White;
            BarColor = Color.Green;
            ForeColor = Color.Black;

            Controls.Add(_progressBar);
            Controls.Add(_progressDisplay);
            _progressBar.ResumeLayout(false);
            _progressBar.PerformLayout();
        }

        public virtual void ResetBarColor()
        {
            BarColor = Color.Green;
        }

        public virtual bool ShouldSerializeBarColor()
        {
            return BarColor != Color.Green;
        }

        public override void ResetBackColor()
        {
            BackColor = Color.White;
        }

        public virtual bool ShouldSerializeBackColor()
        {
            return BackColor != Color.White;
        }

        public override void ResetForeColor()
        {
            ForeColor = Color.Black;
        }

        public virtual bool ShouldSerializeForeColor()
        {
            return ForeColor != Color.Black;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _inProgressDisplay.Size = _progressDisplay.Size = Size;
            _progressBar.Size = new Size(Width * Progress / 100, Height);
            base.OnSizeChanged(e);
        }

        public void Done()
        {
            Progress = 100;
        }
    }

    public class MizukiPanel : Panel, IBBM
    {
        private Color _borderColor = ColorBoard.CtrlBorderColor;

        public static BottomBarLabel BtmBar;

        public bool IsMouseOn { get; private set; } = false;

        public string BtmbarTip { get; set; }

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public MizukiPanel() : base()
        {
            BackColor = Color.Black;
            BorderColor = ColorBoard.CtrlBorderColor;
        }

        public override void ResetBackColor()
        {
            BackColor = Color.Black;
        }

        public virtual bool ShouldSerializeBackColor()
        {
            return BackColor != Color.Black;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
            base.OnPaint(e);
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

    public class ScrollPBPanel : MizukiPanel
    {
        private static Size _buttonSize = new Size(27, 33);
        private int __scrollSize = 3;
        private int _giveUserScSize;
        private int _middleBottom;

        private int PscrollSize
        {
            get => __scrollSize;
            set
            {
                int maxScrollSize = ScrollChanger.Height - ScrollMiddle.Height;
                __scrollSize = value == 0 ? 1 :
                    Math.Abs(value) > Math.Abs(maxScrollSize) ? maxScrollSize : value;
            }
        }

        /// <summary>上下按钮每次移动距离</summary>
        public int ScrollSize
        {
            get => _giveUserScSize;
            set
            {
                _giveUserScSize = PscrollSize = value;
            }
        }

        /// <summary>滚动到最底时。<br />
        /// Event in scrolled to bottom.</summary>
        public event EventHandler ScrolledToBottom;

        /// <summary>滚动时。<br />
        /// Event in scrolls.</summary>
        public event EventHandler Scrolls;

        /// <summary>执行滑块移动事件，用于滚动条中间那个调节器。</summary>
        public bool ControlCanMove { get; set; } = true;

        /// <summary>滚动条中间那个调节器。</summary>
        public PictureBox ScrollMiddle { get; } = new PictureBox();

        /// <summary>向上滚动按钮。</summary>
        public LightFlatButton UpButton { get; } = new LightFlatButton();

        /// <summary>向下滚动按钮。</summary>
        public LightFlatButton DownButton { get; } = new LightFlatButton();

        /// <summary>调节器的底座，调节器只能在那里面滑动。</summary>
        public Panel ScrollChanger { get; } = new Panel();

        /// <summary>被滚动的页面控件，比如标签，图片。</summary>
        public Control Child { get; private set; }

        /// <summary>滚动条的底座，包含了调节器底座，上下按钮。</summary>
        public Panel ScrollBase { get; } = new Panel();

        /// <summary>控件的底座，包含要滚动的东西。</summary>
        public Panel ControlPanel { get; } = new Panel();

        public ScrollPBPanel() : base()
        {
            SuspendLayout();
            ((ISupportInitialize)ScrollMiddle).BeginInit();

            ScrollBase.Name = "ScrollBase";

            UpButton.BackgroundImage = Resources.scrollUp1;
            UpButton.Name = "UpButton";
            UpButton.MouseDown += (object sender, MouseEventArgs e) => UpButton.BackgroundImage = Resources.scrollUp2;
            UpButton.MouseLeave += ScrollUpBtn_MouseLeaveUp;
            UpButton.MouseUp += ScrollUpBtn_MouseLeaveUp;
            UpButton.MouseClick += (object sender, MouseEventArgs e) =>
            {
                ScrollMiddle.Top = Math.Max(ScrollMiddle.Top - __scrollSize, 0);
                UpButton.BackgroundImage = Resources.scrollUp1;
            };

            DownButton.BackgroundImage = Resources.scrollDown1;
            DownButton.Name = "DownButton";
            DownButton.MouseDown += (object sender, MouseEventArgs e) => DownButton.BackgroundImage = Resources.scrollDown2;
            DownButton.MouseLeave += ScrollDownBtn_MouseLeaveUp;
            DownButton.MouseUp += ScrollDownBtn_MouseLeaveUp;
            DownButton.MouseClick += (object sender, MouseEventArgs e) =>
            {
                ScrollMiddle.Top = Math.Min(ScrollMiddle.Top + __scrollSize, _middleBottom);
                DownButton.BackgroundImage = Resources.scrollDown1;
            };

            ScrollChanger.BackgroundImage = Resources.scrollBase;
            ScrollChanger.Name = "ScrollChanger";
            ScrollChanger.MouseClick += (object sender, MouseEventArgs e) => ScrollMiddle.Top = GetMouseTop();

            ScrollMiddle.BackgroundImage = Resources.scrollNone;
            ScrollMiddle.Name = "ScrollMiddle";
            ScrollMiddle.MouseMove += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left)
                    ScrollMiddle.Top = GetMouseTop();
            };
            ScrollMiddle.LocationChanged += (object sender, EventArgs e) =>
            {
                Refreshes();

                if (ScrollMiddle.Top >= _middleBottom)
                {
                    OnScrolledToBottom();
                }
            };

            ControlPanel.Name = "ControlPanel";
            ControlPanel.Top = ControlPanel.Left = 2;

            UpButton.BackgroundImageLayout = DownButton.BackgroundImageLayout =
            ScrollMiddle.BackgroundImageLayout = ScrollChanger.BackgroundImageLayout = ImageLayout.Stretch;

            DownButton.Size = UpButton.Size = ScrollMiddle.Size = _buttonSize;
            ScrollChanger.Width = ScrollBase.Width = _buttonSize.Width;

            ScrollChanger.Controls.Add(ScrollMiddle);
            ScrollBase.Controls.AddRange(new Control[] { ScrollChanger, UpButton, DownButton });
            Controls.AddRange(new Control[] { ScrollBase, ControlPanel });

            ResumeLayout(false);
            PerformLayout();
            ((ISupportInitialize)ScrollMiddle).EndInit();
        }

        private int GetMouseTop() => MizukiTool.Clamp(ScrollChanger.PointToClient(MousePosition).Y - ScrollMiddle.Height / 2, 0, _middleBottom);

        public ScrollPBPanel(Control control) : this()
        {
            SetChild(control);
        }

        public void SetChild(Control control)
        {
            if (Child != null)
            {
                throw new LRXDoesntLoveMeException("无法设置两次Child。");
            }

            Child = control;
            Child.Name = "Child";
            Child.Location = MizukiTool.ZeroPoint;
            ControlPanel.Controls.Add(Child);
            ScrollMiddle.MouseWheel += (object sender, MouseEventArgs e) =>
                Top = MizukiTool.Clamp(
                    Top - e.Delta * (ScrollChanger.Height - Height) / (Child.Height - ScrollBase.Height),
                    0,
                    ScrollChanger.Height - Height);

            ScrollChanger.MouseWheel += (object sender, MouseEventArgs e) =>
            {
                ScrollMiddle.Top = MizukiTool.Clamp(ScrollMiddle.Top - e.Delta * (_middleBottom) / (Child.Height - ScrollBase.Height), 0, _middleBottom);
            };

            Child.MouseWheel += Controling_MouseWheel;
            Child.LocationChanged += Child_LocationChanged;

            Child.SizeChanged += (object sender, EventArgs e) =>
            {
                Debug.WriteLine(Child.Size);

                if (Child.Height <= ControlPanel.Height)
                {
                    Debug.WriteLine("没超过");
                    ControlPanel.Width = Width - 4;
                    ScrollBase.Visible = false;
                }
                else
                {
                    Debug.WriteLine("超过了");
                    ControlPanel.Width = Width - 4 - ScrollBase.Width;
                    ScrollBase.Visible = true;
                }

                if (Child.Height - ControlPanel.Height != 0)
                {
                    SetMiddleTop(MizukiTool.Clamp(-Child.Top * (_middleBottom) / (Child.Height - ControlPanel.Height), 0, _middleBottom));
                }
            };
        }

        /// <summary>手动设置滚动条钮的顶边。</summary>
        private void SetMiddleTop(int top) => BlockScrollEvent(() => ScrollMiddle.Top = top);

        /// <summary>锁定控件不让其跟随滚动钮移动。</summary>
        private void BlockScrollEvent(Action action)
        {
            ControlCanMove = false;
            action();
            ControlCanMove = true;
        }

        public void AddSecondControl(Control control)
        {
            if (Child == null)
            {
                throw new LRXDoesntLoveMeException("还没设第一个控件就想设第二个控件？");
            }

            ControlPanel.Controls.Add(control);
            Child.SizeChanged += (object sender, EventArgs e) => control.Size = Child.Size;
            Child.LocationChanged += (object sender, EventArgs e) => control.Location = Child.Location;
            control.MouseWheel += Controling_MouseWheel;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ControlPanel.Size = new Size(Width - 4 - ScrollBase.Width, Height - 4);

            if (Child != null)
            {
                Child.MaximumSize = new Size(ControlPanel.Width, int.MaxValue);
                Child.MinimumSize = new Size(ControlPanel.Width, 0);
            }

            ScrollBase.Left = Width - ScrollBase.Width;
            ScrollBase.Height = Height;
            ScrollChanger.Top = UpButton.Bottom;
            ScrollChanger.Height = ScrollBase.Height - (UpButton.Height + DownButton.Height);
            DownButton.Top = ScrollBase.Bottom - DownButton.Height;
            _middleBottom = ScrollChanger.Height - ScrollMiddle.Height;

            PscrollSize = PscrollSize;
            base.OnSizeChanged(e);
        }

        private void Child_LocationChanged(object sender, EventArgs e) => OnScrolls();

        private void Controling_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ScrollBase.Visible)
            {
                Control i = (Control)sender;
                int iMin = -(i.Height - ControlPanel.Height);
                i.Top = MizukiTool.Clamp(i.Top + e.Delta, iMin, 0);
                SetMiddleTop(i.Top <= iMin ? _middleBottom : MizukiTool.Clamp(-i.Top * _middleBottom / -iMin, 0, _middleBottom));
            }
        }

        private void ScrollUpBtn_MouseLeaveUp(object sender, EventArgs e)
        {
            UpButton.BackgroundImage = Resources.scrollUp1;
        }

        private void ScrollDownBtn_MouseLeaveUp(object sender, EventArgs e)
        {
            DownButton.BackgroundImage = Resources.scrollDown1;
        }

        /// <summary>刷新控件滚动的位置。<br />
        /// Refresh location of control to scrolling.</summary>
        public void Refreshes()
        {
            if (!ControlCanMove || Child == null)
            {
                return;
            }

            // 当到达最低时，控件也拉到最低。
            if (ScrollMiddle.Top >= _middleBottom)
            {
                Child.Top = -(Child.Height - ControlPanel.Height);
            }
            else
            {
                Child.Top = ScrollMiddle.Top >= _middleBottom ? -(Child.Height - ScrollBase.Height) : -(ScrollMiddle.Top * (Child.Height - ScrollBase.Height) / (_middleBottom));
            }
        }

        protected virtual void OnScrolledToBottom()
        {
            ScrolledToBottom?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnScrolls()
        {
            Scrolls?.Invoke(this, EventArgs.Empty);
        }
    }

    public abstract class TextBoxPanelBase<T> : MizukiPanel where T : TextBox
    {
        [DefaultValue(false)]
        [Description("Don't change width when size changed.")]
        public bool LockWidth { get; set; } = false;

        public T Child { get; set; }

        public Color ActiveBorderColor { get; set; } = ColorBoard.LightBorderColor;

        public Color IdleBorderColor { get; set; } = ColorBoard.CtrlBorderColor;

        public new Font Font
        {
            get => Child.Font;
            set => Child.Font = value;
        }

        public new string Text
        {
            get => Child.Text;
            set => Child.Text = value;
        }

        public int MaxLength
        {
            get => Child.MaxLength;
            set => Child.MaxLength = value;
        }

        public bool ReadOnly
        {
            get => Child.ReadOnly;
            set => Child.ReadOnly = value;
        }

        public CharacterCasing CharacterCasing
        {
            get => Child.CharacterCasing;
            set => Child.CharacterCasing = value;
        }

        public int SelectionStart
        {
            get => Child.SelectionStart;
            set => Child.SelectionStart = value;
        }

        public new ImeMode ImeMode
        {
            get => Child.ImeMode;
            set => Child.ImeMode = value;
        }

        public int Length
        {
            get => Child.Text.Length;
        }

        public TextBoxPanelBase()
        {
            BackColor = Color.Black;
            SizeChanged += AnySizeChanged;

            // 初始化 Child 控件的位置和大小
            Child = Activator.CreateInstance<T>(); // 动态创建控件实例
            Child.Location = new Point(4, 2);
            Child.SizeChanged += AnySizeChanged;
            Child.ForeColor = ColorBoard.HutsuuTextColor;
            Controls.Add(Child);
        }

        private void AnySizeChanged(object sender, EventArgs e)
        {
            Child.Top = Math.Max(0, (Height - Child.Height) / 2);
            if (!LockWidth)
            {
                Child.Width = Math.Max(0, Width - 8);
            }
        }

        public new void Focus()
        {
            Child.Focus();
        }

        // 路径框得到焦点后变亮。
        protected override void OnEnter(EventArgs e)
        {
            BorderColor = ActiveBorderColor;
            base.OnEnter(e);
        }

        // 失去焦点变暗
        protected override void OnLeave(EventArgs e)
        {
            BorderColor = IdleBorderColor;
            base.OnLeave(e);
        }
    }

    public class TextBoxPanel : TextBoxPanelBase<TextBox>
    {
        public TextBoxPanel() : base()
        {
        }
    }

    public class MizukiTextBoxPanel : TextBoxPanelBase<MizukiTextBox>
    {
        public MizukiTextBoxPanel() : base()
        {
        }
    }
}
