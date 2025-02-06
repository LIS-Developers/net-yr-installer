Imports System.Threading

''' <summary>本地化用的类。</summary>
Public Module Localization

    ' 古罗马掌管翻译的神——
    ' 想翻译直接复制下面的字典改字就行了；
    ' 接着在下面那个L10n字典里注册一下（记得全小写）；
    ' 下面预设了一些语言代码（注意：你的翻译字典一定要在L10n字典之上！）；
    ' 调用本地化字符串的格式：L10n.L10n("本地化键名")；
    ' 在MainUninst类中调用可省略MainUninst.。

    Public Const zh_CN As String = "zh-CN"
    Public Const zh_TW As String = "zh-TW"
    Public Const en As String = "en"
    Public Const DefLang$ = en
    Public Lang As String = Thread.CurrentThread.CurrentCulture.Name ' 当前语言

    Public L10n As Dictionary(Of String, String)

    ''' <summary>简体中文字典。<br />
    ''' Simplified Chinese.</summary>
    Public zh_cnD As New Dictionary(Of String, String) From {
        {"UninstallText1", "此程序将从你的电脑上卸载{0}。"},
        {"UninstallText2", "本卸载程序将一并删除该文件夹下的任何文件，请注意备份数据。"},
        {"UninstallText3", "点击取消键以退出卸载程序。点击确认以继续卸载程序。"},
        {"Yes", "确定 &Y"},
        {"No", "取消 &N"},
        {"ConfigNotFound", "找不到卸载配置。"},
        {"UninstallSuccess", "卸载成功"},
        {"UninstallSuccessContent", "{0}已成功卸载。"},
        {"OK", "确定"},
        {"Deleting", "删除 %s 中..."},
        {"UninstExe", "{0}卸载程序"},
        {"GameName", "尤里的复仇"},
        {"DeleteFailed", "移除 %s 失败。"},
        {"AlreadyOpenedOne", "卸载程序已在运行。"},
        {"FolderNotSame", "卸载配置与注册表不符。该程序可能并非在本机内安装，亦或是后期移动了位置，确定继续吗？"}
    }

    ''' <summary>繁体中文字典。<br />
    ''' Traditional Chinese.</summary>
    Public zh_twD As New Dictionary(Of String, String) From {
        {"UninstallText1", "此程式將自您的電腦中移除{0}。"},
        {"UninstallText2", "本移除程式將一併移除该資料夾下任何檔案，請註意備份數據。"},
        {"UninstallText3", "點選取消鍵以退出移除程式。 點選下一步以繼續移除程式。"},
        {"Yes", "確定 &Y"},
        {"No", "取消 &N"},
        {"ConfigNotFound", "找不到移除程式配置檔案。"},
        {"UninstallSuccess", "移除成功"},
        {"UninstallSuccessContent", "{0}已成功從您的電腦上移除。"},
        {"OK", "確定"},
        {"Deleting", "刪除 %s 中..."},
        {"UninstExe", "{0}移除程式"},
        {"GameName", "尤里的復仇"},
        {"DeleteFailed", "移除 %s 失敗。"},
        {"AlreadyOpenedOne", "移除程式已在運行。"},
        {"FolderNotSame", "移除配置與系統資料庫不符，確認繼續嗎？"}
    }

    ''' <summary>英文字典。<br />
    ''' English.</summary>
    Public enS As New Dictionary(Of String, String) From {
        {"UninstallText1", "This program will uninstall {0} from your system."},
        {"UninstallText2", "It will remove all files in this folder."},
        {"UninstallText3", "Click Cancel button to quit the Uninstall program. Click Next to continue with the uninstallation."},
        {"Yes", "&Next"},
        {"No", "&Cancel"},
        {"ConfigNotFound", "Can't find uninstaller configuration file,"},
        {"UninstallSuccess", "Uninstalled."},
        {"UninstallSuccessContent", "{0} has removed successfully from your computer."},
        {"OK", "&OK"},
        {"Deleting", "Deleting %s."},
        {"UninstExe", "{0} Uninstall Program"},
        {"GameName", "Yuri's Revenge"},
        {"DeleteFailed", "Failed to remove %s."},
        {"AlreadyOpenedOne", "Uninstaller is running."},
        {"FolderNotSame", "The uninstall configuration does not match the registry, are you sure to continue?"}
    }

    ''' <summary>语言字典。<br />
    ''' Languages.</summary>
    Public Property LangList As Dictionary(Of String, String)() = New Dictionary(Of String, String)() {
        zh_cnD,
        zh_twD,
        enS
    }

    ''' <summary>检查每个语言是否相同。<br />
    ''' Check every languages.</summary>
    Sub New()
        Dim a As List(Of String) = enS.Keys.ToList()
        a.Sort()
        For Each i As Dictionary(Of String, String) In LangList
            Dim b = i.Keys.ToList()
            b.Sort()
            If Not AreListsEqualIgnoringOrder(b, a) Then
                Throw New KeyNotFoundException("Language file is incorrect.")
            End If
        Next i
        Debug.WriteLine("语言一切正常")

        If CheckLanguage(zh_CN) OrElse CheckLanguage("zh-SG") Then
            L10n = zh_cnD
        ElseIf CheckLanguage(zh_TW) OrElse CheckLanguage("zh-HK") OrElse CheckLanguage("zh-MO") Then
            L10n = zh_twD
        Else
            L10n = enS
        End If
    End Sub

    Function CheckLanguage(_lang$) As Boolean
        CheckLanguage = InStr(1, Lang, _lang, CompareMethod.Text) > 0
    End Function

    ''' <summary>判断两个列表是否相同。</summary>
    Function AreListsEqualIgnoringOrder(list1 As List(Of String), list2 As List(Of String)) As Boolean
        If list1.Count <> list2.Count Then
            Return False
        End If

        list1.Sort()
        list2.Sort()

        For i As Integer = 0 To list1.Count - 1
            If list1(i) <> list2(i) Then
                Return False
            End If
        Next i

        Return True
    End Function
End Module
