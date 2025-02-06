using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YuriInstaller.ExtraWindows;

namespace YuriInstaller.MizukiTools
{
    /// <summary>自己写的小函数。</summary>
    public static class MizukiTool
    {
        /// <summary>听说套上这个函数就能让Thread.Sleep变精准。</summary>
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint MM_BeginPeriod(uint uMilliseconds);

        /// <summary>听说套上这个函数就能让Thread.Sleep变精准。</summary>
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint MM_EndPeriod(uint uMilliseconds);

        /// <summary>所有已经释出的文件。</summary>
        public static string[] OutedFilenames { get; set; } = new string[] { };

        /// <summary>零点 (0,0)。</summary>
        public static Point ZeroPoint { get; } = new Point(0, 0);

        /// <summary>随机数对象。</summary>
        public static Random RandomObject { get; } = new Random();

        /// <summary>使用数组的方式传入多行，并转换为六进制数。设置分隔符和是否转换为汉字。</summary>
        public static string TenkanSixShur(string[] lines, TenkanOption option = TenkanOption.None, string separator = "\n") =>
            string.Join(separator, lines.Select(line => TenkanSixShur(line, option)));

        /// <summary>将文字转换为六进制数，设置false就替换成汉字，设置true将输出数字但用别的数字干扰空格。</summary>
        public static string TenkanSixShur(string srcString, TenkanOption option = TenkanOption.None, bool singleLine = false)
        {
            // 空字符串直接返回
            if (string.IsNullOrEmpty(srcString))
            {
                return srcString;
            }

            // 将每个Unicode码字节转换为十六进制字符串，再转换为十进制数，最后转换为六进制数
            // 单行多行分开处理
            string gitgwo = singleLine
                ? TenkanStringToSix(srcString)
                : string.Join("\n", srcString.Split('\n').Select(TenkanStringToSix));

            switch (option)
            {
                case TenkanOption.Gisou:
                    StringBuilder sb = new StringBuilder();
                    string[] strs = gitgwo.Split(' ');

                    // 不转换成汉字，随机插入非六进制字符，伪装成十进制
                    for (int i = 0; i < strs.Length; i++)
                    {
                        sb.Append(strs[i]);
                        if (i == strs.Length - 1)
                        {
                            break;
                        }

                        for (int g = 0; g <= RandomObject.Next(10); g++)
                        {
                            sb.Append(RandomObject.Next(6, 10).ToString());
                        }
                    }

                    gitgwo = sb.ToString();
                    break;

                default:
                    break;
            }

            // 输出最终结果
            Debug.WriteLine($"转换后的结果: {gitgwo}");
            return $"\u200B{gitgwo}";
        }

        /// <summary>将字符串转换为六进制形式。</summary>
        private static string TenkanStringToSix(string str) =>
            string.Join(" ", str.Select(c => IntegerToSenary(c)));
        /// <summary>将十进制数转换为六进制数。</summary>
        private static string IntegerToSenary(int integer)
        {
            StringBuilder sixBuilder = new StringBuilder();
            do
            {
                sixBuilder.Append(integer % 6);
                integer /= 6;
            }
            while (integer > 0);
            return sixBuilder.ToString().TrimEnd('0').Reverse();
        }

        /// <summary>通过HashAlgorithm的TransformBlock方法对流进行叠加运算获得MD5。<br />
        /// 实现稍微复杂，但可使用与传输文件或接收文件时同步计算MD5值。<br />
        /// 可自定义缓冲区大小，计算速度较快。<br /><br />
        /// 
        /// 来自：https://blog.csdn.net/qiujuer/article/details/19344527。</summary>
        /// <param name="path">文件地址</param>
        /// <returns>MD5Hash</returns>
        public static string GetMD5ByHashAlgorithm(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException($"<{path}>, 不存在");

            int bufferSize = 1024 * 16;//自定义缓冲区大小16K
            byte[] buffer = new byte[bufferSize];
            using (Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
                int readLength; // 每次读取长度
                var output = new byte[bufferSize];
                while ((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //计算MD5
                    hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
                }
                //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0)
                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);

                string md5 = BitConverter.ToString(hashAlgorithm.Hash);
                hashAlgorithm.Clear();
                inputStream.Close();

                md5 = md5.Replace("-", "");
                Debug.WriteLine($"Md5 of {path}: {md5}");
                return md5;
            }
        }

        /// <summary>清除所有释放的文件。</summary>
        public static void ClearOutedFiles()
        {
            foreach (string i in OutedFilenames)
            {
                try
                {
                    DeleteFile(i);
                }
                catch (Exception ex)
                {
                    错误弹窗(ex, $"{Program.L10N[Program.Lang].L10n.TempfileCleaningError}\n\n{ex}");
                }
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>设置鼠标指针。</summary>
        public static Cursor MakeCursor(Bitmap cursor, Point hotPoint)
        {
            using (Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotPoint.X, cursor.Height * 2 - hotPoint.Y))
            using (Graphics g = Graphics.FromImage(myNewCursor))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(cursor, cursor.Width - hotPoint.X, cursor.Height - hotPoint.Y, cursor.Width, cursor.Height);
                return new Cursor(myNewCursor.GetHicon());
            }
        }

        /// <summary>将布尔值转化为数字。</summary>
        public static int BoolToInt(bool value) => value ? 1 : 0;

        /// <summary>将嵌入的资源引入本地，并返回本地地址。</summary>
        public static string EmbedToOutside(Stream stream, string filename)
        {
            if (stream != null)
            {
                filename = Path.Combine(Path.GetTempPath(), filename);
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
                Debug.WriteLine($"ABC {filename}");
                OutedFilenames.Append(filename);
                return filename;
            }
            return null;
        }

        /// <summary>检查字符串是否包含文件夹中不能有的字符。</summary>
        public static bool NoInvalidFilenameChar(string a)
        {
            return !Regex.IsMatch(a, "[\\*\"<>\\|\\?\t\n\f\r]");
        }

        /// <summary>判断是否为正确的路径。</summary>
        public static bool IsGoodPath(string path) =>
            NoInvalidFilenameChar(path) && path.Length > 2 &&
            !path.Substring(2).Contains(':') && path[0] != ':';

        /// <summary>判断是否为正确的文件夹名。</summary>
        public static bool IsGoodFoldername(string folderName) =>
            !string.IsNullOrWhiteSpace(folderName) && NoInvalidFilenameChar(folderName) && !folderName.Contains(":");

        /// <summary>将值限制在一个区间，格式：值，最小值，最大值。</summary>
        public static int Clamp(int value, int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum is bigger than maximum.");
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        /// <summary>将简单错误信息弹出。</summary>
        /// <param name="msg">对话框显示的文字。</param>
        public static void 弹窗(string msg) => new NoticeDialog(msg);

        /// <summary>将复杂错误信息弹出。</summary>
        /// <param name="msg">对话框显示的文字。</param>
        public static void 错误弹窗(Exception ex, string msg = null) => new NoticeDialog(ex, msg);

        /// <summary>弹窗函数的升级版，返回bool判断用户点击了是还是否。</summary>
        /// <param name="msg">弹出的信息</param>
        public static bool 弹窗Yesno(string msg) => new NoticeDialog(msg, true);

        /// <summary>将复杂错误信息弹出，并返回是或否。</summary>
        /// <param name="msg">对话框显示的文字。</param>
        public static bool 错误弹窗Yesno(Exception ex, string msg = null) => new NoticeDialog(ex, msg, true);
    }
}
