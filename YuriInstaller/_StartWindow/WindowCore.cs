using System.Drawing;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            这里是安装包的配置。
            主要配置各项参数，至于什么意思看注释吧。
         */
    }

    partial class _StartWindow
    {
        /// <summary>启用卸载程序。</summary>
        public const bool _UninstallEnabled = true;
        /// <summary>不随语言改变的字体。</summary>
        public const string _GeneralFont = "MS Sans Serif";

        // 安装包的各个页面
        /// <summary>首页。<br />
        /// Page showing video background at start.</summary>
        public const int _StartPage = 0;
        /// <summary>许可协议页面。<br />
        /// Page showing license.</summary>
        public const int _LicensePage = 1;
        /// <summary>占位页面。<br />
        /// Page Dummy.</summary>
        public const int _DummyPage = 2;
        /// <summary>选择安装的组件页面。<br />
        /// Page showing installing selections.</summary>
        public const int _SelectionPage = 3;
        /// <summary>设置安装路径页面。<br />
        /// Page showing path setting textbox.</summary>
        public const int _PathPage = 4;
        /// <summary>安装中进度页面。<br />
        /// Page showing installing progress.</summary>
        public const int _InstallingPage = 5;
        /// <summary>完成页面。<br />
        /// Page showing installing has done.</summary>
        public const int _DonePage = 6;

        /// <summary>右上角面板最大高度</summary>
        public const int _RightTopPanelHeight = 113;
        /// <summary>动态背景大小。</summary>
        public static Size BackgroundPanelSize { get; } = new Size(632, 570);

        /// <summary>注册游戏的程序，如RegSetMD.exe，也可以是Mo3RegUI那种。</summary>
        public const string _RegSetMD = "SetupReg.exe";
        /// <summary>游戏主程序如gamemd.exe。</summary>
        public const string _GameExeName = "gamemd.exe";
        /// <summary>游戏启动器，如RA2MD.EXE。</summary>
        public const string _GameLauncher = "RA2MD.EXE";
        /// <summary>启动参数，可用于Ares。</summary>
        public const string _LaunchArguments = "";
        /// <summary>自述文件，如readme.doc。</summary>
        public const string _ReadmeFile = "readmemd.doc";
        /// <summary>游戏官网，如www.westwood.com。</summary>
        public const string _GameHomepage = "www.westwood.com";
        /// <summary>游戏作者主页，如https://dtsm.mqmrx.cn。</summary>
        public const string _AuthorHomepage = "https://dtsm.mqmrx.cn";
        /// <summary>路径框默认显示的路径，如C:\Program Files (x86)\。</summary>
        public const string _DefaultPath = @"C:\Program Files (x86)\";
        /// <summary>游戏文件夹名，默认新建这个文件夹安装进去，如RA2MD。</summary>
        public const string _GamePath = @"RA2MD\";
        /// <summary>兼容性设置语句，如~ DWM8And16BitMitigation RUNASADMIN 16BITCOLOR WINXPSP2 HIGHDPIAWARE，意为十六位简化颜色、管理员身份运行、Windows XP (Service Pack 2)兼容性、高DPI。</summary>
        internal const string _jrxStr = "~ DWM8And16BitMitigation RUNASADMIN 16BITCOLOR WINXPSP2 HIGHDPIAWARE";

        /// <summary>制作人员名单。</summary>
        internal const string _creditFull = "{0}\n\n- 주요 공헌 -\nEnderseven Tina\n\n- 기술 고문 -\n군열OwO\n\n- 찬조자 -\nFlactine\n\n- 기술 지지 -\n두포\n\n- 참고 자료 -\nCSDN\n\n- 법무 -\n쌍살보창\n\n- 특별 -\nWestwood Studios: Yuri's Revenge";

        /// <summary>节点补充的宽度，不然点不到</summary>
        public const string _TreeNodePart = "     ";

        /// <summary>卸载程序配置</summary>
        public const string _UnsCmd = "uninst.bat";
        /// <summary>卸载程序</summary>
        public const string _UnsExe = "uninst.exe";
        /// <summary>卸载程序配置</summary>
        public const string _UnsIni = "uninst.info";
        /// <summary>程序图标</summary>
        public const string _UnsExeIcon = "RA2MD.ICO";
        /// <summary>程序版本</summary>
        public const string _GameVersion = "1.001";
        /// <summary>程序注册名（英文）</summary>
        public const string _RegeditName = "Yuri's Revenge";
    }
}
