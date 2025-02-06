using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace YuriInstaller.MizukiTools
{
    partial class Comment
    {
        /*
            这里存放其它控件。
         */
    }

    public class DoubleBufferedForm : Form
    {
        [DefaultValue(true)]
        protected override bool DoubleBuffered
        {
            get => base.DoubleBuffered;
            set => base.DoubleBuffered = value;
        }

        public DoubleBufferedForm() : base()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
    }

    public static class ColorBoard
    {
        /// <summary>普通的文字颜色。</summary>
        public static Color HutsuuTextColor { get; } = Color.Red;

        /// <summary>醒目的文字颜色，一般用于按钮、非提示文字。</summary>
        public static Color LightTextColor { get; } = Color.Yellow;

        /// <summary>程序的主题颜色。</summary>
        public static Color ThemeColor { get; } = Color.Red;

        /// <summary>边框的颜色，如编辑框、树形框等。</summary>
        public static Color CtrlBorderColor { get; } = Color.FromArgb(0xAD, 0x08, 0x08);

        /// <summary>被点亮后的边框颜色，用于突出显示。</summary>
        public static Color LightBorderColor { get; } = Color.Red;

        /// <summary>用于特殊用途的颜色。</summary>
        public static Color SpecialColor { get; } = Color.White;

        /// <summary>树形框用的笔刷（普通颜色）。</summary>
        public static Brush HutsuuTreeBrush { get; } = new SolidBrush(HutsuuTextColor);

        /// <summary>树形框用的笔刷（醒目颜色）。</summary>
        public static Brush LightTreeBrush { get; } = new SolidBrush(LightTextColor);
    }
    public class MizukiTreeView : TreeView
    {
        public MizukiTreeView() : base()
        {
            ForeColor = ColorBoard.HutsuuTextColor;
            TabStop = false;
        }

        protected override void OnEnter(EventArgs e)
        {
            ForeColor = ColorBoard.LightTextColor;
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            ForeColor = ColorBoard.HutsuuTextColor;
            base.OnEnter(e);
        }
    }

    public class MizukiWaveOutEvent : WaveOutEvent
    {
        /// <summary>
        /// 不受全局静音影响。
        /// </summary>
        [DefaultValue(false)]
        public bool NotGlobalQuiet { get; set; } = false;

        /// <summary>
        /// 全局静音。
        /// </summary>
        [DefaultValue(false)]
        public static bool GlobalQuiet { get; set; } = false;

        /// <summary>
        /// 对应的Reader。
        /// </summary>
        public StreamMediaFoundationReader Reader { get; set; }

        public MizukiWaveOutEvent() : base()
        {
        }

        public MizukiWaveOutEvent(Stream stream) : base()
        {
            Init(new StreamMediaFoundationReader(stream));
        }

        public new void Init(IWaveProvider waveProvider)
        {
            Reader = (StreamMediaFoundationReader)waveProvider;
            base.Init(waveProvider);
        }

        /// <summary>重新播放音乐。</summary>
        public void Replay()
        {
            if (Reader != null)
            {
                Reader.Position = 0;
            }

            Play();
        }

        public new void Play()
        {
            if (GlobalQuiet && !NotGlobalQuiet)
            {
                return;
            }

            base.Play();
        }
    }

    /// <summary>定义接口，获取IsMouseOn属性。<br />
    /// A interface to get IsMouseOn property.</summary>
    public interface IMouseOn
    {
        bool IsMouseOn { get; }
    }

    /// <summary>定义接口，获取IsMouseOn属性。<br />
    /// A interface to get IsMouseOn property.</summary>
    public interface IBtmBar
    {
        string BtmbarTip { get; }
    }

    public interface IBBM : IBtmBar, IMouseOn
    {
    }

    public static class StringExtension
    {
        public static string Reverse(this string str) => string.Concat(Enumerable.Reverse(str));

        /// <summary>判断是否含有非ASCII字符。</summary>
        public static bool ContainsNonEnglish(this string input)
        {
            foreach (char c in input)
            {
                if (c < 32 || c > 126)  // 非 ASCII 范围
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static class ControlExtension
    {
        /*
            public static void 显示(this Control control) => control.Show();
            public static void 隐藏(this Control control) => control.Hide();
            public static void 移到(this Control control, Point point) => control.Location = point;
        */

        public static void SetBasicProperties(this Control control, Point? location = null, string name = null, Size? size = null, bool? visible = null, Size? maximumSize = null, Size? minimumSize = null)
        {
            if (location != null) control.Location = location.Value;
            if (name != null) control.Name = name;
            if (size != null) control.Size = size.Value;
            if (visible != null) control.Visible = visible.Value;
            if (maximumSize != null) control.MaximumSize = maximumSize.Value;
            if (minimumSize != null) control.MinimumSize = minimumSize.Value;
        }

        public static void SetBasicProperties(this Control[] controls, Point? location = null, string name = null, Size? size = null, bool? visible = null, Size? maximumSize = null, Size? minimumSize = null)
        {
            foreach (var i in controls)
            {
                i.SetBasicProperties(location, name, size, visible, maximumSize, minimumSize);
            }
        }

        public static void Hide(this Control[] controls)
        {
            foreach (var i in controls)
            {
                i?.Hide();
            }
        }

        /// <summary> 统一 SuspendLayout / ResumeLayout </summary>
        public static void SuspendLayout(this Control[] panels)
        {
            foreach (var i in panels)
            {
                i?.SuspendLayout();
            }
        }

        /// <summary> 统一 SuspendLayout / ResumeLayout </summary>
        public static void ResumeLayout(this Control[] panels, bool performLayout)
        {
            foreach (var i in panels)
            {
                i?.ResumeLayout(performLayout);
            }
        }
    }

    /// <summary>一个用于命令行的类。 A Class use for command line.</summary>
    public static class ConsoleCommandManager
    {
        // 运行命令行。
        public static void RunConsoleCommand(string command, string argument, out int exitCode, out string stdOut, out string stdErr)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = command,
                    Arguments = argument,
                    RedirectStandardInput = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            _ = process.Start();
            process.WaitForExit();

            stdOut = process.StandardOutput.ReadToEnd();
            stdErr = process.StandardError.ReadToEnd();
            exitCode = process.ExitCode;
        }
    }
}
