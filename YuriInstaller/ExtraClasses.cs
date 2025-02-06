namespace System
{

}

namespace YuriInstaller
{
    /// <summary>存放注释用的类，嘿嘿，我机灵吧！在_StartWindow的partial前面加个这个类就可以直接打开代码而不是设计器了。</summary>
    public static partial class Comment
    {
    }

    /// <summary>游戏卸载信息类。</summary>
    public static class GameUninstInfo
    {
        /// <summary>游戏安装路径。</summary>
        public static string InstallPath { get; set; }

        /// <summary>卸载注册表里的英文名。</summary>
        public static string RegeditName { get; set; }

        /// <summary>开始菜单文件夹路径。</summary>
        public static string StartMenuPath { get; set; }

        /// <summary>桌面快捷方式路径。</summary>
        public static string DesktopShortcut { get; set; }

        /// <summary>程序注册表路径。</summary>
        public static string Regedits { get; set; }

        /// <summary>游戏可执行文件路径（用于兼容性）。</summary>
        public static string GameExePath { get; set; }

        /// <summary>写成程序配置。</summary>
        public static new string ToString() => string.Join<string>("\n", ToArray());

        public static string[] ToArray() => new string[6]
        {
            InstallPath,
            RegeditName,
            StartMenuPath,
            DesktopShortcut,
            Regedits,
            GameExePath
        };
    }
}
