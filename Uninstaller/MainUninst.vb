Imports System.IO
Imports System.Reflection
Imports System.Text
Imports Microsoft.Win32

' /*
'     改文字请到L10n.vb修改。
'     流程说明：
'     1）弹出窗口，询问是否一定要卸载；Start a window and asking sure to uninstall;
'     2）读取uninst.info配置文件，遍历目录一一删除文件；read uninst.info, and deleting files.
'     3）删除注册表和快捷方式；delete registry and shortcuts.
'     4）弹出结束窗口，关闭，删除游戏文件夹。start end window and close, delete game folder.
'  */

''' <summary>程序主窗口。<br />
''' Main window class.</summary>
Public Class MainUninst
    Public Property AssemblyInfo As Assembly = Assembly.GetExecutingAssembly() ' Assembly
    Public Property StartupPath As String = Path.GetDirectoryName(AssemblyInfo.Location) ' 启动路径
    Public uninstConfig As String = Path.Combine(StartupPath, "uninst.info") ' 卸载配置路径
    Public generalFont As Font
    Public GameName As String

    ''' <summary>构造函数。<br />
    ''' Constructor of main window.</summary>
    Public Sub New()
        Debug.WriteLine("开启卸载程序。")

        GameName = L10n("GameName")

        InitializeComponent()

        Text = String.Format(L10n("UninstExe"), GameName) '窗口标题

        ' 设置字体和文字 Set font and text to controls
        Label1.Font = generalFont
        Label2.Font = generalFont
        Label3.Font = generalFont

        Button1.Font = generalFont
        Button2.Font = generalFont

        Label1.Text = String.Format(L10n("UninstallText1"), GameName)
        Label2.Text = L10n("UninstallText2")
        Label3.Text = L10n("UninstallText3")

        Button1.Text = L10n("Yes")
        Button2.Text = L10n("No")
    End Sub

    ''' <summary>点取消就退出。</summary>
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Visible = False
        If File.Exists(uninstConfig) Then
            Try
                ' 挨个读取文件信息
                Using reader As New StreamReader(uninstConfig, Encoding.UTF8)
                    InstallPath = ParseString(reader.ReadLine())
                    UninstallKey = ParseString(reader.ReadLine())
                    StartsMenuPath = ParseString(reader.ReadLine())
                    DeskstopPath = ParseString(reader.ReadLine())
                    SoftwareRegistKey = ParseString(reader.ReadLine())
                    CompatibilityRegistry = ParseString(reader.ReadLine())
                End Using

                ' 检查注册表与配置记录的路径是否一致
                Try
                    If DirectoryNotSame(InstallPath, StartupPath) Then
                        Throw New Exception()
                    End If
                    Dim key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Westwood\Yuri's Revenge", True)
                    If key IsNot Nothing Then
                        Dim install As String = key.GetValue("InstallPath").ToString()
                        If DirectoryNotSame(install, InstallPath) AndAlso
                            DirectoryNotSame(Directory.GetParent(install).FullName, InstallPath) Then
                            Throw New Exception()
                        End If
                        key.Close()
                    Else
                        Throw New Exception()
                    End If
                Catch ex As Exception
                    If MsgBox(L10n("FolderNotSame"), MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                        Visible = True
                        Exit Sub
                    End If
                End Try

                ' 打开卸载窗口 Open Uninstalling window
                Try
                    Dim form2 As New ProgressWin()
                    form2.Show()
                    Debug.WriteLine("正在删除第一层文件夹")
                    form2.TraverseFolders(InstallPath)
                    form2.Close()
                Catch ex As Exception
                    MsgBox(ex.ToString())
                    Debug.WriteLine($"梁如萱我TM真喜欢你啊艹{ex}")
                End Try

#Region "删除注册信息"
                ' 删除掉程序的注册信息
                DeleteRA2YRRegistry()
                DeleteUninstallRegistry(UninstallKey)
                DeleteCompatibilityRegistry(CompatibilityRegistry)
                DeleteDesktopShortcut(DeskstopPath)
                DeleteStartMenuFolder(StartsMenuPath)
                DeleteInstallRegistry(SoftwareRegistKey)
#End Region

                ' 显示结束窗口，删除游戏文件夹 Show the end window and deleting game folder
                Dim form3 As New EndWindow()
                form3.ShowDialog()
                DeleteGameFolder()
                Application.Exit()
            Catch ex As Exception
                MessageBox.Show(ex.ToString())
                Close()
            End Try
        Else
            MessageBox.Show(L10n("ConfigNotFound"))
            Close()
        End If
        Application.Exit()
    End Sub

    Private Function DirectoryNotSame(path1 As String, path2 As String) As Boolean
        ' 确保路径都以反斜杠结尾，使用 Path.Combine 来处理
        DirectoryNotSame = Path.Combine(path1, "1") <> Path.Combine(path2, "1")
    End Function

    ''' <summary>删除兼容性配置。</summary>
    Sub DeleteCompatibilityRegistry(gameExe As String)
        If String.IsNullOrWhiteSpace(gameExe) Then
            Exit Sub
        End If

        Try
            Dim key = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\", True)
            If key IsNot Nothing Then
                key.DeleteValue(gameExe)
                key.Close()
                Debug.WriteLine("已删除兼容性配置")
            End If
        Catch ex As Exception
            Debug.WriteLine($"→←{ex}")
        End Try
    End Sub

    ''' <summary>删除程序卸载信息。</summary>
    Sub DeleteUninstallRegistry(regsKey As String)
        Try
            Dim key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", True)
            If key IsNot Nothing Then
                key.DeleteSubKeyTree(regsKey)
                key.Close()
                Debug.WriteLine("已删除程序卸载信息")
            End If
        Catch ex As Exception
            Debug.WriteLine($"你凭什么不喜欢我啊（T_T）{ex}")
        End Try
    End Sub

    ''' <summary>删除程序注册信息。</summary>
    Sub DeleteInstallRegistry(regsKey As String)
        Try
            Registry.LocalMachine.DeleteSubKey(regsKey)
            Debug.WriteLine("已删除程序注册信息")
        Catch ex As Exception
            Debug.WriteLine($"梁如萱你他妈为什么要拒绝我！{ex}")
        End Try
    End Sub

    ''' <summary>删除红警注册信息。<br/>
    ''' Delete RA2 registry.</summary>
    Sub DeleteRA2YRRegistry()
        Try
            Dim key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Westwood\", True)
            If key IsNot Nothing Then
                key.DeleteSubKeyTree("Red Alert 2")
                key.DeleteSubKeyTree("Yuri's Revenge")
                key.Close()
                Debug.WriteLine("已删除红警注册信息")
            End If
        Catch ex As Exception
            Debug.WriteLine($"→←{ex}")
        End Try
    End Sub

    ''' <summary>删除桌面快捷方式。<br/>
    ''' Delete desktop shortcut.</summary>
    Sub DeleteDesktopShortcut(shortcutPath As String)
        If String.IsNullOrWhiteSpace(shortcutPath) Then
            Return
        End If

        Try
            If File.Exists(shortcutPath) Then
                File.Delete(shortcutPath)
            End If
        Catch ex As Exception
            Debug.WriteLine($"梁如萱你凭什么不喜欢我啊！{ex}")
        End Try
    End Sub

    ''' <summary>删除游戏文件夹。<br/>
    ''' Delete game folder.</summary>
    Sub DeleteGameFolder()
        Dim psi As New ProcessStartInfo With
        {
            .FileName = "cmd.exe",
            .Arguments = $"/C timeout /t 2 & rd /s/q ""{InstallPath}""",
            .CreateNoWindow = True,
            .UseShellExecute = False,
            .WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        }
        Process.Start(psi)
    End Sub

    ''' <summary>删除开始菜单文件夹。<br/>
    ''' Delete Start Menu folder.</summary>
    Sub DeleteStartMenuFolder(starts As String)
        If String.IsNullOrWhiteSpace(starts) Then
            Return
        End If

        Dim psi As New ProcessStartInfo With
        {
            .FileName = "cmd.exe",
            .Arguments = $"/C timeout /t 2 & rd /s/q ""{starts}""",
            .CreateNoWindow = True,
            .UseShellExecute = False,
            .WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        }
        Process.Start(psi)
    End Sub
End Class
