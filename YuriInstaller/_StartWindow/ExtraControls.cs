using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            安装选项添加方法：
            （1）先在类里声明SetupLabel控件；
            （2）将你的SetupLabel添加到setuplbls数组里；
            （3）找到本文件“// 安装选项开始”部分，参照例子设置；
            （4）在L10N.cs注册你的安装选项名称变量；
            （5）在L10N方法里规定变量内容；
            （6）在L10N.cs里找到“// 安装选项开始”部分，参照例子设置；
            （7）在setup.spec里添加你的安装文件，记得每条第二个空一定要是Temp；
            （8）设置这个SetupLabel的实例的DisplayName为你的个性化名称（如果你想）；
            （9）例如你的SetupLabel绑定setup.7z，右上角会显示正在解压setup.7z，如果设置DisplayName为aaa，则显示正在解压aaa。
         */

        /*
            安装结束行为选项添加方法：
            （1）先在类里声明EndLabel控件；
            （2）在SetInstallEnding方法里将RunPath设为要执行的程序；
            （3）在L10N.cs注册你的安装选项名称变量；
            （4）在L10N方法里规定变量内容；
            （5）将你的选项添加到EndingLabels，再在SetEndingSelectionText方法里添加前面定义的字符串变量。
         */
    }

    // 自定义标签、安装选项等
    partial class _StartWindow
    {
        // 定制编辑框面板 Custom TextPanels
        private static ScrollPBPanel contentPanel, licensePanel;
        private static MizukiTextBoxPanel pathBoxPanel, StartMenuPanel;
        private static MizukiTextBoxBase license;

        // 各种自定义标签实例 Custom Checkboxes
        private static readonly SetupCheckBox chkInstallSet = new SetupCheckBox();
        private static readonly SetupCheckBox chkInstallSetA = new SetupCheckBox();

        private static readonly MizukiCheckBox chkCreateDesktopBox = new MizukiCheckBox();
        private static readonly MizukiCheckBox chkCreateStartMenuBox = new MizukiCheckBox();
        private static readonly MizukiCheckBox chkJianRongXingBox = new MizukiCheckBox();
        private static readonly MizukiCheckBox chkAgreeLicenses = new MizukiCheckBox();

        private static readonly EndComboBox schkReturnToWindows = new EndComboBox();
        private static readonly EndComboBox schkRunGame = new EndComboBox();
        private static readonly EndComboBox schkReadmeFile = new EndComboBox();
        private static readonly EndComboBox schkSeeWWWeb = new EndComboBox();
        private static readonly EndComboBox schkSeeMyWeb = new EndComboBox();
        private static readonly EndComboBox schkOpenExplorer = new EndComboBox();
        private static readonly EndComboBox schkRegUI = new EndComboBox();

        /// <summary>初始化安装选项组</summary>
        private static SetupCheckBox[] SetupLbls { get; } = new SetupCheckBox[2]
        {
            chkInstallSet,
            chkInstallSetA
        };

        /// <summary>初始化普通自定义标签数组。</summary>
        private static MizukiCheckBoxBase[] MizukiLabels { get; set; } = new MizukiCheckBox[4]
        {
            chkAgreeLicenses,
            chkJianRongXingBox,
            chkCreateStartMenuBox,
            chkCreateDesktopBox
        };

        /// <summary>初始化安装结束单选框组</summary>
        private static EndComboBox[] EndingLabels { get; } = new EndComboBox[7]
        {
            schkRunGame,
            schkReadmeFile,
            schkReturnToWindows,
            schkSeeWWWeb,
            schkSeeMyWeb,
            schkOpenExplorer,
            schkRegUI
        };

        /// <summary>安装附加任务选项。</summary>
        private static MizukiCheckBox[] installMissions = new MizukiCheckBox[3]
        {
            chkCreateDesktopBox,
            chkCreateStartMenuBox,
            chkJianRongXingBox
        };

        /// <summary>设置安装结束选项运行路径。</summary>
        private void SetInstallEndingPath()
        {
            string installPath = GameUninstInfo.InstallPath;
            schkReturnToWindows.RunPath = " ";
            schkRunGame.RunPath = Path.Combine(installPath, _GameLauncher);
            schkRunGame.Arguments = _LaunchArguments;
            schkReadmeFile.RunPath = Path.Combine(installPath, _ReadmeFile);
            schkSeeWWWeb.RunPath = _GameHomepage;
            schkSeeMyWeb.RunPath = _AuthorHomepage;
            schkOpenExplorer.RunPath = installPath;
            schkRegUI.RunPath = Path.Combine(installPath, _RegSetMD);
        }

        /// <summary>初始化各种自定义控件及其属性。</summary>
        private void CustomControls()
        {
            MizukiLabels = MizukiLabels.Concat(SetupLbls).Concat(EndingLabels).ToArray();

            // 设置各标签的初始选中状态
            chkCreateDesktopBox.Checked = chkCreateStartMenuBox.Checked =
            chkInstallSet.Checked = schkRunGame.Checked = true;

            // 设置各标签的具体位置及事件处理（部分）
            chkAgreeLicenses.Location = new Point(67, 458);
            chkAgreeLicenses.Name = "chkAgreeLicenses";
            chkAgreeLicenses.CheckedChanged += (object sender, EventArgs e) => bottomButton2.Enabled = chkAgreeLicenses.Checked;

            chkCreateDesktopBox.Name = "chkCreateDesktopBox";
            chkCreateStartMenuBox.Name = "chkCreateStartMenuBox";
            chkJianRongXingBox.Name = "chkJianRongXingBox";

            schkRunGame.Name = "schkRunGame";
            schkReturnToWindows.Name = "schkReturnToWindows";
            schkReadmeFile.Name = "schkReadmeFile";
            schkSeeWWWeb.Name = "schkSeeWWWeb";
            schkSeeMyWeb.Name = "schkSeeMyWeb";
            schkOpenExplorer.Name = "schkOpenExplorer";
            schkRegUI.Name = "schkRegUI";

            SetSetupCheckbox();

            // 将所有自定义标签添加到控件集合中
            foreach (var chk in MizukiLabels)
            {
                Controls.Add(chk);
            }

            // 设置安装选项的位置
            SetSettingChkLocation(SetupLbls, new Point(67, 80), 25);
            SetSettingChkLocation(installMissions, new Point(67, 370), 25);
            CustomPanelsAndTextBoxes();

            chkCreateStartMenuBox.MouseClick += (object sender, MouseEventArgs e) => StartMenuPanel.Visible = chkCreateStartMenuBox.Checked;
            chkCreateStartMenuBox.LocationChanged += ChkCreateStartMenuBox_Changed;
            chkCreateStartMenuBox.SizeChanged += ChkCreateStartMenuBox_Changed;
        }

        private void SetSetupCheckbox()
        {
            chkInstallSet.Name = "chkInstallSet";
            chkInstallSet.CheckDisabled = true;     // 必选项不能关闭
            chkInstallSet.Filename = "setup.7z";
            chkInstallSet.MD5 = "25f242b57a007a3bd21e8b70256787d7";

            chkInstallSetA.Name = "chkInstallSetA";
            chkInstallSetA.Filename = "setup1.7z";
            chkInstallSetA.MD5 = "b91989c54eca97cfb375009562860e4a";
        }

        /// <summary>初始化定制文本框面板。<br />
        /// Initialize custom paneles and textboxes.</summary>
        private void CustomPanelsAndTextBoxes()
        {
            contentPanel = new ScrollPBPanel();
            licensePanel = new ScrollPBPanel();
            pathBoxPanel = new MizukiTextBoxPanel();
            StartMenuPanel = new MizukiTextBoxPanel();

            contentPanel.SetChild(contentTree);
            licensePanel.SetChild(licenseSider);

            licenseSider.SizeChanged += (object sender, EventArgs e) => license.Size = licenseSider.Size;
            licenseSider.TextChanged += (object sender, EventArgs e) => license.Text = licenseSider.Text;

            license = new MizukiTextBoxBase
            {
                Location = MizukiTool.ZeroPoint,
                Multiline = true,
                Name = "license",
                ReadOnly = true,
                Size = new Size(100, 21)
            };

            Panel[] CustomPanels = new Panel[4]
            {
                contentPanel,
                StartMenuPanel,
                pathBoxPanel,
                licensePanel
            };

            CustomPanels.SuspendLayout();
            contentPanel.SetBasicProperties(new Point(64, 164), "contentPanel", new Size(502, 267), false);
            licensePanel.SetBasicProperties(new Point(66, 55), "licensePanel", new Size(495, 400));
            pathBoxPanel.SetBasicProperties(new Point(64, 330), "pathBoxPanel", new Size(502, 28), false);
            StartMenuPanel.SetBasicProperties(new Point(8, 576), "StartMenuPanel", new Size(100, 20), false);

            pathBoxPanel.Child.Name = "pathBox";
            pathBoxPanel.Child.Size = new Size(490, 17);
            pathBoxPanel.ReadOnly = true;
            pathBoxPanel.Child.MouseClick += new MouseEventHandler(PathBox_MouseClick);
            pathBoxPanel.Child.TextChanged += (object sender, EventArgs e) =>
            {
                SetPathInfo();
                pathInfo.ForeColor = ColorBoard.HutsuuTextColor;
            };

            StartMenuPanel.Child.Name = "StartMenuNameBox";
            StartMenuPanel.ImeMode = ImeMode.On;
            Controls.AddRange(CustomPanels);
            CustomPanels.ResumeLayout(false);
        }

        /// <summary>开始菜单文件夹编辑框相关。</summary>
        private void ChkCreateStartMenuBox_Changed(object sender, EventArgs e)
        {
            int targetH = StartMenuPanel.Child.Height + 4;
            StartMenuPanel.Location = new Point(chkCreateStartMenuBox.Right + 10, chkCreateStartMenuBox.Top + (chkCreateStartMenuBox.Height - targetH) / 2);
            StartMenuPanel.Size = new Size(pathBoxPanel.Right - StartMenuPanel.Left - (chkCreateStartMenuBox.Left - pathBoxPanel.Left), targetH);
        }

        private void PathBox_MouseClick(object sender, MouseEventArgs e)
        {
            bottomButton4.Clicked = true;
            BottomButton4_MouseClick(bottomButton4, e);
        }

        /// <summary>只有将用户协议拉到底才能同意协议。</summary>
        private void LicensePanel_ScrolledToBottom(object sender = null, EventArgs e = null)
        {
            licensePanel.ScrolledToBottom -= LicensePanel_ScrolledToBottom;
            chkAgreeLicenses.CheckDisabled = false;
            chkAgreeLicenses.Checked = chkAgreeLicenses.Checked;
            chkAgreeLicenses.BtmbarTip = null;
            chkAgreeLicenses.ForeColor = ColorBoard.HutsuuTextColor;
        }
    }
}
