using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            安装程序界面侧边的红色按钮事件。
         */
    }

    partial class _StartWindow
    {
        /// <summary>碰到右边的按钮，让底部栏显示文字。</summary>
        private void Rbtn_MouseEnter(object sender, EventArgs e)
        {
            RightBarButton btn = (RightBarButton)sender;
            bottomBar.ScrollDisplayOnBtmBar(btn, Localization.BottomBarText[CurrentProgress, btn.BtnID]);
            touchedButton = true;
        }

        /// <summary>背景音乐开关。<br />
        /// Background music switch.</summary>
        private void MusicOnOff_MouseClick(object sender, EventArgs e)
        {
            if (rbtnMusicOnOff.Clicked)
            {
                backgroundMusic?.Play();
            }
            else
            {
                backgroundMusic?.Pause();
            }
        }

        /// <summary>背景音乐开关。<br />
        /// Background music switch.</summary>
        private void SoundOnOff_MouseClick(object sender, EventArgs e)
        {
            MizukiWaveOutEvent.GlobalQuiet = !rbtnSoundOnOff.Clicked;
        }

        /// <summary>退出键事件。<br />
        /// Event when click exit button.</summary>
        private void BottomButton1_MouseClick(object sender, EventArgs e)
        {
            bottomButton1.Enabled = false;
            bottomButton1.State = ButtonStates.Normal;
            Application.DoEvents();
            if (CurrentProgress == _DonePage)
            {
                foreach (var i in EndingLabels)
                {
                    if (i.Visible)
                    {
                        i.Run();
                    }
                }
                Application.Exit();
            }
            else
            {
                if (MizukiTool.弹窗Yesno(Localization.QuitConfirm))
                {
                    Application.Exit();
                }
            }
            Application.DoEvents();
            bottomButton1.Enabled = true;
        }


        /// <summary>前进键按钮事件。<br />
        /// Event when click next button.</summary>
        private async void BottomButton2_MouseClick(object sender, EventArgs e)
        {
            bottomButton1.Enabled = bottomButton2.Enabled = false;
            bottomButton3.Enabled = bottomButton4.Enabled = false;
            Application.DoEvents();

            // 如果到了安装前界面
            if (CurrentProgress == _InstallingPage - 1)
            {
                string path = pathBoxPanel.Text;

                // 路径格式非法 Invalid path
                if (!MizukiTool.IsGoodPath(path))
                {
                    pathInfo.Text = string.Format(Localization.InvaildPath, pathInfo.BackupText);
                    pathInfo.ForeColor = ColorBoard.LightTextColor;
                    pengci?.Replay();
                    Debug.WriteLine(pathInfo.Text);
                }
                // 空间不够 Free space not enough
                else if (spaceNeeded > freeSpace)
                {
                    MizukiTool.弹窗(Localization.DisknotEnough);
                }
                // 安装文件丢失 Installation files broken
                else if (spaceNeeded <= 0)
                {
                    MizukiTool.弹窗(Localization.NoSetupBin);
                }
                // 文件夹名含非法字符 Invalid charactor in folder name
                else if (!MizukiTool.IsGoodFoldername(StartMenuPanel.Text) && chkCreateStartMenuBox.Checked)
                {
                    pengci?.Replay();
                    MizukiTool.弹窗(Localization.InvaildStartMenu);
                }
                else
                {
                    bool notEmpty = Directory.Exists(path) && Directory.GetDirectories(path).Length + Directory.GetFiles(path).Length > 0;

                    // 该文件夹有东西 The folder is not empty
                    if (notEmpty)
                    {
                        if (!MizukiTool.弹窗Yesno(Localization.DisknotEmpty))
                        {
                            goto rebutton;
                        }
                    }

                    await InstallEvent(!notEmpty);
                    await UpdateInterface();
                    return;
                }

            // 恢复到正常 Back
            rebutton: Application.DoEvents();
                bottomButton4.Enabled = !bottomButton4.CantUse;
                bottomButton1.Enabled = bottomButton2.Enabled =
                bottomButton3.Enabled = true;
            }
            else
            {
                CurrentProgress++;
                if (CurrentProgress == _DummyPage)
                {
                    CurrentProgress++;
                }
                await UpdateInterface(100);
            }
        }

        /// <summary>返回键单击事件。<br />
        /// Event when click back button.</summary>
        private async void BottomButton3_MouseClick(object sender, EventArgs e)
        {
            bottomButton1.Enabled = false;
            bottomButton2.Enabled = false;
            bottomButton3.Enabled = false;
            bottomButton4.Enabled = false;

            CurrentProgress--;
            if (CurrentProgress == _DummyPage)
            {
                CurrentProgress--;
            }
            await UpdateInterface(100);
        }

        /// <summary>浏览按钮单击事件。<br />
        /// Event when click browse button.</summary>
        private async void BottomButton4_MouseClick(object sender, EventArgs e)
        {
            Debug.WriteLine("点击浏览按钮");
            await TenkaiTree();
            pathBoxPanel.ReadOnly = false;
            bottomButton4.MouseClick -= BottomButton4_MouseClick;
            pathBoxPanel.Child.MouseClick -= PathBox_MouseClick;
        }
    }
}
