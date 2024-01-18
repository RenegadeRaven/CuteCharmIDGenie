Imports System.Windows.Shell

Module Essentials
    'Checks For Update
    Public Sub UpdateCheck()
        If File.Exists(Main.TempPath & "\vsn.txt") Then File.Delete(Main.TempPath & "\vsn.txt")
        If File.Exists(Main.TempPath & "\dt.txt") Then File.Delete(Main.TempPath & "\dt.txt")
#If DEBUG Then
        File.WriteAllText(Main.apppath & "..\..\..\version.txt", My.Application.Info.Version.ToString)
        File.WriteAllText(Main.apppath & "..\..\..\version.json", "{
" & ControlChars.Quote & "version" & ControlChars.Quote & ": " & ControlChars.Quote & My.Application.Info.Version.ToString & ControlChars.Quote & "
}")
        If My.Computer.Network.IsAvailable And Pinger() Then
            My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/RenegadeRaven/CuteCharmIDGenie/master/CuteCharmIDGen/version.txt", Main.TempPath & "\vsn.txt")
            Dim v As String = ReadUpdate(Main.TempPath & "\vsn.txt")
            File.Delete(Main.TempPath & "\vsn.txt")
            If Application.ProductVersion <> v Then File.WriteAllText(Main.res & "/date.txt", (System.DateTime.Today.Year & "/" & System.DateTime.Today.Month & "/" & System.DateTime.Today.Day))
        End If
        Main.lklb_Update.Hide()
#Else
        Main.Text = Main.LangRes.GetString("Title") & " (" & My.Resources._date & ")"
        If My.Computer.Network.IsAvailable And Pinger() Then
            Try
                My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/RenegadeRaven/CuteCharmIDGenie/master/CuteCharmIDGen/Resources/date.txt", Main.TempPath & "\dt.txt")
            Catch
                File.WriteAllText(Main.TempPath & "\dt.txt", " ")
            End Try
            Dim dtt As String = ReadUpdate(Main.TempPath & "\dt.txt")
            File.Delete(Main.TempPath & "\dt.txt")
            If My.Resources._date <> dtt Then
                Main.lklb_Update.Text = Main.LangRes.GetString("Update") & " " & dtt
                Main.lklb_Update.Show()
            Else
                Main.lklb_Update.Hide()
            End If
        Else
            Main.lklb_Update.Hide()
        End If
        File.Delete(Main.TempPath & "\date.txt")
#End If
    End Sub
    Public Function Pinger() As Boolean
        Try
            Return My.Computer.Network.Ping("google.com")
        Catch
            Return False
        End Try
    End Function
    Private Function ReadUpdate(path As String) As String
        Dim Reader As New IO.StreamReader(path)
        Dim dtt As String = Reader.ReadToEnd
        Reader.Close()
        Return dtt
    End Function
End Module
