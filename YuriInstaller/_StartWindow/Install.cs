using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            这是安装事件，安装的步骤（复制文件、写入注册表）都在这里。
         */
    }

    partial class _StartWindow
    {
        /// <summary>闪烁窗口任务栏图标。</summary>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "FlashWindow")]
        private static extern void FlashWindow(IntPtr hwnd, bool bInvert);

        private string registSoft = string.Empty;
        private CMDScript uninstScript = new CMDScript();

        private static Bytes freeSpace = 0;
        private static Bytes spaceNeeded = 0;
        private static Bytes currentSize = 0;

        private int randomint;

        // 每隔一段时间从画廊里选一张图片进去
        private void BgSwitchTimer_Tick(object sender, EventArgs e)
        {
            int newIndex;
            do
            {
                newIndex = MizukiTool.RandomObject.Next(Paints.Length);
            }
            while (randomint == newIndex);

            imageChange.Replay();
            randomint = newIndex;
            bgPaints.BackgroundImage = Paints[randomint];
            Application.DoEvents();
        }

        // 更新路径信息
        private void SetPathInfo()
        {
            freeSpace = -1;
            if (MizukiTool.IsGoodPath(pathBoxPanel.Text))
            {
                try
                {
                    var drive = new DriveInfo(Path.GetPathRoot(pathBoxPanel.Text));
                    if (drive.IsReady)
                    {
                        freeSpace = drive.AvailableFreeSpace;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                Debug.WriteLine("Has invalid character.");
            }

            pathInfo.Backup(string.Format(
                Localization.PathPage, spaceNeeded,
                (int)spaceNeeded.ToKBytes(), (int)spaceNeeded.ToMBytes(), spaceNeeded.ToGBytes(),
                freeSpace, (int)freeSpace.ToKBytes(), (int)freeSpace.ToMBytes(), freeSpace.ToGBytes(), bottomButton4.Clicked ? string.Empty : Localization.LittleTip));
        }

        /// <summary>安装事件。</summary>
        private async Task InstallEvent(bool toEmptyFolder = false)
        {
            CurrentProgress++;
            await UpdateInterface();
            rightTopLabel.Text = $"{Localization.Installing}...\n";

            if (await TryToInstall())
            {
                SetInstallEndingPath();
                useableEndLabels = EndingLabels.Where(i => i.CanBeRun()).ToArray();
                schkReturnToWindows.Checked = string.IsNullOrEmpty(_GameLauncher);
                CurrentProgress++;
                FlashWindow(Handle, true);
            }
            else
            {
                bottomButton3.Enabled = true;
                progressBar1.Visible = false;

                CurrentProgress--;
                if (toEmptyFolder)
                {
                    Directory.Delete(pathBoxPanel.Text, true);
                }
            }

            Cursor = Program.ReleaseCursor;
            bottomButton4.Enabled = !bottomButton4.Clicked;
            rbtnMusicOnOff.Enabled = rbtnSoundOnOff.Enabled = lblCreditBtn.Enabled = true;
        }

        private string CreateGameShorts(string tip, string path, string filename, string del = null)
        {
            rightTopLabel.Text += $"{tip}...\n";
            if (!CreateShortcut(path, filename,
                                Path.Combine(GameUninstInfo.InstallPath, _GameLauncher),
                                _LaunchArguments))
            {
                rightTopLabel.Text += $"{Localization.Failed}...\n";
                return string.Empty;
            }

            string returns = Path.Combine(path, filename);
            del = del ?? returns;
            if (!string.IsNullOrWhiteSpace(del))
            {
                uninstScript.AddDeleteFolderCommand(del);
            }

            return returns;
        }

        /// <summary>尝试安装，并返回是否安装成功。</summary>
        private async Task<bool> TryToInstall()
        {
            randomint = MizukiTool.RandomObject.Next(Paints.Length);
            bgPaints.BackgroundImage = Paints[randomint];
            bgSwitchTimer.Start();
            bgPaints.Visible = true;
            Application.DoEvents();

            try
            {
                bottomButton1.Enabled = bottomButton2.Enabled =
                bottomButton3.Enabled = bottomButton4.Enabled =
                rbtnMusicOnOff.Enabled = rbtnSoundOnOff.Enabled = lblCreditBtn.Enabled = false;

                uninstScript.Clear();
                uninstScript.AddCommand("chcp 65001");
                uninstScript.AddCommand($"goto then\n\n:then\n echo \"{Localization.TipBeforeRun}\"");
                uninstScript.AddCommand("pause");
                uninstScript.AddCommand("cd /");

                GameUninstInfo.InstallPath = pathBoxPanel.Text;
                Cursor = Program.WaitingCursor;

                foreach (var i in SetupLbls)
                {
                    try
                    {
                        await ExtractAFile(i, GameUninstInfo.InstallPath);
                    }
                    catch (Exception ex)
                    {
                        MizukiTool.错误弹窗(ex, $"{string.Format(Localization.UnpackError, i.Filename)}\n{ex.Message}");
                        return false;
                    }
                }

                if (chkCreateDesktopBox.Checked)
                {
                    GameUninstInfo.DesktopShortcut = CreateGameShorts(Localization.CreatingShortcut,
                    Program.DesktopPath, $"{Localization.GameName}.lnk");
                }

                if (chkCreateStartMenuBox.Checked)
                {
                    string startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), StartMenuPanel.Text);
                    GameUninstInfo.StartMenuPath = startMenuPath;
                    CreateGameShorts(Localization.CreStarShortcut, startMenuPath, $"{Localization.GameName}.lnk", startMenuPath);
                }

                // 写注册表
                if (!WriteToRegistry(GameUninstInfo.InstallPath))
                {
                    rightTopLabel.Text += $"{Localization.Failed}...\n";
                }

                // 创建卸载程序
                if (_UninstallEnabled)
                {
                    CreateUninstallProgram();
                }

                progressBar1.Done();
                return true;
            }
            catch (Exception ex)
            {
                MizukiTool.错误弹窗(ex);
                return false;
            }
            finally
            {
                bgSwitchTimer.Stop();
                bgPaints.Visible = false;
            }
        }

        /// <summary>写入卸载程序。</summary>
        private void CreateUninstallProgram()
        {
            Debug.WriteLine("写入卸载程序");
            rightTopLabel.Text += $"{Localization.CreatingUns}...\n";
            uninstScript.AddDeleteFolderCommand(GameUninstInfo.InstallPath);

            var unsExe = Path.Combine(GameUninstInfo.InstallPath, _UnsExe);
            var unsCmd = Path.Combine(GameUninstInfo.InstallPath, _UnsCmd);

            try
            {
                // 写入卸载程序
                File.WriteAllBytes(unsExe, Resources.uninst);
                File.WriteAllText(Path.Combine(GameUninstInfo.InstallPath, _UnsIni), GameUninstInfo.ToString(), Encoding.UTF8);
                File.WriteAllText(unsCmd, uninstScript.ToScript(), Encoding.UTF8);

                if (chkCreateStartMenuBox.Checked)
                {
                    CreateShortcut(GameUninstInfo.StartMenuPath, $"{Localization.Uninstall} {Localization.GameName}.lnk", unsExe);
                    CreateShortcut(GameUninstInfo.StartMenuPath, $"{Localization.Uninstall} {Localization.GameName} (cmd).lnk", unsCmd);
                }
            }
            catch (Exception ex)
            {
                MizukiTool.错误弹窗(ex);
                rightTopLabel.Text += $"{Localization.Failed}...\n";
            }
        }

        /// <summary>将游戏文件夹写入注册表。</summary>
        private bool WriteToRegistry(string GameDir)
        {
            rightTopLabel.Text += $"{Localization.Registering}...\n";

            try
            {
                string sn = new string(Enumerable.Range(0, 22).Select(_ => "0123456789"[new Random().Next(10)]).ToArray());

                // 中国一般会连同本体一起安装，若是仅MOD包可能需要修改一下
                using (var HKLM_32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    void SetGameRegistry(string path, string keyPath, string name, int sku, int version)
                    {
                        using (var key = HKLM_32.CreateSubKey(keyPath))
                        {
                            key.SetValue("Serial", sn);
                            key.SetValue("Name", name);
                            key.SetValue("InstallPath", path);
                            key.SetValue("SKU", sku);
                            key.SetValue("Version", version);
                        }
                        uninstScript.AddDeleteRegCommand($@"HKEY_LOCAL_MACHINE\{keyPath}");
                    }

                    SetGameRegistry(Path.Combine(GameDir, "RA2.EXE"), @"SOFTWARE\Westwood\Red Alert 2", "Red Alert 2", 8448, 65542);
                    SetGameRegistry(Path.Combine(GameDir, "RA2MD.EXE"), @"SOFTWARE\Westwood\Yuri's Revenge", "Yuri's Revenge", 10496, 65537);

                    if (_UninstallEnabled)
                    {
                        /* 
                         * 程序卸载信息
                         * DisplayName -- 显示名称
                         * DisplayIcon -- 图标
                         * EstimatedSize -- 大小
                         * DisplayVersion -- 版本
                         * InstallLocation -- 安装位置
                         * UninstallString -- 卸载程序
                         * Publisher -- 发布者
                         */

                        string comp = ((AssemblyCompanyAttribute)Assembly.GetCustomAttribute(typeof(AssemblyCompanyAttribute))).Company;
                        GameUninstInfo.Regedits = $@"SOFTWARE\{comp}\{_RegeditName}";
                        using (var unsKey = HKLM_32.CreateSubKey(
                            $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{_RegeditName}"))
                        using (var modKey = HKLM_32.CreateSubKey(GameUninstInfo.Regedits))
                        {
                            unsKey.SetValue("DisplayName", Localization.LongGameName);
                            unsKey.SetValue("DisplayIcon", Path.Combine(GameDir, _UnsExeIcon));
                            unsKey.SetValue("EstimatedSize", spaceNeeded.ToKBytes(), RegistryValueKind.DWord);
                            unsKey.SetValue("DisplayVersion", _GameVersion);
                            unsKey.SetValue("InstallLocation", GameDir);
                            unsKey.SetValue("UninstallString", Path.Combine(GameDir, _UnsExe));
                            unsKey.SetValue("Publisher", comp);

                            // MOD包注册表                
                            modKey.SetValue("Path", GameDir);
                            modKey.SetValue("Version", _GameVersion);
                            uninstScript.AddDeleteRegCommand($@"HKEY_LOCAL_MACHINE\{GameUninstInfo.Regedits}");
                        }
                        uninstScript.AddDeleteRegCommand($@"HKEY_LOCAL_MACHINE\{$@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{_RegeditName}"}");
                    }
                }

                // 写入兼容性注册表
                if (chkJianRongXingBox.Checked)
                {
                    var keyPath = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";
                    GameUninstInfo.GameExePath = Path.Combine(GameUninstInfo.InstallPath, _GameExeName);
                    using (var customKey = Registry.CurrentUser.OpenSubKey(keyPath, true))
                    {
                        customKey.SetValue(GameUninstInfo.GameExePath, _jrxStr);
                    }
                    uninstScript.AddDeleteRegKeyCommand($@"HKEY_CURRENT_USER\{keyPath}", GameUninstInfo.GameExePath);
                }

                // 写入blowfish.dll的注册表
                string blowfishPath = Path.Combine(GameDir, "Blowfish.dll");
                if (!File.Exists(blowfishPath))
                {
                    MizukiTool.弹窗(Localization.NoBlowfishDll);
                }
                ConsoleCommandManager.RunConsoleCommand("regsvr32.exe", $"/s \"{blowfishPath}\"", out int exitCode, out string stdOut, out string stdErr);

                // 判断执行是否正常
                foreach (var i in new string[] { stdOut, stdErr })
                {
                    if (!string.IsNullOrWhiteSpace(i))
                        MizukiTool.弹窗(i.Trim());
                }

                if (exitCode != 0)
                {
                    MizukiTool.弹窗(string.Format(Localization.FailedRetrun, exitCode));
                }
            }
            catch (Exception ex)
            {
                MizukiTool.错误弹窗(ex);
                return false;
            }

            return true;
        }
    }
}
