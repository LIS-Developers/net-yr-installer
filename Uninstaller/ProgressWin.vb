Imports System.IO
Imports System.Reflection

''' <summary>删除文件窗口。</summary>
Public Class ProgressWin
    Public fileNumber As Integer = 0
    Dim progress As Integer = 0

    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()
        Text = MainUninst.Text
        Application.DoEvents()
        TraverseFoldersCount(MainUninst.InstallPath)
    End Sub

    Sub TraverseFolders(folderPath As String)
        Debug.WriteLine($"操作此文件夹：{folderPath}")

        Try
            '获取文件夹中的所有文件
            Dim files As String() = Directory.GetFiles(folderPath)
            For Each file As String In files
                Debug.WriteLine("文件: " & file)
                Threading.Thread.Sleep(100)
                Label1.Text = L10n("Deleting").Replace("%s", file)
                progress += 1
                ProgressBar1.Value = CInt(progress * 100 / fileNumber)
                Debug.WriteLine(progress)
                Debug.WriteLine(fileNumber)

                ' 不要把自身删除了，因为根本删不了
                If file = Assembly.GetExecutingAssembly().Location Then
                    Return
                End If

                ' 删除文件
                Try
                    IO.File.Delete(file)
                Catch
                    MessageBox.Show(L10n("DeleteFailed").Replace("%s", file))
                    Debug.WriteLine($"Failed: {file}")
                End Try

                ' 更新文本
                Application.DoEvents()
            Next

            ' 获取文件夹中的所有子文件夹
            Dim subFolders As String() = Directory.GetDirectories(folderPath)
            For Each subFolder As String In subFolders
                Debug.WriteLine("子文件夹: " & subFolder)
                ' 递归遍历子文件夹（弃用，直接一锅端得了）
                ' TraverseFolders(subFolder)
                Label1.Text = L10n("Deleting").Replace("%s", subFolder)
                progress += 1
                ProgressBar1.Value = CInt(progress * 100 / fileNumber)
                Application.DoEvents()
                Directory.Delete(subFolder, True)
            Next
        Catch ex As Exception
            Debug.WriteLine("出错: " & ex.ToString())
        End Try
    End Sub

    Sub TraverseFoldersCount(folderPath As String)
        Debug.WriteLine($"操作此文件夹1：{folderPath}")
        Try
            '获取文件夹中的所有文件 get all subfiles.
            Dim files As String() = Directory.GetFiles(folderPath)
            For Each file As String In files
                fileNumber += 1
            Next

            '获取文件夹中的所有子文件夹 get all subfolders
            Dim subFolders As String() = Directory.GetDirectories(folderPath)
            fileNumber += subFolders.Length

        Catch ex As Exception
            Debug.WriteLine("出错: " & ex.ToString())
        End Try
    End Sub
End Class
