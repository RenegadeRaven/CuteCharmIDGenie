Imports System.Threading
Public Class Form1
#Region "Variables"
    Dim Game As SByte = 0 'Game Selection
    Dim Group As SByte = 0 'Shiny Group Selection
    Dim rnd(1) As Boolean '{Is Shiny Group random?, With Quirky?}
    Dim IDs(1) As Short '{TID, SID} as numbers
    Dim hIDs(1) As String '{TID, SID} as hex
    Dim code(3) As String 'AR Codes {D, P, Pt, HGSS}

    Dim apppath As String = My.Application.Info.DirectoryPath 'Path to .exe directory
    Dim res As String = System.IO.Path.GetFullPath(Application.StartupPath & "\..\..\Resources\") 'Path to Project Resources
    Dim TempPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp" 'Path to Temp
#End Region

    'Startup
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateChk()

    End Sub
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        drawDE(pg1)
        drawDE(pg2)
        drawDE(pg3)
        drawDE(pg4)
        gSG.Enabled = False
        rD.Checked = False
        rR.PerformClick()
        TID.Visible = False
        SID.Visible = False
        AR.Enabled = False
    End Sub

    'Checks For Update
    Private Sub UpdateChk()
        If System.IO.File.Exists(TempPath & "\vsn.txt") Then
            System.IO.File.Delete(TempPath & "\vsn.txt")
        End If
        If System.IO.File.Exists(TempPath & "\dt.txt") Then
            System.IO.File.Delete(TempPath & "\dt.txt")
        End If
#If DEBUG Then
        System.IO.File.WriteAllText(apppath & "/version.txt", My.Application.Info.Version.ToString)
        System.IO.File.WriteAllText(apppath & "/version.json", "{
" & ControlChars.Quote & "version" & ControlChars.Quote & ": " & ControlChars.Quote & My.Application.Info.Version.ToString & ControlChars.Quote & "
}")
        'If My.Computer.Network.IsAvailable Then
        '    My.Computer.Network.DownloadFile("LINK HERE: VERSION", TempPath & "\vsn.txt")
        '    Dim Reader As New IO.StreamReader(TempPath & "\vsn.txt")
        '    Dim v As String = Reader.ReadToEnd
        '    Reader.Close()
        '    System.IO.File.Delete(TempPath & "\vsn.txt")
        '    If Application.ProductVersion <> v Then
        '        System.IO.File.WriteAllText(res & "/date.txt", (System.DateTime.Today.Year & "/" & System.DateTime.Today.Month & "/" & System.DateTime.Today.Day))
        '    End If
        'End If
        LinkLabel1.Hide()
#Else
        System.IO.File.WriteAllText(TempPath & "\date.txt", My.Resources._date)
        Dim dat As String = System.IO.File.ReadAllText(TempPath & "\date.txt")
        Me.Text = "Cute Charm Glitch ID Generator (" & dat & ")"
        If My.Computer.Network.IsAvailable Then
            My.Computer.Network.DownloadFile("LINK HERE: DATE", TempPath & "\dt.txt")
            Dim Reader As New IO.StreamReader(TempPath & "\dt.txt")
            Dim dtt As String = Reader.ReadToEnd
            Reader.Close()
            System.IO.File.Delete(TempPath & "\dt.txt")
            If dat <> dtt Then
                LinkLabel1.Text = "New Update Available! " & dtt
                LinkLabel1.Show()
            Else
                LinkLabel1.Hide()
            End If
        Else
            LinkLabel1.Hide()
        End If
        System.IO.File.Delete(TempPath & "\date.txt")
#End If
    End Sub

    'Generate AR Code
    Private Sub genAR()
        'Diamond
        code(0) = ""

        'Pearl
        code(1) = ""

        'Platinum
        code(2) = "94000130 FFFB0000
B2101D40 00000000
0000008C " & hIDs(1) & hIDs(0) & "
D2000000 00000000"

        'HeartGold/SoulSilver
        code(3) = "94000130 FFFB0000
B2111880 00000000
00000084 SSSSTTTT
D2000000 00000000"
    End Sub

    'Greys out PictureBoxes
    Private Sub drawDE(ByVal pb As PictureBox)
        pb.Enabled = False
        ControlPaint.DrawImageDisabled(pb.CreateGraphics, pb.BackgroundImage, 0, 0, Color.Gray)
    End Sub

#Region "Specific Shiny Group RadioButtons"
    'Shiny Group 1
    Private Sub sg1_CheckedChanged(sender As Object, e As EventArgs) Handles sg1.CheckedChanged
        If sg1.Checked = True Then
            Group = 1
            drawDE(pg2)
            drawDE(pg3)
            drawDE(pg4)
        Else
            Group = 0
            pg2.Enabled = True
            pg3.Enabled = True
            pg4.Enabled = True
        End If
    End Sub

    'Shiny Group 2
    Private Sub sg2_CheckedChanged(sender As Object, e As EventArgs) Handles sg2.CheckedChanged
        If sg2.Checked = True Then
            Group = 2
            drawDE(pg1)
            drawDE(pg3)
            drawDE(pg4)
        Else
            Group = 0
            pg1.Enabled = True
            pg3.Enabled = True
            pg4.Enabled = True
        End If
    End Sub

    'Shiny Group 3
    Private Sub sg3_CheckedChanged(sender As Object, e As EventArgs) Handles sg3.CheckedChanged
        If sg3.Checked = True Then
            Group = 3
            drawDE(pg2)
            drawDE(pg1)
            drawDE(pg4)
        Else
            Group = 0
            pg2.Enabled = True
            pg1.Enabled = True
            pg4.Enabled = True
        End If
    End Sub

    'Shiny Group 4
    Private Sub sg4_CheckedChanged(sender As Object, e As EventArgs) Handles sg4.CheckedChanged
        If sg4.Checked = True Then
            Group = 4
            drawDE(pg2)
            drawDE(pg3)
            drawDE(pg1)
        Else
            Group = 0
            pg2.Enabled = True
            pg3.Enabled = True
            pg1.Enabled = True
        End If
    End Sub
#End Region
#Region "Shiny Group Selection"
    'User Chooses Shiny Group
    Private Sub rC_CheckedChanged(sender As Object, e As EventArgs) Handles rC.CheckedChanged
        If rC.Checked = True Then
            rnd(0) = False
            gSG.Enabled = True
            pg1.Enabled = True
            pg2.Enabled = True
            pg3.Enabled = True
            pg4.Enabled = True
        Else
            rnd(0) = Nothing
            gSG.Enabled = False
            drawDE(pg1)
            drawDE(pg2)
            drawDE(pg3)
            drawDE(pg4)
        End If
    End Sub

    'Shiny Group is random without Group 4
    Private Sub rR_CheckedChanged(sender As Object, e As EventArgs) Handles rR.CheckedChanged
        If rR.Checked = True Then
            rnd(0) = True
            rnd(1) = False
        Else
            rnd(0) = Nothing
            rnd(1) = Nothing
        End If
    End Sub

    'Shiny Group is random with Group 4
    Private Sub rRQ_CheckedChanged(sender As Object, e As EventArgs) Handles rRQ.CheckedChanged
        If rRQ.Checked = True Then
            rnd(0) = True
            rnd(1) = True
        Else
            rnd(0) = Nothing
            rnd(1) = Nothing
        End If
    End Sub


#End Region


End Class
