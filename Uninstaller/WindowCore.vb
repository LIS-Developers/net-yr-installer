Partial NotInheritable Class Comment
    ' 这里是卸载程序的信息
End Class

Partial Class MainUninst
    ''' <summary>程序安装路径。</summary>
    Public Property InstallPath As String = String.Empty

    ''' <summary>卸载注册表键名。</summary>
    Public Property UninstallKey As String = String.Empty

    ''' <summary>开始菜单文件夹路径。</summary>
    Public Property StartsMenuPath As String = String.Empty

    ''' <summary>桌面快捷方式路径。</summary>
    Public Property DeskstopPath As String = String.Empty

    ''' <summary>软件注册表键名。</summary>
    Public Property SoftwareRegistKey As String = String.Empty

    ''' <summary>兼容性注册表键名。</summary>
    Public Property CompatibilityRegistry As String = String.Empty
End Class
