using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using YuriInstaller.ExtraWindows;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            古希腊掌管翻译的神。添加新语言方法： 
            1) 先把底下的L10n类折起来（如果没折的话），方便切换到要修改的地方（因为全程跟他没关系）；
            2）在底下注册你的const，比如我要加个英语，public const string en = "en";；
            3）写好你的翻译json，可以参考自带的zh_TW.json和zh_CN.json；
            4）在字典L10N里注册你的Language结构体，格式为{许可证，JsonSerializer.Deserialize<L10n>(语言json)}
            5）如果添加新许可证/json，可以添加进资源，或者写相对目录，只要能读到就行；
            6）如果你要添加新条目，就打开L10n类，添加一个public string（不要忘了{ get; set }）；
            7）如果需要格式化，就写在Initialize方法里，参照原有句子；
            8）调用语言条目使用(_StartWindow.)L10N.xxx；
            9）如果你没源代码或者懒得改就直接把json塞打包器里面。
         */
    }

    /// <summary>翻译类。</summary>
    internal sealed class L10n
    {
        #region 条目

        private bool _inited = false;

        /// <summary>语言代码。<br />
        /// Language Code.</summary>
        public string LangCode { get; private set; } = "en";

        /// <summary>全局字体。</summary>
        public Font GeneralFont { get; private set; }   // 全局使用的字体类型、大小、样式。 

        /// <summary>比普通西文字体小三号。</summary>
        public Font Little3Arial { get; private set; }

        /// <summary>MS Sanserif小两号。</summary>
        public Font Little2Standard { get; private set; }

        /// <summary>比普通全局字体小一号。</summary>
        public Font Little1 { get; private set; }

        /// <summary>比普通西文字体小两号的粗体。</summary>
        public Font Little1B { get; private set; }

        /// <summary>比普通西文字体小两号的粗体。</summary>
        public Font Little2ArialB { get; private set; }

        /// <summary>字体不随语言改变，硬编码为MS Sans Serif。</summary>
        public Font Standard { get; private set; }

        /// <summary>背景文字字体。</summary>
        public Font BackgroundFont { get; private set; } = null;

        /// <summary>底部栏。</summary>
        public string[,] BottomBarText { get; private set; } = new string[7, 5];

        /// <summary>按钮一。</summary>
        public string[] BottomBtn1 { get; private set; } = new string[7];

        /// <summary>按钮2。</summary>
        public string[] BottomBtn2 { get; private set; } = new string[7];

        #region 用于数组
        public string BottomBtn10 { get; set; } = "Quit";
        public string BottomBtn11 { get; set; } = "Quit";
        public string BottomBtn12 { get; set; } = null;
        public string BottomBtn13 { get; set; } = null;
        public string BottomBtn14 { get; set; } = null;
        public string BottomBtn15 { get; set; } = null;
        public string BottomBtn16 { get; set; } = "Exit";

        public string BottomBtn20 { get; set; } = "Continue";
        public string BottomBtn21 { get; set; } = "Next";
        public string BottomBtn22 { get; set; } = "Verify";
        public string BottomBtn23 { get; set; } = "Ok";
        public string BottomBtn24 { get; set; } = "Install";
        public string BottomBtn25 { get; set; } = null;
        public string BottomBtn26 { get; set; } = string.Empty;

        public string BottomBarText00 { get; set; } = "Welcome Back, Commander.";
        public string BottomBarText01 { get; set; } = "Quit installer.";
        public string BottomBarText02 { get; set; } = "Start installing {0}.";
        public string BottomBarText03 { get; set; } = string.Empty;
        public string BottomBarText04 { get; set; } = string.Empty;

        public string BottomBarText10 { get; set; } = string.Empty;
        public string BottomBarText11 { get; set; } = null;
        public string BottomBarText12 { get; set; } = "Agree license and continue.";
        public string BottomBarText13 { get; set; } = "Return to back page.";
        public string BottomBarText14 { get; set; } = string.Empty;

        public string BottomBarText20 { get; set; } = string.Empty;
        public string BottomBarText21 { get; set; } = null;
        public string BottomBarText22 { get; set; } = null;
        public string BottomBarText23 { get; set; } = null;
        public string BottomBarText24 { get; set; } = string.Empty;

        public string BottomBarText30 { get; set; } = string.Empty;
        public string BottomBarText31 { get; set; } = null;
        public string BottomBarText32 { get; set; } = "Select done, go to next page.";
        public string BottomBarText33 { get; set; } = null;
        public string BottomBarText34 { get; set; } = string.Empty;

        public string BottomBarText40 { get; set; } = string.Empty;
        public string BottomBarText41 { get; set; } = null;
        public string BottomBarText42 { get; set; } = "Start install {0}.";
        public string BottomBarText43 { get; set; } = null;
        public string BottomBarText44 { get; set; } = "Browsing folder.";

        public string BottomBarText50 { get; set; } = string.Empty;
        public string BottomBarText51 { get; set; } = string.Empty;
        public string BottomBarText52 { get; set; } = string.Empty;
        public string BottomBarText53 { get; set; } = string.Empty;
        public string BottomBarText54 { get; set; } = string.Empty;

        public string BottomBarText60 { get; set; } = string.Empty;
        public string BottomBarText61 { get; set; } = string.Empty;
        public string BottomBarText62 { get; set; } = "Complete installation.";
        public string BottomBarText63 { get; set; } = string.Empty;
        public string BottomBarText64 { get; set; } = string.Empty;
        #endregion

        /// <summary>主要字体名称。</summary>
        public string GeneralFontFamily { get; set; } = "Segoe UI";

        /// <summary>背景文字字体名称。</summary>
        public string BackgroundFontFamily { get; set; } = null;

        /// <summary>背景文字。</summary>
        public string BackgroundText { get; set; } = "Yuri's Revenge";

        /// <summary>背景字体大小。</summary>
        public float BackgroundFontSize { get; set; } = 64F;

        /// <summary>主要字体大小。</summary>
        public float GeneralFontSize { get; set; } = 12F;

        /// <summary>西文/数字字体名称。</summary>
        public string ArialFontFamily { get; set; } = "Segoe UI";

        /// <summary>应用版本。</summary>
        public string ApplicationVer { get; set; } = "Installer Ver.{0}";

        /// <summary>游戏名称。</summary>
        public string GameName { get; set; } = "Yuri's Revenge";

        /// <summary>长游戏名称。</summary>
        public string LongGameName { get; set; } = "Command & Conquer: Yuri's Revenge";

        /// <summary>窗口标题。</summary>
        public string WindowTitle { get; set; } = "{0} Installer";

        /// <summary>描述。</summary>
        public string Description { get; set; } = "C&C - Yuri's Revenge";

        /// <summary>卸载。</summary>
        public string Uninstall { get; set; } = "Uninstall";

        /// <summary>同意协议。</summary>
        public string AgreeTitle { get; set; } = "I agree to these terms.";

        /// <summary>创建桌面快捷方式。</summary>
        public string CreateDesktopShortcut { get; set; } = "Create Desktop Shortcut";

        /// <summary>创建开始菜单快捷方式。</summary>
        public string CreateStartMenuShortcut { get; set; } = "Create Start Menu Shortcut";

        /// <summary>设置兼容性。</summary>
        public string SetCompatibility { get; set; } = "Set Game Compatibility";

        /// <summary>必选项尤复。</summary>
        public string InstallSetting1 { get; set; } = "Install {0}.";

        /// <summary>选择项。</summary>
        public string InstallSettingA { get; set; } = "Install CG Movies.";

        /// <summary>可选。</summary>
        public string Selectable { get; set; } = " (Selectable)";

        /// <summary>返回Windows。</summary>
        public string ReturnToWindows { get; set; } = "I want to return to Windows.";

        /// <summary>阅读自述文件。</summary>
        public string OpenReadMe { get; set; } = "I want to view the ReadMe file.";

        /// <summary>运行游戏。</summary>
        public string PleaseRunGame { get; set; } = "I want to play {0} now.";

        /// <summary>官方网站。</summary>
        public string SeeWWWeb { get; set; } = "I want to go to the {0} homepage.";

        /// <summary>打开作者的主页。</summary>
        public string SeeMyWeb { get; set; } = "I want to go to author's homepage.";

        /// <summary>打开安装文件夹。</summary>
        public string OpenExplorer { get; set; } = "I want to open installing directory.";

        /// <summary>运行注册机。</summary>
        public string GameRegisterRun { get; set; } = "I want to run registering wizard.";

        /// <summary>背景音乐开关。</summary>
        public string OnOffMusic { get; set; } = "Turn background music on or off.";

        /// <summary>音效开关。</summary>
        public string OnOffSound { get; set; } = "Turn sounds on or off.";

        /// <summary>跳过。</summary>
        public string Button_Skip { get; set; } = "Skip";

        /// <summary>浏览文件夹。</summary>
        public string Button_Browse { get; set; } = "Browse";

        /// <summary>确定。</summary>
        public string Button_OK { get; set; } = "Ok";

        /// <summary>返回按钮。</summary>
        public string Button_Back { get; set; } = "Back";

        /// <summary>取消。</summary>
        public string Button_Cancel { get; set; } = "Cancel";

        /// <summary>音乐开启。</summary>
        public string Button_MusicOpen { get; set; } = "Music: On";

        /// <summary>音乐关闭。</summary>
        public string Button_MusicOff { get; set; } = "Music: Off";

        /// <summary>音效开启。</summary>
        public string Button_SoundOpen { get; set; } = "Sound: On";

        /// <summary>音效关闭。</summary>
        public string Button_SoundOff { get; set; } = "Sound: Off";

        /// <summary>显示制作人员。</summary>
        public string Button_Credit { get; set; } = "Credits";

        /// <summary>关闭视频。</summary>
        public string Button_Close { get; set; } = "Click any area to close it.";

        /// <summary>表示程序正在加载中。</summary>
        public string Laoding { get; set; } = "Loading...";

        /// <summary>安装程序在准备资源。</summary>
        public string Bottomtip { get; set; } = "Initializing, please stand by ...";

        public string BottomtipAgree { get; set; } = "To install {0}, you must accept this agreement.";

        /// <summary>找不到指定文件。</summary>
        public string FileNotFound { get; set; } = "File not found: {0}\nLaunch failed.";

        /// <summary>设置安装路径时展示的文本信息。</summary>
        public string PathPage { get; set; } = "Select the directory where Setup will install {0}.\n{{8}}\nDisk space required: {{0}} B ({{1}} KB，{{2:0.00}} MB，{{3:0.00}} GB)\nDisk space available: {{4}} B ({{5}} KB，{{6:0.00}} MB，{{7:0.00}} GB)";

        /// <summary>小提示信息。</summary>
        public string LittleTip { get; set; } = "\n";

        /// <summary>用于展示完成进度百分比的文本格式。</summary>
        public string Percents { get; set; } = "{0}% Completed";

        /// <summary>设置开始菜单文件夹名。</summary>
        public string StartFolderName { get; set; } = "Set start menu folder name.";

        /// <summary>要安装到的文件夹。</summary>
        public string AbleFolder { get; set; } = "Available drives:";

        /// <summary>选择的安装路径不合法。</summary>
        public string InvaildPath { get; set; } = "{0}\n\nInvalid path name! Make sure you use the format 'drive:\\directory'.";

        /// <summary>磁盘空间不足。</summary>
        public string DisknotEnough { get; set; } = "Disk space is not enough, change a disk.";

        /// <summary>目录创建失败。</summary>
        public string CreateDirFailed { get; set; } = "Failed to create directory.";

        /// *<summary>所选目录不为空。</summary>
        public string DisknotEmpty { get; set; } = "Directory is not empty, rewrite it?";

        /// <summary>失败。</summary>
        public string Failed { get; set; } = "Failed.";

        /// <summary>暂无许可证的占位符。</summary>
        public string NoLicense { get; set; } = "No license here.";

        /// <summary>在安装完成后等场景下引导用户选择接下来操作的文本提示。</summary>
        public string Selections { get; set; } = "Selections:";

        /// <summary>查看自述文件以了解最新更改及注意事项。</summary>
        public string MoreInfo { get; set; } = "For last minute changes && notes, please examine the readme file.\n\nVisit {0} for all of the latest strategies, toumaments, and events.\n\nThe uninstaller requires .NET Framework execution, if you have uninstalled .NET Framework in advance, please use the alternate script {1}.";

        /// <summary>是否要退出安装程序。</summary>
        public string QuitConfirm { get; set; } = "Installation not completed. To exit, click Ok.";

        /// <summary>安装文件若不是遗失就是无效。</summary>
        public string NoSetupBin { get; set; } = "Setup files lost.";

        /// <summary>解压缩文件失败。</summary>
        public string UnpackError { get; set; } = "Failed to extract {0}.";

        /// <summary>显示在右上角面板的文本信息。</summary>
        public string RightTopPanelText { get; set; } = "Thanks for using {0} installer.\nYou're installing {0} {1}.\n\nCopyright EA,Westwood.";

        /// <summary>安装完成。</summary>
        public string ExtraSetting { get; set; } = "{0} has been successfully installed.";

        /// <summary>删除临时文件目录失败。</summary>
        public string TempfileCleaningError { get; set; } = "Failed to delete temp file directory.";

        /// <summary>文件信息不正确。</summary>
        public string FileSetError { get; set; } = "文件信息不正確。";

        /// <summary>从某个位置复制文件到另一个位置失败。</summary>
        public string MD5Error { get; set; } = "The MD5 verification of the installation file failed.";

        /// <summary>选择要安装的部分。</summary>
        public string SelectParts { get; set; } = "Please select the items you would like to install.";

        /// <summary>欢迎用户使用安装程序的文本提示。</summary>
        public string WelcomeInfo { get; set; } = "Welcome to the {0} Setup program.\n\nIt is strongly recommended that you exit all Windows programs before running this Setup prgram.\n\nClick Cancel to quit Setup and then close any programs you have running. \n\nClick Next to continue with the Setup program.\n\nWARNING: This program is protected by copyright law and international treaties. Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted to the maximum extent possible under law.";

        /// <summary>版权相关的商标及注册信息等内容。</summary>
        public string TradeMark { get; set; } = "(c)2001 Electronic Arts. Westwood Studios is a trademark or registered trademark of Electronic Arts in the U.S. and/or other countries. All rights reserved. Westwood Studios is an Electronic Arts(tm) company. Command & Conquer(tm) and Yuri's Revenge(tm) are trademarks or registered trademarks of Electronic Arts Inc. in the U.S. and/or other countries.";

        /// <summary>安装中。</summary>
        public string Installing { get; set; } = "Installing...";

        /// <summary>正在创建开始菜单。</summary>
        public string CreStarShortcut { get; set; } = "Creating Start Menu Shortcut";

        /// <summary>正在解压文件。</summary>
        public string UnPacking { get; set; } = "Extracting {0}";

        /// <summary>单选项项目不同。</summary>
        public string ExceptionDiff { get; set; } = "Label group of ComboLabel {0} not contians itself.";

        /// <summary>创建快捷方式。</summary>
        public string CreatingShortcut { get; set; } = "Creating Desktop Shortcut";

        /// <summary>写入注册表中...。</summary>
        public string Registering { get; set; } = "Writing To Registry";

        /// <summary>内部循环。</summary>
        public string ExceptionRound { get; set; } = "ChildItem has the same one.";

        /// <summary>创建卸载程序。</summary>
        public string CreatingUns { get; set; } = "Creating Uninstall Program";

        /// <summary>没有能运行的东西。</summary>
        public string NullRunning { get; set; } = "Nothing can be running.";

        /// <summary>开始菜单文件夹无效。</summary>
        public string InvaildStartMenu { get; set; } = "Invaild start menu folder name.";

        /// <summary>程序已运行。</summary>
        public string CanRunOnlyOne { get; set; } = "The program is running!";

        /// <summary>Blowfish.dll丢了。</summary>
        public string NoBlowfishDll { get; set; } = "Blowfish.dll not found!";

        /// <summary>返回值非零。</summary>
        public string FailedRetrun { get; set; } = "Execute failed, return {0}.";

        /// <summary>仔细阅读许可协议。</summary>
        public string NeedSeeToEnd { get; set; } = "Please agree the license after read it.";

        /// <summary>制作人员标题。</summary>
        public string CreditsTitle { get; set; } = "Credits";

        /// <summary>制作人员标题+内容。</summary>
        public string Credits { get; private set; }

        /// <summary>安装器版本。</summary>
        public string InsVer { get; private set; }

        /// <summary>动态显示底边栏文本（用于文字不多的情况）。</summary>
        public bool DynamicBottomBarText { get; set; } = false;

        /// <summary>卸载脚本运行前提示。</summary>
        public string TipBeforeRun { get; set; } = "Please run this script as Administrator.";

        /// <summary>路径太长。</summary>
        public string PathTooLong { get; set; } = "Your target path is too long.";

        /// <summary>选择安装语言标题。</summary>
        public string SelectLanguageTitle { get; set; } = "Select Setup Language";

        /// <summary>选择安装语言标签文本。</summary>
        public string SelectLanguageLabel { get; set; } = "Select the language to use during the installation.";

        /// <summary>选择安装语言窗口帮助。</summary>
        public string SelectLanguageHelp { get; set; } = "You can reopen this window by double-clicking the label in the top-right corner of the installer.";

        /// <summary>背景文字样式。<br />
        /// 0普通，1粗体，2斜体，4下划线，8删除线。多种样式请用 | 符号。</summary>
        public FontStyle BackgroundFontStyle { get; set; } = FontStyle.Bold | FontStyle.Italic;

        public string AutoSelect { get; set; } = "Auto Select";

        #endregion

        /// <summary>初始化翻译类，将里面的字符串格式化应用上。</summary>
        public void Initialize(string a)
        {
            if (_inited)
            {
                Debug.WriteLine("已初始化。");
                return;
            }

            LangCode = a;

            BottomBtn12 = BottomBtn12 ?? BottomBtn11;
            BottomBtn13 = BottomBtn13 ?? BottomBtn11;
            BottomBtn14 = BottomBtn14 ?? BottomBtn11;
            BottomBtn15 = BottomBtn15 ?? BottomBtn11;

            BottomBtn25 = BottomBtn25 ?? BottomBtn24;

            BottomBarText11 = BottomBarText11 ?? BottomBarText01;
            BottomBarText21 = BottomBarText21 ?? BottomBarText01;
            BottomBarText23 = BottomBarText23 ?? BottomBarText13;
            BottomBarText31 = BottomBarText31 ?? BottomBarText01;
            BottomBarText33 = BottomBarText33 ?? BottomBarText13;
            BottomBarText41 = BottomBarText41 ?? BottomBarText01;
            BottomBarText43 = BottomBarText43 ?? BottomBarText13;

            BackgroundFontFamily = BackgroundFontFamily ??
               (BackgroundText.ContainsNonEnglish() ? GeneralFontFamily : "Tahoma");

            WindowTitle = string.Format(WindowTitle, GameName);
            BottomBarText02 = string.Format(BottomBarText02, LongGameName);
            BottomBarText42 = string.Format(BottomBarText42, LongGameName);
            InstallSetting1 = string.Format(InstallSetting1, GameName);
            RightTopPanelText = string.Format(RightTopPanelText, GameName, _StartWindow._GameVersion);
            PathPage = string.Format(PathPage, GameName);
            PleaseRunGame = string.Format(PleaseRunGame, LongGameName);
            SeeWWWeb = string.Format(SeeWWWeb, LongGameName);
            MoreInfo = string.Format(MoreInfo, _StartWindow._GameHomepage, _StartWindow._UnsCmd);
            ExtraSetting = string.Format(ExtraSetting, LongGameName);
            WelcomeInfo = string.Format(WelcomeInfo, LongGameName);
            Credits = string.Format(_StartWindow._creditFull, CreditsTitle);
            InsVer = string.Format(ApplicationVer, Assembly.GetExecutingAssembly().GetName().Version);

            var defLabel = Program.L10N[Program.DefaultLang].L10n.SelectLanguageLabel;
            if (defLabel != SelectLanguageLabel)
            {
                SelectLanguageLabel = $"{SelectLanguageLabel}\n{defLabel}";
            }

            BottomBarText = new string[7, 5]
            {
                {BottomBarText00, BottomBarText01, BottomBarText02, BottomBarText03, BottomBarText04},
                {BottomBarText10, BottomBarText11, BottomBarText12, BottomBarText13, BottomBarText14},
                {BottomBarText20, BottomBarText21, BottomBarText22, BottomBarText23, BottomBarText24},
                {BottomBarText30, BottomBarText31, BottomBarText32, BottomBarText33, BottomBarText34},
                {BottomBarText40, BottomBarText41, BottomBarText42, BottomBarText43, BottomBarText44},
                {BottomBarText50, BottomBarText51, BottomBarText52, BottomBarText53, BottomBarText54},
                {BottomBarText60, BottomBarText61, BottomBarText62, BottomBarText63, BottomBarText64}
            };

            /// <summary>按钮一。</summary>
            BottomBtn1 = new string[7]
            {
                BottomBtn10,
                BottomBtn11,
                BottomBtn12,
                BottomBtn13,
                BottomBtn14,
                BottomBtn15,
                BottomBtn16
            };

            /// <summary>按钮2。</summary>
            BottomBtn2 = new string[7]
            {
                BottomBtn20,
                BottomBtn21,
                BottomBtn22,
                BottomBtn23,
                BottomBtn24,
                BottomBtn25,
                BottomBtn26
            };

            GeneralFont = new Font(GeneralFontFamily, GeneralFontSize, FontStyle.Regular, GraphicsUnit.Point, 134);

            BackgroundFont = new Font(BackgroundFontFamily, BackgroundFontSize, BackgroundFontStyle, GraphicsUnit.Point, 134);
            Little3Arial = new Font(ArialFontFamily, GeneralFontSize - 3F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Little2Standard = new Font(_StartWindow._GeneralFont, GeneralFontSize - 2F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Little1 = new Font(GeneralFontFamily, GeneralFontSize - 1F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Little1B = new Font(GeneralFontFamily, GeneralFontSize - 1F, FontStyle.Bold, GraphicsUnit.Point, 134);
            Little2ArialB = new Font(ArialFontFamily, GeneralFontSize - 2F, FontStyle.Bold, GraphicsUnit.Point, 134);
            Standard = new Font(_StartWindow._GeneralFont, GeneralFontSize, FontStyle.Regular, GraphicsUnit.Point, 134);

            _inited = true;
        }
    }

    /// <summary>语言结构体，包含许可协议和语言字符串。</summary>
    internal struct Language
    {
        public string License { get; private set; }
        public L10n L10n { get; private set; }

        /// <summary>如果把许可证设置为null，将显示“暂无许可证”。</summary>
        public Language(string License, L10n L10n)
        {
            this.L10n = L10n;
            this.License = License ?? this.L10n.NoLicense;
        }

        /// <summary>如果只提供语言文件，将显示默认语言的许可证。</summary>
        public Language(L10n L10n)
        {
            this.L10n = L10n;
            License = Resources.LICENSE;
        }
    }

    // 处理客户端使用的文本字符串
    partial class _StartWindow
    {
        /// <summary>此窗口调用的本地化对象。</summary>
        internal static L10n Localization { get; private set; } = Program.L10N[Program.DefaultLang].L10n;

        /// <summary>本地化方法，传入语言参数。</summary>
        public void L10n(string lang)
        {
            Debug.WriteLine("开始加载语言");
            PLayoutAction(() =>
            {
                lang = Program.GetLanguage(lang);
                Debug.WriteLine($"语言设置为：{lang}");
                Localization = Program.L10N[lang].L10n;
                Program.Lang = lang;

                SetCommonFont();
                SetCommonText();

                rbtnMusicOnOff.BtmbarTip = Localization.OnOffMusic;
                rbtnSoundOnOff.BtmbarTip = Localization.OnOffSound;
                chkAgreeLicenses.BtmbarTip = chkAgreeLicenses.CheckDisabled ? Localization.NeedSeeToEnd : null;
                StartMenuPanel.Child.BtmbarTip = Localization.StartFolderName;

                InstallSyoukai2.Text = Localization.TradeMark;
                InstallSyoukai2.Top = Height - (InstallSyoukai2.Height + 100);
                InstallSyoukai1.Text = Localization.WelcomeInfo;
                InstallSyoukai1.Top = InstallSyoukai2.Top - (InstallSyoukai1.Height + 16);

                EndTitle.Text = Localization.ExtraSetting;
                EndLabel1.Text = Localization.Selections;
                EndLabel1.Top = EndTitle.Bottom + 32;
                EndLabel2.Text = Localization.MoreInfo;
                EndLabel2.Top = Height - (EndLabel2.Height + 100);

                SetSetupSettings();
                SetEndingSelection();
                UpdateTextsAndControls();

                if (!StartMenuPanel.Child.KeyPressed)
                {
                    StartMenuPanel.Text = Localization.Description;
                }

                licenseSider.Text = Program.L10N[lang].License;
                licensePanel.Refreshes();

                if (CurrentProgress != _DonePage)
                {
                    rightTopLabel.Text = Localization.RightTopPanelText;
                }
            });

            Debug.WriteLine("语言设置完毕。");
        }

        /// <summary>设置安装选项文本。</summary>
        private void SetSetupSettings()
        {
            chkInstallSet.Text = Localization.InstallSetting1;
            chkInstallSetA.Text = Localization.InstallSettingA;

            Array.ForEach(SetupLbls.Where(i => !i.CheckDisabled).ToArray(), i => i.Text += Localization.Selectable);
        }

        /// <summary>设置安装结束选项文本。</summary>
        private void SetEndingSelection()
        {
            schkRunGame.Text = Localization.PleaseRunGame;
            schkReturnToWindows.Text = Localization.ReturnToWindows;
            schkReadmeFile.Text = Localization.OpenReadMe;
            schkSeeWWWeb.Text = Localization.SeeWWWeb;
            schkSeeMyWeb.Text = Localization.SeeMyWeb;
            schkOpenExplorer.Text = Localization.OpenExplorer;
            schkRegUI.Text = Localization.GameRegisterRun;
        }

        /// <summary>设置不随页面变化的文本。</summary>
        private void SetCommonText()
        {
            Text = Localization.WindowTitle;
            (Owner as Background)?.Retext(Text);

            lblCreditBtn.Text = Localization.Button_Credit;
            lblCredits.Text = Localization.Credits;
            lblInstallerVer.Text = Localization.InsVer;
            chkAgreeLicenses.Text = Localization.AgreeTitle;
            chkCreateDesktopBox.Text = Localization.CreateDesktopShortcut;
            chkCreateStartMenuBox.Text = Localization.CreateStartMenuShortcut;
            chkJianRongXingBox.Text = Localization.SetCompatibility;
            rbtnMusicOnOff.ClickedText = Localization.Button_MusicOpen;
            rbtnMusicOnOff.UnclickedText = Localization.Button_MusicOff;
            rbtnSoundOnOff.ClickedText = Localization.Button_SoundOpen;
            rbtnSoundOnOff.UnclickedText = Localization.Button_SoundOff;
            bottomButton3.Text = Localization.Button_Back;
            bottomButton4.Text = Localization.Button_Browse;
        }

        private void SetCommonFont()
        {
            Array.ForEach(generalFontControls, control => control.Font = Localization.GeneralFont);
            contentTree.Font = Localization.Little2Standard;
            progressBar1.Font = Localization.Little2ArialB;
            progressBar1.ProgressFormat = Localization.Percents;
            license.Font = licenseSider.Font = StartMenuPanel.Font = Localization.Little1;
            pathBoxPanel.Font = Localization.Standard;

            InstallSyoukai1.Font = Localization.LangCode.Contains("en") ? Localization.Little2ArialB : Localization.Little1B;
            lblInstallerVer.Font = rightTopLabel.Font = lblCreditBtn.Font =
                InstallSyoukai2.Font = Localization.Little3Arial;
        }
    }
}
