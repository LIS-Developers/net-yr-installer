namespace YuriInstaller.MizukiTools
{
    /// <summary>用于设置或反映按钮状态的枚举。</summary>
    public enum ButtonStates
    {
        /// <summary>平常状态。</summary>
        Normal = 0,
        /// <summary>点亮状态。</summary>
        Light = 1,
        /// <summary>隐藏状态。</summary>
        Hide = 2,
        /// <summary>移动状态。</summary>
        MoveA = 3,
        /// <summary>移动但不显示文字。</summary>
        MoveB = 3
    }

    /// <summary>用于设置或反映按钮状态的枚举。</summary>
    public enum TenkanOption
    {
        /// <summary>普通转换。</summary>
        None,
        /// <summary>伪装成十进制。</summary>
        Gisou
    }
}
