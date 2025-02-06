using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            这里是目录选择框的代码，正因为这些代码才能做出红警那样的树形框。
         */
    }

    partial class _StartWindow
    {
        /// <summary>用于缓存每一层级是否需要绘制连接线的状态。</summary>
        private bool[] shouldDrawConnector;

        /// <summary>展开树状目录选择框。<br />
        /// Show treeview path selecting box.</summary>
        private async Task TenkaiTree()
        {
            bottomButton4.Enabled = false;
            await HideButton(bottomButton4);
            PLayoutAction(() =>
            {
                pathInfo.Top = 40;
                SetPathInfo();
                // 计算控件的大小位置
                selFolderTitle.Top = pathInfo.Top + 120;
                contentPanel.Top = selFolderTitle.Bottom + 9;
                contentPanel.Height = 420 - contentPanel.Top;
                contentTree.Height = contentPanel.Height - 4;
                pathBoxPanel.Top = contentPanel.Bottom + 9;
                contentPanel.Visible = selFolderTitle.Visible = true;
                SetSettingChkLocation(installMissions, new Point(chkCreateDesktopBox.Left, pathBoxPanel.Bottom + 9), 25);
                Debug.WriteLine(contentPanel.Bottom);
            });
        }

        /// <summary>目录树节点绘制方法。</summary>
        private void ContentTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // 加减框
            Image img = Resources.lineicon;
            if (e.Node.Nodes.Count > 0)
            {
                // 加减框
                img = e.Node.IsExpanded ? Resources.minusicon : Resources.plusicon;
            }

            // 加减框绘制高度
            int height = e.Bounds.Top + (e.Bounds.Height - img.Height) / 2;

            // 字符串绘制偏移，初始为加减框右侧
            int offset = img.Width - 1;

            e.Graphics.FillRectangle(new SolidBrush(contentTree.BackColor), e.Bounds); // 填充背景            
            if (e.Node.Level == 0)                                                     // 判断是否为顶级节点
            {
                // 如果有项目
                if (e.Node.Nodes.Count <= 0)
                {
                    return;
                }

                e.Graphics.DrawImage(img, e.Bounds.Left, height);
            }
            else
            {
                // 计算缩进量，每个子节点层级缩进一定像素
                int indentation = (e.Node.Level - 1) * contentTree.Indent;

                // 如果该文件夹是最后一个文件夹，就用最后才用的连接线
                Image img2 = (e.Node.Parent.Nodes.IndexOf(e.Node) == e.Node.Parent.Nodes.Count - 1) ? Resources.lasticon : Resources.middleicon;

                // 连接线绘制高度
                int height2 = e.Bounds.Top + (e.Bounds.Height - img2.Height) / 2;

                // 更新 shouldDrawConnector 缓存（在树结构改变时更新）
                UpdateShouldDrawConnector(e.Node);

                // 绘制连接线
                for (int i = 1; i < e.Node.Level; i++)
                {
                    if (shouldDrawConnector[i])
                    {
                        e.Graphics.DrawImage(Resources.throughicon,
                            x: e.Bounds.Left + (i - 1) * contentTree.Indent,
                            y: height2);
                    }
                }

                // 加减框和分叉线
                e.Graphics.DrawImage(img2, e.Bounds.Left + indentation, height2);
                e.Graphics.DrawImage(img, e.Bounds.Left + img2.Width - 1 + indentation, height);

                // 将偏移设置为加减框和连接线和缩进量右侧
                offset += img2.Width - 1 + indentation;
            }

            // 绘制文字
            e.Graphics.DrawString(e.Node.Text, contentTree.Font,
                e.Node.IsSelected ? ColorBoard.LightTreeBrush : ColorBoard.HutsuuTreeBrush,
                e.Bounds.Left + offset, e.Bounds.Top);
        }

        /// <summary>在结点折叠展开后以后改变高度。</summary>
        private void ContentTree_AfterExpanse(object sender, TreeViewEventArgs e)
        {
            MizukiCheckBoxBase.CheckSound.Play();
            AdjustTreeViewHeight(contentTree, contentPanel.Height - 4);
        }

        /// <summary>在将要展开结点时发生，加载子结点。</summary>
        private void ContentTree_BeforeExpand(object sender, TreeViewCancelEventArgs e) => TreeViewItems.Add(e.Node);

        /// <summary>将路径加载到路径框里。<br />
        /// Load path into path textbox.</summary>
        private void ContentTree_AfterSelect(object sender, TreeViewEventArgs e) => pathBoxPanel.Text = Path.Combine(e.Node?.Name ?? string.Empty, _GamePath);

        /// <summary>更新 shouldDrawConnector 的缓存，避免每次绘制时重新计算。</summary>
        private void UpdateShouldDrawConnector(TreeNode node)
        {
            int level = node.Level;
            if (shouldDrawConnector == null || shouldDrawConnector.Length < level)
            {
                shouldDrawConnector = new bool[level];
            }

            for (int i = 0; i < level; i++)
            {
                shouldDrawConnector[i] = true;
            }

            TreeNode currentNode = node.Parent;
            while (currentNode != null && level > 0)
            {
                shouldDrawConnector[--level] = currentNode.NextNode != null;
                currentNode = currentNode.Parent;
            }
        }
    }

    /// 自定义类TreeViewItems 调用其Add(TreeNode e)方法加载子目录。</summary>
    public static class TreeViewItems
    {
        public static void Add(TreeNode e)
        {
            e.Nodes.Clear();                     // 清除空节点再加载子节点
            try
            {
                // 获取指定目录中的子目录名称并加载结点
                var dics = Directory.GetDirectories(e.Name);
                Debug.WriteLine($"{e.Name} 有 {dics.Length} 个子文件夹。");

                if (dics.Length == 0)
                {
                    return;
                }

                foreach (var dic in dics)
                {
                    var dicr = new DirectoryInfo(dic);
                    var subNode = new TreeNode($"{dicr.Name}{_StartWindow._TreeNodePart}"); // 实例化
                    subNode.Tag = subNode.Name = dicr.FullName; // 完整目录
                    try
                    {
                        // 有子目录再添加占位符
                        if (Directory.EnumerateDirectories(dicr.FullName).Any())
                        {
                            subNode.Nodes.Add("");  // 添加空节点占位 实现+号
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    // 文本过长时简化显示文本，将完整文本塞进ToolTip
                    if (subNode.Text.Length > 24)
                    {
                        subNode.ToolTipText = subNode.Text.Trim();
                        subNode.Text = CompressString(subNode.Text);
                    }

                    e.Nodes.Add(subNode);
                }
            }
            catch (Exception ex)
            {
                MizukiTool.错误弹窗(ex);
            }
        }

        public static string CompressString(string text) => $"{text.Substring(0, 10)}...{text.Substring(text.Length - 10)}";
    }
}
