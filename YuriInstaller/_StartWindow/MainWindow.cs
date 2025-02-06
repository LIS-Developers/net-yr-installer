using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

/*
    强烈不建议使用设计器编辑_StartWindow，因为其中一些设计器不支持的属性可能会在修改后丢失。
    如果你非要用后果自行承担。

    添加几个控件调个位置再用VSCode比较一下提取有用部分就可以了吧。
 */

namespace YuriInstaller
{
    /// <summary>程序主窗口类。此类不能被继承。</summary>
    internal sealed partial class _StartWindow : DoubleBufferedForm
    {
        /// <summary>安装过程展示的画廊。</summary>
        private static Bitmap[] Paints = new Bitmap[13]
        {
            Resources._1, Resources._2, Resources._3,
            Resources._4, Resources._5, Resources._6,
            Resources._7, Resources._8, Resources._9,
            Resources._10, Resources._11, Resources._12,
            Resources._13
        };

        public static readonly string runPath = Application.StartupPath;

        /// <summary>当前显示的页面。</summary>
        private int CurrentProgress { get; set; }

        private bool touchedButton = false;
        private int progressBase = 0;

        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

        public Image ResizedImage { get; private set; } = new Bitmap(36, 20);

        private string[] _args;

        /// <summary>启动窗口构造函数，初始化需要的一切东西。</summary>
        public _StartWindow()
        {
            InitializeComponent();
        }

        private void StartWindow_Load(object sender, EventArgs e)
        {
            PLayoutAction(() =>
            {
                Cursor = Program.ReleaseCursor;
                glslmd.Size = Size;
                bottomInstallTip.Font = LoadingText.Font = Program.L10N[Program.Lang].L10n.GeneralFont;
                LoadingText.Text = Program.L10N[Program.Lang].L10n.Laoding;
                bottomInstallTip.Text = Program.L10N[Program.Lang].L10n.Bottomtip;
            });
        }

        private async void StartWindow_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            // 循环遍历计算机所有逻辑驱动器名称(盘符)
            foreach (string drive in Environment.GetLogicalDrives())
            {
                // 实例化DriveInfo对象
                var dir = new DriveInfo(drive);
                if (dir.DriveType == DriveType.Fixed)// || dir.DriveType == DriveType.Removable)
                {
                    // Split仅获取盘符字母
                    TreeNode tNode = new TreeNode($"{dir.Name.Split(':')[0]}:{_TreeNodePart}")
                    {
                        Name = dir.Name,
                        Tag = dir.Name
                    };
                    contentTree.Nodes.Add(tNode); //加载驱动节点
                    tNode.Nodes.Add("");
                }
            }

            PLayoutAction(CustomControls);
            InitialControlsArray();
            CustomPreset();
            Debug.WriteLine("初始化完基本控件");

            L10n(Program.Lang);
            if (licensePanel.ScrollBase.Visible)
            {
                chkAgreeLicenses.CheckDisabled = true;
                chkAgreeLicenses.BtmbarTip = Localization.NeedSeeToEnd;
                chkAgreeLicenses.SetImage(ConvertToGray(CheckBoxStyles.Classic[0]));
                chkAgreeLicenses.ForeColor = Color.Gray; // 灰色字，达到禁止效果
                licensePanel.ScrolledToBottom += new EventHandler(LicensePanel_ScrolledToBottom);
            }

            Debug.WriteLine("显示窗口");
            MizukiButtonWithoutFocus.FocusTo = MizukiTextBoxBase.BtmBar = MizukiButton.BtmBar = bottomBar;

            FirstTimeUse();
            LaunchSoftware();

            await UpdateInterface();
        }

        private void StartWindow_FormClosing(object sender, FormClosingEventArgs e) => e.Cancel = CurrentProgress == _InstallingPage;

        private void StartWindow_FormClosed(object sender, FormClosedEventArgs e) => Exits();

        private void StartWindow_MouseEnter(object sender, EventArgs e)
        {
            if (touchedButton)
            {
                bottomBar.Text = Localization.BottomBarText[CurrentProgress, 0];
            }
        }

        public void SetArguments(string[] args)
        {
            _args = args;

            foreach (var arg in _args)
            {
                switch (arg)
                {
                    case "-win":
                        FormBorderStyle = FormBorderStyle.FixedSingle;
                        return;
                    default:
                        break;
                }
            }
        }

        /// <summary>退出程序之前关掉其他东西。</summary>
        public void Exits()
        {
            Debug.WriteLine("调用了Exits函数");
            Task.Delay(100).ContinueWith(_ => MizukiTool.ClearOutedFiles());
            Application.Exit();
        }

        /// <summary>初始化一些部件。</summary>
        private void LaunchSoftware()
        {
            Application.DoEvents();
            PLayoutAction(() =>
            {
                foreach (var i in GetAllControls(this))
                {
#if DEBUG
                    i.MouseClick += (object sender, MouseEventArgs e) =>
                    {
                        Debug.WriteLine($"{i.Name}: Size=({i.Width}, {i.Height}); Location=({i.Left}, {i.Top})");
                    };
#endif
                }

                foreach (var i in SetupLbls)
                {
                    if (!i.MD5Success)
                    {
                        MizukiTool.弹窗(Localization.NoSetupBin);
                        Exits();
                    }
                }
                InitializeBGM();
                glslmd.Dispose();
            });
        }

        private static List<Control> GetAllControls(Control container)
        {
            var controls = new List<Control>();
            // 先将当前容器控件的直接子控件添加到列表中
            foreach (Control control in container.Controls)
            {
                controls.Add(control);
                // 如果子控件本身也是容器控件（比如Panel等），则递归获取它里面的子控件并添加到列表
                if (control.HasChildren)
                {
                    controls.AddRange(GetAllControls(control));
                }
            }
            return controls;
        }

        private void FirstTimeUse()
        {
            Debug.WriteLine("初始化一些控件。");

            // 获取管理员盾牌图标并调整到合适大小
            using (Image adminImage = SystemIcons.Shield.ToBitmap())
            using (Graphics g = Graphics.FromImage(ResizedImage))
            {
                int newWidth = (int)(adminImage.Width * ((float)ResizedImage.Height / adminImage.Height));
                g.DrawImage(adminImage, ResizedImage.Width - newWidth, 0, newWidth, ResizedImage.Height);
            }
            GameUninstInfo.RegeditName = _RegeditName;
        }

        #region 其他事件

        private void RightTopPanel_SizeChanged(object sender, EventArgs e)
        {
            // 如果是安装中，多行文本，且超行了
            if (!rightTopLabel.Eventing &&
                CurrentProgress >= _InstallingPage &&
                rightTopLabel.Height > _RightTopPanelHeight - 1 &&
                Regex.Match(rightTopLabel.Text, @"\n.", RegexOptions.Singleline).Success)
            {
                rightTopLabel.Eventing = true;
                Debug.WriteLine("超行了");
                rightTopLabel.Text = rightTopLabel.Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)?.Last();
            }
            else
            {
                rightTopLabel.Eventing = false;
            }
        }

        private void RightTopPanel_TextChanged(object sender, EventArgs e) => Application.DoEvents();

        /// <summary>双击右上角面板切换语言，但安装中不能切换。<br />
        /// Double click the right-top label to change language in every page except installing page.</summary>
        private void RightTopLabel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (CurrentProgress != _InstallingPage)
            {
                Program.langSelection.ShowDialog(this);
            }
        }

        private void Credits_MouseClick(object sender, MouseEventArgs e)
        {
            lblCredits.Visible = false;
            progressBar1.Visible = CurrentProgress == _DonePage;
        }

        private void CreditBtn_MouseClick(object sender, MouseEventArgs e)
        {
            lblCredits.Visible = true;
            progressBar1.Visible = false;
        }

        #endregion
    }
}
