using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using YuriInstaller.Properties;

namespace YuriInstaller.MizukiTools
{
    partial class Comment
    {
        /*
            这里存放选择框控件。
            SetupLabel可以设置绑定某个文件路径，DisplayName设置在右上角面板显示成什么，默认是你的路径。
         */
    }

    /// <summary>继承自Label的自定义标签类，用于选择框。</summary>
    public abstract class MizukiCheckBoxBase : HutsuuFlatButton, IBBM
    {
        public event EventHandler CheckedChanged;

        private bool _checked = false;
        private MizukiCheckBox[] _childItem;
        private string _text;

        /// <summary>禁止切换选中状态。<br />
        /// If set, you can't change check status by clicking.</summary>
        [DefaultValue(false)]
        public bool CheckDisabled { get; set; } = false;

        /// <summary>选择框点击声。<br />
        /// Checkbox click sound.</summary>
        public static MizukiWaveOutEvent CheckSound { get; } = new MizukiWaveOutEvent(Resources.checkbox);

        private Size imageSize;
        private int imageBorder;

        /// <summary>子选择框列表。</summary>
        public MizukiCheckBox[] ChildItem
        {
            get => _childItem;
            set
            {
                if (value?.Any(i => i.ChildItem?.Contains(this) == true) == true)
                {
                    throw new LRXDoesntLoveMeException(_StartWindow.Localization.ExceptionRound);
                }
                _childItem = value;
            }
        }

        /// <summary>选择框样式。</summary>
        protected Image[] CheckStyles { get; set; } = CheckBoxStyles.Classic;

        /// <summary>
        /// 需要点击选择框而非文字才起效。
        /// </summary>
        [DefaultValue(false)]
        public static bool NeedClickOnBox { get; set; } = true;

        /// <summary>获取或设置标签的选中状态。</summary>
        [DefaultValue(false)]
        public virtual bool Checked
        {
            get => _checked;
            set
            {
                Debug.WriteLine($"更改选项来自{Name}");
                SetCheck(value);

                if (_childItem?.Any() == true)
                {
                    foreach (var item in _childItem)
                    {
                        item.Enabled = _checked;
                        item.Checked = false;
                    }
                }
                OnCheckedChanged();
            }
        }

        public new Image Image
        {
            get => base.Image;
            set
            {
                return;
            }
        }

        /// <summary>获取或设置标签的文本内容，自动添加空格。</summary>
        public new string Text
        {
            get => _text;
            set
            {
                _text = value;
                base.Text = ($" {value}");
            }
        }

        public MizukiCheckBoxBase() : base()
        {
            AutoSize = true;
            FlatAppearance.MouseDownBackColor =
            FlatAppearance.MouseOverBackColor = BackColor = Color.Transparent;
            ImageAlign = TextAlign = ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.ImageBeforeText;
            Visible = false;
            base.Image = CheckStyles[0];
        }

        protected void SetCheck(bool check)
        {
            _checked = check;
            base.Image = check ? CheckStyles[1] : CheckStyles[0];
        }

        public void SetImage(Image image)
        {
            base.Image = image;
        }

        public virtual void ResetCheckStyles()
        {
            CheckStyles = CheckBoxStyles.Classic;
        }

        public virtual bool ShouldSerializeCheckStyles()
        {
            return CheckStyles != CheckBoxStyles.Classic;
        }

        protected override void OnImageChanged(EventArgs e)
        {
            imageSize = base.Image.Size;
            imageBorder = Math.Max(0, (Height - imageSize.Height) / 2);
            base.OnImageChanged(e);
        }

        protected bool MouseOnImage(Point mousePoint)
        {
            return ImageAlign != ContentAlignment.MiddleLeft ||
                   (mousePoint.X >= 4 && mousePoint.X <= imageSize.Width + imageBorder &&
                    mousePoint.Y >= 4 && mousePoint.Y <= imageSize.Height + imageBorder);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!CheckDisabled)
            {
                // 只点到选择框才触发事件
                if (NeedClickOnBox && !MouseOnImage(PointToClient(MousePosition)))
                {
                    return;
                }

                OnChecks();
            }

            base.OnMouseClick(e);
        }

        /// <summary>设置选中状态后干什么。</summary>
        protected virtual void OnChecks()
        {
            CheckSound.Replay();
        }

        protected virtual void OnCheckedChanged() => CheckedChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>继承自Label的自定义标签类，用于选择框。</summary>
    public class MizukiCheckBox : MizukiCheckBoxBase
    {
        public MizukiCheckBox() : base()
        {
        }

        protected override void OnChecks()
        {
            Checked = !Checked;
            base.OnChecks();
        }
    }

    /// <summary>继承自Label的自定义标签类，用于选择框。</summary>
    public class MizukiComboBox : MizukiCheckBoxBase
    {
        /// <summary>
        /// 存储的窗口所有子控件的集合。
        /// </summary>
        private ControlCollection _controls;
        private MizukiComboBox[] _singles;

        public MizukiComboBox() : base()
        {
        }

        protected override void OnChecks()
        {
            if (Checked)
            {
                return;
            }

            if (Parent == null)
            {
                return;
            }

            ControlCollection cc = Parent.Controls;
            if (cc != _controls)
            {
                Debug.WriteLine("刷新单选框");
                var singles = cc.OfType<MizukiComboBox>().ToArray();
                foreach (var i in singles)
                {
                    i._controls = cc;
                    i._singles = singles;
                }
            }

            foreach (var i in _singles)
            {
                i.SetCheck(i == this);
            }

            base.OnChecks();
        }
    }

    /// <summary>安装结束后显示的选项专用的选择框。</summary>
    public class EndComboBox : MizukiComboBox
    {
        public string RunPath;
        public string Arguments;

        public EndComboBox() : base()
        {
        }

        public void Run()
        {
            if (!Checked)
            {
                return;
            }

            if (!CanBeRun())
            {
                // 如果不可运行就报错
                throw new InvalidOperationException(_StartWindow.Localization.NullRunning);
            }
            else if (string.IsNullOrWhiteSpace(RunPath))
            {
                return;
            }

            try
            {
                Process.Start(RunPath);
            }
            catch (Exception ex)
            {
                MizukiTool.错误弹窗(ex);
            }
        }

        public bool CanBeRun() => !string.IsNullOrEmpty(RunPath);
    }

    /// <summary>继承自MizukiLabel的类，用于安装选项相关的标签</summary>
    public class SetupCheckBox : MizukiCheckBox
    {
        private string _file;
        private string _truemd5;
        private string _md5;

        /// <summary>获取所设文件大小。</summary>
        public long FileSize { get; private set; }

        /// <summary>文件的MD5应该是多少。</summary>
        public string MD5
        {
            get => _md5;
            set => _md5 = value?.Trim();
        }

        /// <summary>实际MD5与预期MD5是否一致。</summary>
        public bool MD5Success => string.IsNullOrEmpty(_md5) || string.Equals(_md5, _truemd5 ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>右上角显示的名字。</summary>
        public string DisplayName { get; set; }

        /// <summary>获取或设置文件名，同时设置文件大小，请填写相对路径。</summary>
        public string Filename
        {
            get => _file;
            set
            {
                // 若没设置显示名就设置显示名
                if (string.IsNullOrEmpty(DisplayName))
                {
                    DisplayName = value;
                }

                // 刷新文件大小
                _file = string.IsNullOrWhiteSpace(_file) ?
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), value) :
                    null;
                RefreshFileSize();

                // 计算MD5
                _truemd5 = File.Exists(_file) ? MizukiTool.GetMD5ByHashAlgorithm(_file) : null;
            }
        }

        /// <summary>刷新文件大小。</summary>
        public void RefreshFileSize()
        {
            if (string.IsNullOrWhiteSpace(_file))
            {
                return;
            }

            FileSize = _StartWindow.GetUncompressedSize(_file);
        }

        // 安装选项默认使用八叉样式
        public SetupCheckBox()
        {
            CheckStyles = CheckBoxStyles.Cross;
        }

        public override void ResetCheckStyles()
        {
            CheckStyles = CheckBoxStyles.Cross;
        }

        public override bool ShouldSerializeCheckStyles()
        {
            return CheckStyles != CheckBoxStyles.Cross;
        }
    }
}
