Imports System.Threading
Imports System.IO
Public Class Form1
#Region "Variables"
    Dim Game As SByte = 0 'Game Selection
    Dim Group As SByte = 0 'Shiny Group Selection
    Dim rnd(1) As Boolean '{Is Shiny Group random?, With Quirky?}
    Dim IDs(1) As Integer '{TID, SID} as numbers
    Dim hIDs(1) As String '{TID, SID} as hex
    Dim code(3) As String 'AR Codes {D, P, Pt, HGSS}
    Dim prob As Boolean = False 'Is there an Error?

    Dim apppath As String = My.Application.Info.DirectoryPath 'Path to .exe directory
    Dim res As String = System.IO.Path.GetFullPath(Application.StartupPath & "\..\..\Resources\") 'Path to Project Resources
    Dim TempPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp" 'Path to Temp
#End Region

    'Startup
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateChk()
        rnd(0) = Nothing
        rnd(1) = Nothing
        Directory.CreateDirectory(TempPath & "/Groups")
        Dim name As String = TempPath & "/Groups"
        File.WriteAllText(name & "/SG1.txt", My.Resources.Shiny_Group_1)
        File.WriteAllText(name & "/SG2.txt", My.Resources.Shiny_Group_2)
        File.WriteAllText(name & "/SG3.txt", My.Resources.Shiny_Group_3)
        File.WriteAllText(name & "/SG4.txt", My.Resources.Shiny_Group_4)
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

    'On Exit
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Directory.Delete(TempPath & "/Groups", True)
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
        If My.Computer.Network.IsAvailable Then
            My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/PlasticJustice/CuteCharmIDGenie/master/CuteCharmIDGen/bin/Debug/version.txt", TempPath & "\vsn.txt")
            Dim Reader As New IO.StreamReader(TempPath & "\vsn.txt")
            Dim v As String = Reader.ReadToEnd
            Reader.Close()
            System.IO.File.Delete(TempPath & "\vsn.txt")
            If Application.ProductVersion <> v Then
                System.IO.File.WriteAllText(res & "/date.txt", (System.DateTime.Today.Year & "/" & System.DateTime.Today.Month & "/" & System.DateTime.Today.Day))
            End If
        End If
        LinkLabel1.Hide()
#Else
        System.IO.File.WriteAllText(TempPath & "\date.txt", My.Resources._date)
        Dim dat As String = System.IO.File.ReadAllText(TempPath & "\date.txt")
        Me.Text = "Cute Charm Glitch ID Generator (" & dat & ")"
        If My.Computer.Network.IsAvailable Then
            My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/PlasticJustice/CuteCharmIDGenie/master/CuteCharmIDGen/Resources/date.txt", TempPath & "\dt.txt")
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
    Private Function genAR()
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
00000084 " & hIDs(1) & hIDs(0) & "
D2000000 00000000"

        If Game = 1 Then
            Return code(0)
        ElseIf Game = 2 Then
            Return code(1)
        ElseIf Game = 3 Then
            Return code(2)
        ElseIf Game = 4 Or Game = 5 Then
            Return code(3)
        Else
            Return "Error"
        End If
    End Function

    'Greys out PictureBoxes
    Private Sub drawDE(ByVal pb As PictureBox)
        pb.Enabled = False
        ControlPaint.DrawImageDisabled(pb.CreateGraphics, pb.BackgroundImage, 0, 0, Color.Gray)
    End Sub

    'Checks for empty options
    Private Sub chks()
        If Game = 0 Then
            gG.ForeColor = Color.Red
            prob = True
        ElseIf Group = 0 And rC.Checked = True Then
            gSG.ForeColor = Color.Red
            prob = True
        ElseIf rnd(0) = Nothing And rC.Checked = False Then
            gRC.ForeColor = Color.Red
            prob = True
        Else
            gG.ForeColor = DefaultForeColor
            gRC.ForeColor = DefaultForeColor
            gSG.ForeColor = DefaultForeColor
            prob = False
        End If
    End Sub

    'Picks IDs
    Private Sub pickID()
        Dim name As String
        If Group = 1 Then
            name = TempPath & "/Groups/SG1.txt"
        ElseIf Group = 2 Then
            name = TempPath & "/Groups/SG2.txt"
        ElseIf Group = 3 Then
            name = TempPath & "/Groups/SG3.txt"
        ElseIf Group = 4 Then
            name = TempPath & "/Groups/SG4.txt"
        Else
            Exit Sub
        End If
        Dim list As String = File.ReadAllText(name)
        Dim sets() As String
        sets = list.Split("
")
        Dim gen As New Random
        Dim pick As Integer = gen.Next(1, UBound(sets))
        Dim sel As String = sets(pick)
        Dim sep As String() = sel.Split("/")
        IDs(0) = Convert.ToUInt32(sep(LBound(sep)))
        IDs(1) = Convert.ToUInt32(sep(UBound(sep)))
        vchk()
        If prob = True Then
            Exit Sub
        End If
        TID.Text = "TID: " & IDs(0)
        SID.Text = "SID: " & IDs(1)
        TID.Show()
        SID.Show()
    End Sub

    'Verifies IDs
    Private Sub vchk()
        Dim result As Integer = ((IDs(0) Xor IDs(1)) >> 3)
        If result = (Group - 1) Then
            prob = False
        Else
            prob = True
        End If
    End Sub

    'Executes AR code generation
    Private Sub bGO_Click(sender As Object, e As EventArgs) Handles bGO.Click
        chks()
        If prob = True Then
            Exit Sub
        End If
        If rnd(0) = True Then
            Dim gen As New Random
            If rnd(1) = False Then
                Group = gen.Next(1, 31) / 10
            ElseIf rnd(1) = True Then
                Group = gen.Next(1, 41) / 10
            End If
        End If
        pickID()
        Thread.Sleep(300)
        hIDs(0) = Hex(IDs(0))

        hIDs(1) = Hex(IDs(1))
        For i = 0 To 3 Step 1
            If hIDs(0).Length < 4 Then
                hIDs(0) = "0" & hIDs(0)
            End If
            If hIDs(1).Length < 4 Then
                hIDs(1) = "0" & hIDs(1)
            End If
        Next i
        AR.Text = genAR()
        AR.Enabled = True
    End Sub

    'Copy AR Code to clipboard
    Private Sub AR_Click(sender As Object, e As EventArgs) Handles AR.Click, AR.MouseClick, AR.DoubleClick, AR.MouseDoubleClick
        Clipboard.Clear()
        Clipboard.SetText(AR.Text, TextDataFormat.Text)
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
#Region "Game Selection"
    Private Sub rD_CheckedChanged(sender As Object, e As EventArgs) Handles rD.CheckedChanged
        If rD.Checked = True Then
            Game = 1
        Else
            Game = 0
        End If
    End Sub
    Private Sub rP_CheckedChanged(sender As Object, e As EventArgs) Handles rP.CheckedChanged
        If rP.Checked = True Then
            Game = 2
        Else
            Game = 0
        End If
    End Sub
    Private Sub rPt_CheckedChanged(sender As Object, e As EventArgs) Handles rPt.CheckedChanged
        If rPt.Checked = True Then
            Game = 3
        Else
            Game = 0
        End If
    End Sub
    Private Sub rHG_CheckedChanged(sender As Object, e As EventArgs) Handles rHG.CheckedChanged
        If rHG.Checked = True Then
            Game = 4
        Else
            Game = 0
        End If
    End Sub
    Private Sub rSS_CheckedChanged(sender As Object, e As EventArgs) Handles rSS.CheckedChanged
        If rSS.Checked = True Then
            Game = 5
        Else
            Game = 0
        End If
    End Sub
#End Region


End Class
