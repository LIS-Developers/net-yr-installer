using IWshRuntimeLibrary;
using SharpSevenZip;
using SharpSevenZip.EventArguments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            这里是程序使用的函数，为什么会划分到这里我也不清楚，可能就是因为他善吧。
            There are functions that program need them to run normally.
         */
    }

    partial class _StartWindow
    {
        /// <summary>无法在设计器内使用的属性。<br />
        /// Properties that can't use in designer.</summary>
        private void CustomPreset()
        {
            lblCredits.MinimumSize = Size;
            licensePanel.AddSecondControl(license);
            pathBoxPanel.Text = Path.Combine(_DefaultPath, _GamePath);
            progressBar1.BarColor = ColorBoard.ThemeColor;
            progressBar1.ForeColor = ColorBoard.LightTextColor;
            rightTopLabel.MaximumSize = new Size(144, _RightTopPanelHeight);
            bgPaints.Size = BackgroundPanelSize;
            Control[] labels = new Control[]
            {
                pathInfo, selFolderTitle,
                InstallSetting, InstallSyoukai1, InstallSyoukai2,
                EndTitle, EndLabel1, EndLabel2
            };

            Size max = new Size(500, int.MaxValue);
            Size min = new Size(500, 0);

            foreach (var i in labels)
            {
                i.MaximumSize = max;
                i.MinimumSize = min;
            }
        }

        private void MoveButton(RightBarButton btmBtn, int step)
        {
            if (InvokeRequired)
            {
                Debug.WriteLine("需要Invoke。");
                Invoke((Action<RightBarButton, int>)MoveButton, btmBtn, step);
            }
            else
            {
                btmBtn.Left += step * 2;
            }
        }

        /// <summary>向右向左移。</summary>
        /// <param name="btmBtn">按钮</param>
        /// <param name="step">距离</param>
        private void MoveButtonLR(Action action, Action action1, RightBarButton btmBtn)
        {
            action();
            for (int i = 2; i <= 16; i++)
            {
                MoveButton(btmBtn, i);
                Thread.Sleep(3);
            }
            action1();
            for (int i = 2; i <= 16; i++)
            {
                MoveButton(btmBtn, -i);
                Thread.Sleep(4);
            }
        }

        private Task RunTaskPeriod(Action action) => Task.Run(() =>
        {
            MenuUpdate.Replay();
            MizukiTool.MM_BeginPeriod(1);
            action();
            MizukiTool.MM_EndPeriod(1);
        });

        /// <summary>移动按钮，并设置按钮标题（显示按钮）。<br />
        /// Move bottons and show them.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task ShowButton(RightBarButton btmBtn)
        {
            if (btmBtn?.CantUse != true)
            {
                return;
            }

            await RunTaskPeriod(() =>
            {
                MoveButtonLR(() => btmBtn.State = ButtonStates.MoveB,
                             () => btmBtn.State = ButtonStates.MoveA, btmBtn);
                btmBtn.State = ButtonStates.Normal;
            });
        }

        /// <summary>单纯移动按钮（隐藏按钮）。<br />
        /// Move buttons and hide them.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task HideButton(RightBarButton btmBtn)
        {
            if (btmBtn?.CantUse != false)
            {
                return;
            }

            await RunTaskPeriod(() => MoveButtonLR(() => btmBtn.State = ButtonStates.MoveA,
                                () => btmBtn.State = ButtonStates.Hide, btmBtn));
        }

        /// <summary>隐藏多个按钮。<br />
        /// Hide many buttons.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task HideButtons(RightBarButton[] sourceButton)
        {
            if (sourceButton?.Any() != true)
            {
                return;
            }

            List<RightBarButton> bottomButton = new List<RightBarButton>();
            foreach (var bbtn in sourceButton)
            {
                // 跳过已隐藏的按钮
                if (bbtn?.CantUse != false)
                {
                    continue;
                }

                bbtn.State = ButtonStates.MoveA;
                bottomButton.Add(bbtn);
            }

            await Task.Run(() =>
            {
                MenuUpdate.Replay();
                MizukiTool.MM_BeginPeriod(1);
                // 向右移
                for (int i = 2; i <= 16; i++)
                {
                    foreach (var bbtn in bottomButton)
                    {
                        MoveButton(bbtn, i);
                    }
                    Thread.Sleep(3);
                }

                foreach (var bbtn in bottomButton)
                {
                    bbtn.State = ButtonStates.Hide;
                }

                // 向左移
                for (int i = 2; i <= 16; i++)
                {
                    foreach (var bbtn in bottomButton)
                    {
                        MoveButton(bbtn, -i);
                    }
                    Thread.Sleep(4);
                }
                MizukiTool.MM_EndPeriod(1);
            });
        }

        /// <summary>链式设置选择框位置。</summary>
        /// <param name="mls">要设置的选择框</param>
        /// <param name="firstPoint">起始位置</param>
        /// <param name="vertical">垂直间距</param>
        /// <param name="visible">是否顺便显示</param>
        private void SetSettingChkLocation(MizukiCheckBoxBase[] mls, Point firstPoint, int vertical, int horizontal = 0, bool? visible = null) => SetSettingChkLocation(mls, firstPoint.X, firstPoint.Y, vertical, horizontal, visible);

        /// <summary>链式设置选择框位置。</summary>
        /// <param name="mls">要设置的选择框</param>
        /// <param name="firstPoint">起始位置</param>
        /// <param name="vertical">垂直间距</param>
        /// <param name="visible">是否顺便显示</param>
        private void SetSettingChkLocation(MizukiCheckBoxBase[] mls, int firstX, int firstY, int vertical, int horizontal = 0, bool? visible = null)
        {
            if (!mls.Any())
            {
                return;
            }

            for (int i = 0; i < mls.Length; i++)
            {
                mls[i].Location = new Point(firstX + i * horizontal,
                                            firstY + i * vertical);
                if (visible != null)
                {
                    mls[i].Visible = (bool)visible;
                }
            }
        }

        /// <summary>创建快捷方式。</summary>
        /// <param name="createPath">创建在哪里</param>
        /// <param name="shortcutName">叫什么名字</param>
        /// <param name="targetPath">指向什么文件</param>
        /// <param name="args">运行参数</param>
        /// <returns>创建成功与否</returns>
        private static bool CreateShortcut(string createPath, string shortcutName, string targetPath, string args = null)
        {
            if (!Directory.Exists(createPath))
            {
                Directory.CreateDirectory(createPath);
            }

            try
            {
                string shortcutPath = Path.Combine(createPath, shortcutName);
                if (!string.IsNullOrEmpty(args))
                {
                    shortcutPath += $" {args}";
                }
                var shortcut = (IWshShortcut)new WshShell().CreateShortcut(shortcutPath);
                shortcut.TargetPath = targetPath;
                Debug.WriteLine($"目标地址：{targetPath}");
                Debug.WriteLine($"保存位置：{shortcutPath}");
                shortcut.Save();
                return true;
            }
            catch (Exception ex)
            {
                MizukiTool.错误弹窗(ex);
                return false;
            }
        }

        /// <summary>将图像转换为灰度图像。</summary>
        private Image ConvertToGray(Image originalImage)
        {
            var width = originalImage.Width;
            var height = originalImage.Height;
            var grayBitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixelColor = ((Bitmap)originalImage).GetPixel(x, y);
                    // 计算灰度值
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    var grayColor = Color.FromArgb(pixelColor.A, grayValue, grayValue, grayValue);
                    grayBitmap.SetPixel(x, y, grayColor);
                }
            }

            return grayBitmap;
        }

        /// <summary>解压安装文件。</summary>
        private async Task ExtractAFile(SetupCheckBox i, string targetPath)
        {
            if (i.Checked)
            {
                // 更改右上角文本
                rightTopLabel.Text += $"{string.Format(Localization.UnPacking, i.DisplayName)}...\n";
                progressBar1.Visible = true;
                currentSize = i.FileSize;

                var progress = new Progress<int>(value =>
                {
                    // 在UI线程上更新进度条
                    progressBar1.Progress = value;
                });

                await Task.Run(() =>
                {
                    using (var extractor = new SharpSevenZipExtractor(i.Filename))
                    {
                        // 更新解压进度
                        extractor.Extracting += (object sender, ProgressEventArgs e) =>
                        {
                            ((IProgress<int>)progress).Report(progressBase + (int)(currentSize * e.PercentDone / spaceNeeded));
                        };
                        extractor.ExtractArchive(targetPath);
                        progressBase = (int)(currentSize * 100 / spaceNeeded);
                    }
                });
            }
        }

        /// <summary>获取嵌入的资源大小。</summary>
        public static long GetEmbeddedResourceSize(Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Seek(0, SeekOrigin.End);
            long size = stream.Position;
            stream.Seek(originalPosition, SeekOrigin.Begin);
            return size;
        }

        /// <summary>获取文件原始大小。</summary>
        public static long GetUncompressedSize(string filename)
        {
            try
            {
                using (SharpSevenZipExtractor extractor = new SharpSevenZipExtractor(filename))
                {
                    long totalSizeAfterExtraction = 0;
                    for (int i = 0; i < extractor.ArchiveFileData.Count; i++)
                    {
                        totalSizeAfterExtraction += (long)extractor.ArchiveFileData[i].Size;
                    }
                    Debug.WriteLine($"解压后的总大小: {totalSizeAfterExtraction} 字节");
                    return totalSizeAfterExtraction;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return -1;
            }
        }

        /// <summary>计算并调整目录树的高度。</summary>
        private void AdjustTreeViewHeight(TreeView tree, int max = 0)
        {
            int visibleNodesCount = 0;
            CalculateVisibleNodes(tree.Nodes, ref visibleNodesCount);
            int i = visibleNodesCount * tree.ItemHeight;
            tree.Height = Math.Max(i, max);
        }

        /// <summary>计算TreeView有多少个节点。</summary>
        private void CalculateVisibleNodes(TreeNodeCollection nodes, ref int count)
        {
            foreach (TreeNode node in nodes)
            {
                count++;
                if (node.IsExpanded)
                {
                    CalculateVisibleNodes(node.Nodes, ref count);
                }
            }
        }
    }
}
