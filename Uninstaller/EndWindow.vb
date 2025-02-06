''' <summary>卸载结束窗口。</summary>
Public Class EndWindow
    Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Text = L10n("UninstallSuccess")
        Label1.Text = String.Format(L10n("UninstallSuccessContent"), MainUninst.GameName)
        Button1.Text = L10n("OK")
    End Sub

    ''' <summary>按下退出。</summary>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub
End Class