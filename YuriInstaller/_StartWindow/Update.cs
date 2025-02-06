using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            这里是用于界面更新的，切换到哪个页面就改成什么样。
            因为这里的代码写得比较早，所以比较乱。
         */
    }

    partial class _StartWindow
    {
        /// <summary>切换页面可能会改变的控件。</summary>
        private Control[] usedControls;

        /// <summary>使用全局字体（12F宋体）的控件。</summary>
        private Control[] generalFontControls;

        private EndComboBox[] useableEndLabels;

        /// <summary>初始化控件数组。</summary>
        private void InitialControlsArray()
        {
            usedControls = new Control[]
            {
                pathInfo,
                selFolderTitle,
                contentPanel,
                licensePanel,
                pathBoxPanel,
                InstallSetting,
                InstallSyoukai1,
                InstallSyoukai2,
                EndTitle,
                EndLabel1,
                EndLabel2,
                StartMenuPanel,
                chkAgreeLicenses,
                chkJianRongXingBox,
                chkCreateDesktopBox,
                chkCreateStartMenuBox
            }.Concat(MizukiLabels).ToArray();

            generalFontControls = new Control[]
            {
                InstallSetting,
                EndTitle,
                EndLabel1,
                EndLabel2,
                bottomBar,
                bottomButton1,
                bottomButton2,
                bottomButton3,
                bottomButton4,
                rbtnMusicOnOff,
                rbtnSoundOnOff,
                lblCredits,
                pathInfo,
                selFolderTitle
            }.Concat(MizukiLabels).ToArray();
        }

        /// <summary>更新界面，并设置延迟。</summary>
        /// <param name="sleep">延迟</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task UpdateInterface(int sleep = 0)
        {
            Debug.WriteLine("界面更新");
            await PLayoutAction(async () =>
            {
                BackgroundImage = CurrentProgress == _StartPage ? Resources.Binary_bg : Resources.Binary_bg2;
                usedControls.Hide();
                await ChangeButtons();
                UpdateTextsAndControls();
                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
                }
                Application.DoEvents();
                SetButtonsEnabled();
            });
        }

        /// <summary>封装 SuspendLayout / ResumeLayout 以优化代码（异步版本）</summary>
        private async Task PLayoutAction(Func<Task> action)
        {
            SuspendLayout();
            await action();
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>封装 SuspendLayout / ResumeLayout 以优化代码</summary>
        private void PLayoutAction(Action action)
        {
            SuspendLayout();
            action();
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>封装 SuspendLayout / ResumeLayout 以优化代码</summary>
        private void PLayoutActionWithTryCatch(Action @try, Action @catch)
        {
            SuspendLayout();
            try
            {
                @try();
            }
            catch
            {
                @catch();
            }
            finally
            {
                ResumeLayout(false);
                PerformLayout();
            }
        }

        /// <summary>设置按钮启用禁用。</summary>
        private void SetButtonsEnabled()
        {
            bottomButton1.Enabled = CurrentProgress != _InstallingPage;
            bottomButton2.Enabled = (CurrentProgress == _LicensePage && chkAgreeLicenses.Checked) ||
                                     !(CurrentProgress == _LicensePage || CurrentProgress == _DummyPage ||
                                       CurrentProgress == _DonePage);
            bottomButton3.Enabled = !(CurrentProgress == _StartPage || CurrentProgress == _DonePage);
            bottomButton4.Enabled = CurrentProgress == _PathPage && !bottomButton4.Clicked;
        }

        /// <summary>更新按钮的位置。</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task ChangeButtons()
        {
            if (CurrentProgress == _DonePage) //因为到了完成安装以后就不可逆了，所以就不写双向表达式了
            {
                await HideButtons(new RightBarButton[3] { bottomButton4, bottomButton3, bottomButton2 });
                return;
            }

            if (CurrentProgress == _StartPage || CurrentProgress == _DonePage)
            {
                await HideButton(bottomButton3);
            }
            else
            {
                await ShowButton(bottomButton3);
            }

            if (bottomButton4.Clicked)
            {
                return;
            }

            if (CurrentProgress == _PathPage || CurrentProgress == _InstallingPage)
            {
                Debug.WriteLine("浏览按钮展示出来");
                await ShowButton(bottomButton4);
            }
            else
            {
                Debug.WriteLine("浏览按钮收回去");
                await HideButton(bottomButton4);
            }
        }

        /// <summary>更新控件的文字、可见性、位置。</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateTextsAndControls()
        {
            Debug.WriteLine("更新控件文字");
            switch (CurrentProgress)
            {
                case _LicensePage:
                    licensePanel.Visible = chkAgreeLicenses.Visible = true;
                    break;

                case _SelectionPage:
                    SetSelectionPage();
                    break;

                case _PathPage:
                    SetPathPage();
                    break;

                case _DonePage:
                    SetDonePage();
                    break;
            }

            bottomButton2.Image = CurrentProgress == _InstallingPage - 1 ? ResizedImage : null;

            bottomBar.Text = string.Empty;
            bottomButton1.Text = Localization.BottomBtn1[CurrentProgress];
            bottomButton2.Text = Localization.BottomBtn2[CurrentProgress];
        }

        private void SetSelectionPage()
        {
            InstallSetting.Text = Localization.SelectParts;
            InstallSetting.Visible = InstallSyoukai1.Visible = InstallSyoukai2.Visible = true;
            foreach (var i in SetupLbls)
            {
                i.Visible = true;
            }
        }

        private void SetPathPage()
        {
            // 重置占用空间
            spaceNeeded = SetupLbls.Sum(i => i.FileSize * MizukiTool.BoolToInt(i.Checked));

            pathInfo.Top = bottomButton4.Clicked ? 40 : 120;
            selFolderTitle.Top = pathInfo.Top + 120;
            selFolderTitle.Text = Localization.AbleFolder;

            SetPathInfo();

            selFolderTitle.Visible = contentPanel.Visible = bottomButton4.Clicked;
            pathInfo.Visible = pathBoxPanel.Visible = chkCreateDesktopBox.Visible =
            chkCreateStartMenuBox.Visible = chkJianRongXingBox.Visible = true;
            StartMenuPanel.Visible = chkCreateStartMenuBox.Checked;
        }

        private void SetDonePage()
        {
            EndTitle.Visible = EndLabel1.Visible = EndLabel2.Visible = true;
            // 将安装完的选项添加进去，忽略不可用的。
            SetSettingChkLocation(useableEndLabels, EndLabel1.Left + 10, EndLabel1.Bottom + 10, 25, 0, true);
        }
    }
}
