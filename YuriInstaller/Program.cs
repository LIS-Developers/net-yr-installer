using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using YuriInstaller.ExtraWindows;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

/*
    易语言时期的屎山太多了，现在的C#也是很多屎山。 
 */

namespace YuriInstaller
{
    internal static class Program
    {
        internal const string zh_TW = "zh-tw";
        internal const string en = "en-us";
        internal const string zh_CN = "zh-cn";
        internal const string ko_KR = "ko-kr";
        internal static Font defaultFont = new Font("MS Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 134);

        /// <summary>箭头鼠标指针。</summary>
        public static Cursor ReleaseCursor { get; } = MizukiTool.MakeCursor(Resources.release, new Point(0, 0));

        /// <summary>沙漏鼠标指针。</summary>
        public static Cursor WaitingCursor { get; } = MizukiTool.MakeCursor(Resources.waiting, new Point(0, 0));

        internal static string Lang { get; set; } = Thread.CurrentThread.CurrentCulture.Name;
        internal static string PCLang { get; } = Lang;
        internal static string DesktopPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        internal static Dictionary<string, Language> L10N = new Dictionary<string, Language>
        {
            { en, new Language(Resources.LICENSE, new L10n())},
            { zh_TW, new Language(Resources.LICENSETW, JsonSerializer.Deserialize<L10n>(Resources.zh_TW))},
            { zh_CN, new Language(Resources.LICENSECN, JsonSerializer.Deserialize<L10n>(Resources.zh_CN))},
            { ko_KR, new Language(Resources.LICENSE, JsonSerializer.Deserialize<L10n>(Resources.ko_KR))}
        };

        internal static string GetLanguage(string lang)
        {
            lang = lang.ToLower();
            switch (lang)
            {
                case zh_CN:
                case "zh-hans":
                case "zh-sg":
                    return zh_CN;
                default:
                    switch (lang.Substring(0, 2))
                    {
                        case "zh":
                            return zh_TW;
                        case "ko":
                            return ko_KR;
                        case "en":
                        default:
                            return DefaultLang;
                    }
            }
        }

        internal static LangSelection langSelection;

        internal static string DefaultLang { get; } = L10N.First().Key;

        internal static _StartWindow AppForm { get; private set; } = null;

        /// <summary>应用程序的主入口点。</summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            Debug.WriteLine("开启新程序。");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Lang = GetLanguage(Lang);

            // 尝试创建互斥体并获取其所有权，禁止多开。
            using (var mutex = new Mutex(true, "net_yuri_installer", out bool createdNew))
            {
                try
                {
                    // 初始化翻译类。
                    foreach (var i in L10N.Keys)
                    {
                        L10N[i].L10n.Initialize(i);
                    }
                    L10N[en].L10n.BackgroundFontFamily = "Tahoma";

                    if (!createdNew)
                    {
                        // 应用程序已经在运行，显示不能运行提示框
                        MessageBox.Show(L10N[Lang].L10n.CanRunOnlyOne);
                        return;
                    }

                    try
                    {
                        langSelection = new LangSelection();
                        AppForm = new _StartWindow();
                        if (args.Contains("-b"))
                        {
                            var bg = new Background(AppForm);
                        }

                        AppForm.SetArguments(args);
                        langSelection.SetLanguageToForm = AppForm;
                        langSelection.ShowInTaskbar = false;
                        langSelection.HelpButton = false;
                        Debug.WriteLine("设置语言完毕，开始进入程序。");
                        Application.Run(AppForm);
                    }
                    catch (Exception ex)
                    {
                        AppForm?.Close();
                        // 内部错误。
                        _ = new IEWindows($"I {ex}");
                    }
                }
                catch (Exception ex)
                {
                    // 表示外部错误。
                    _ = new IEWindows($"O {ex}");
                }
            }
        }
    }
}