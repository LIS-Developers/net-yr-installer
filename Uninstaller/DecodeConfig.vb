''' <summary>解析配置文件的类。</summary>
Public Module DecodeConfig
    ''' <summary>将编码的字符解析为字符串。</summary>
    Function ParseString(str As String) As String
        ' 字符串无效就返回
        If String.IsNullOrWhiteSpace(str) Then
            Return String.Empty
        Else
            Return str.Trim()
        End If
    End Function

    ''' <summary>将六进制数转换为十进制数。</summary>
    Function Hex6ToDecimal(hex6String As String) As Integer
        Dim decimalValue As Integer = 0
        Dim power As Integer = 1

        ' 转换
        For i As Integer = Len(hex6String) To 1 Step -1
            Select Case Mid$(hex6String, i, 1)
                Case "0" : decimalValue += 0 * power
                Case "1" : decimalValue += 1 * power
                Case "2" : decimalValue += 2 * power
                Case "3" : decimalValue += 3 * power
                Case "4" : decimalValue += 4 * power
                Case "5" : decimalValue += 5 * power
                Case Else
                    ' 如果输入的字符串包含无效字符
                    decimalValue = 0
                    Exit For
            End Select
            power *= 6
        Next i

        Hex6ToDecimal = decimalValue
    End Function
End Module
