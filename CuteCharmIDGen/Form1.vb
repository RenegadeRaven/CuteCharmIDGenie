Imports System.Threading
Imports System.IO
Imports System.Drawing
Public Class Form1
#Region "Variables"
    Dim TIDchoose As Boolean = False 'Is TID random?
    Dim TSVt As Short = 0 'Target Trainer Shiny Value
    Dim Group As SByte = 0 'Shiny Group Selection
    Dim rnd(1) As Boolean '{Is Shiny Group random?, With Quirky?}
    Dim IDs(1) As Integer '{TID, SID} as numbers
    Dim hIDs(1) As String '{TID, SID} as hex
    Dim butt As String 'Haha. It's Short for Button. The one that activates the AR code
    Dim nButt As String = Nothing 'List of buttons
    Dim prob As Boolean = False 'Is there an Error?
    Dim gender As String

    Dim apppath As String = My.Application.Info.DirectoryPath 'Path to .exe directory
    Dim res As String = Path.GetFullPath(Application.StartupPath & "\..\..\Resources\") 'Path to Project Resources
    Dim TempPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp" 'Path to Temp
    Dim Local As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\CuteCharmIDGenie"
#End Region

    'Startup
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateChk()
        rnd(0) = Nothing
        rnd(1) = Nothing
        LeadList.SelectedIndex = My.Settings.DLead
        If My.Settings.DGame <> Nothing Then
            GameList.SelectedIndex = My.Settings.DGame
        End If
        Do While Not Directory.Exists(Local)
            Try
                If Not Directory.Exists(Local) Then
                    Directory.CreateDirectory(Local)
                    Directory.CreateDirectory(Local & "\Male")
                    Directory.CreateDirectory(Local & "\Female")
                    File.WriteAllBytes(Local & "\Male\174 - MAGIC - D8D7F7D437DE.ek4", My.Resources.MaleLead)
                    File.WriteAllBytes(Local & "\Female\174 - MAGIC - D8D95E400116.ek4", My.Resources.FemaleLead)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Loop
        PicTXT()
    End Sub
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        sg1.Checked = False
        gSG.Enabled = False
        rR.PerformClick()
        rTR.PerformClick()
        AR.Enabled = False
        ActDraw()
        For i = 1 To 18 Step 1
            BoxList.Items.Add("Box " & i)
        Next i
        For i = 1 To 30 Step 1
            SlotList.Items.Add("Slot " & i)
        Next i
        Dim defaultSlot() As String = My.Settings.PCspot.Split("/")
        BoxList.SelectedIndex = defaultSlot(LBound(defaultSlot)) - 1
        SlotList.SelectedIndex = defaultSlot(UBound(defaultSlot)) - 1
        If My.Settings.CCPoke = True Then
            cbLead.Checked = True
            BoxList.Enabled = True
            SlotList.Enabled = True
        ElseIf My.Settings.CCPoke = False Then
            cbLead.Checked = False
            BoxList.Enabled = False
            SlotList.Enabled = False
        End If

        Dim ind As Char() = My.Settings.SavedButt.ToCharArray
        For i = 0 To UBound(ind) Step 1
            Select Case ind(i)
                Case "7"
                    PL_Click(sender, e)
                Case "9"
                    PR_Click(sender, e)
                Case "1"
                    PStart_Click(sender, e)
                Case "3"
                    PSel_Click(sender, e)
                Case "6"
                    PA_Click(sender, e)
                Case "2"
                    PB_Click(sender, e)
                Case "8"
                    PX_Click(sender, e)
                Case "4"
                    PY_Click(sender, e)
                Case "W"
                    PUp_Click(sender, e)
                Case "A"
                    PLeft_Click(sender, e)
                Case "S"
                    PDown_Click(sender, e)
                Case "D"
                    PRight_Click(sender, e)
            End Select
        Next i
        DrawDE(pg1)
        DrawDE(pg2)
        DrawDE(pg3)
        DrawDE(pg4)
    End Sub

    'Checks For Update
    Private Sub UpdateChk()
        If File.Exists(TempPath & "\vsn.txt") Then
            File.Delete(TempPath & "\vsn.txt")
        End If
        If File.Exists(TempPath & "\dt.txt") Then
            File.Delete(TempPath & "\dt.txt")
        End If
#If DEBUG Then
        File.WriteAllText(apppath & "/version.txt", My.Application.Info.Version.ToString)
        File.WriteAllText(apppath & "/version.json", "{
" & ControlChars.Quote & "version" & ControlChars.Quote & ": " & ControlChars.Quote & My.Application.Info.Version.ToString & ControlChars.Quote & "
}")
        If My.Computer.Network.IsAvailable Then
            My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/PlasticJustice/CuteCharmIDGenie/master/CuteCharmIDGen/bin/Debug/version.txt", TempPath & "\vsn.txt")
            Dim Reader As New IO.StreamReader(TempPath & "\vsn.txt")
            Dim v As String = Reader.ReadToEnd
            Reader.Close()
            File.Delete(TempPath & "\vsn.txt")
            If Application.ProductVersion <> v Then
                File.WriteAllText(res & "/date.txt", (System.DateTime.Today.Year & "/" & System.DateTime.Today.Month & "/" & System.DateTime.Today.Day))
            End If
        End If
        LinkLabel1.Hide()
        MenuStrip1.Location = New Point(0, 0)
#Else
        File.WriteAllText(TempPath & "\date.txt", My.Resources._date)
        Dim dat As String = File.ReadAllText(TempPath & "\date.txt")
        Me.Text = "Cute Charm Glitch ID Generator (" & dat & ")"
        If My.Computer.Network.IsAvailable Then
            My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/PlasticJustice/CuteCharmIDGenie/master/CuteCharmIDGen/Resources/date.txt", TempPath & "\dt.txt")
            Dim Reader As New IO.StreamReader(TempPath & "\dt.txt")
            Dim dtt As String = Reader.ReadToEnd
            Reader.Close()
            File.Delete(TempPath & "\dt.txt")
            If dat <> dtt Then
                LinkLabel1.Text = "New Update Available! " & dtt
                MenuStrip1.Location = New Point(113, 0)
                LinkLabel1.Show()
            Else
                LinkLabel1.Hide()
                MenuStrip1.Location = New Point(0, 0)
            End If
        Else
            LinkLabel1.Hide()
        End If
        File.Delete(TempPath & "\date.txt")
#End If
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If My.Computer.Network.IsAvailable Then
            Process.Start("https://github.com/PlasticJustice/CuteCharmIDGenie/releases/latest")
        Else
            MsgBox("No Internet connection!
You can not update at the moment.", vbOKOnly, "Error 404")
        End If
    End Sub

    'Determines Activation Button
    Private Sub ActButt()
        Dim arButt(8) As String
        Dim ind As Char() = nButt.ToCharArray
        My.Settings.SavedButt = nButt
        For i = 0 To UBound(ind) Step 1
            Select Case ind(i)
                Case "7"
                    arButt(i) = My.Resources.L
                Case "9"
                    arButt(i) = My.Resources.R
                Case "1"
                    arButt(i) = My.Resources.Start
                Case "3"
                    arButt(i) = My.Resources.SEL
                Case "6"
                    arButt(i) = My.Resources.A
                Case "2"
                    arButt(i) = My.Resources.B
                Case "8"
                    arButt(i) = My.Resources.X
                Case "4"
                    arButt(i) = My.Resources.Y
                Case "W"
                    arButt(i) = My.Resources.Up
                Case "A"
                    arButt(i) = My.Resources.Left
                Case "S"
                    arButt(i) = My.Resources.Down
                Case "D"
                    arButt(i) = My.Resources.Right
            End Select
        Next i

        Dim tnum As Integer = Convert.ToInt32(arButt(0), 16)
        Dim tbutt = tnum
        For n = 0 To UBound(ind) Step 1
            Dim num As Integer = Convert.ToInt32(arButt(n), 16)
            tbutt = tbutt And num
        Next n
        butt = Convert.ToString(tbutt, 16).ToUpper
    End Sub

    'Setup Activation Buttons Design
    Private Sub ActDraw()
        pDown.BackgroundImage.RotateFlip(RotateFlipType.RotateNoneFlipY)
        pLeft.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipX)
        pLeft.Size = New Size(24, 20)
        pRight.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
        pRight.Size = New Size(24, 20)
        DrawTXT("A", pA, New Point(4, 1))
        DrawTXT("B", pB, New Point(5, 1))
        DrawTXT("X", pX, New Point(5, 1))
        DrawTXT("Y", pY, New Point(6, 1))
        DrawTXT("Select", pSel, New Point(4, 1), True, 9)
        DrawTXT("Start", pStart, New Point(8, 1), True, 9)
        DrawTXT("L", pL, New Point(12, 1))
        DrawTXT("R", pR, New Point(11, 1))
        DrawTXT("↑", pUp, New Point(0, 0))
        DrawTXT("↓", pDown, New Point(0, 2))
        DrawTXT("→", pRight, New Point(4, 0))
        DrawTXT("←", pLeft, New Point(0, 0))
    End Sub

    'Adds needed zeros to hex string
    Private Function Hex_Zeros(ByVal hex_value As String, ByVal length As Integer)
        Dim Str As String = hex_value.ToUpper
        Do While Str.Length < length
            Str = "0" & Str
        Loop
        Return Str
    End Function

    'Does Little Endian
    Private Function LittleEndian(ByVal hex_value As String, ByVal length As Integer)
        Dim s As String = Hex_Zeros(hex_value, length)
        Dim s2 As String = Nothing
        If length = 8 Then
            s2 = s.Skip(6).ToArray() & s.Remove(6, 2).ToArray().Skip(4).ToArray() & s.Remove(4, 4).ToArray().Skip(2).ToArray() & s.Remove(2, 6).ToArray()
        ElseIf length = 4 Then
            s2 = s.Skip(2).ToArray() & s.Remove(2, 2).ToArray()
        End If
        Return s2
    End Function

    'Feeds into LittleEndian
    Private Function toLE(ByVal hex_value As String)
        hex_value = hex_value.Replace(" ", "")
        Do While (hex_value.Length Mod 16) <> 0
            hex_value &= "0"
        Loop
        Dim stringList As New List(Of String)
        For i As Integer = 0 To hex_value.Length - 1 Step 8
            stringList.Add(hex_value.Substring(i, 8))
        Next i
        Dim stringFragments As String() = stringList.ToArray
        Dim stringFragments2(UBound(stringFragments)) As String
        For n As Integer = 0 To UBound(stringFragments) Step 1
            stringFragments2(n) = LittleEndian(stringFragments(n), 8)
        Next n
        Dim endString As String = Nothing
        For c = 0 To UBound(stringFragments2) Step 1
            endString &= stringFragments2(c)
            Select Case (c Mod 2)
                Case 0
                    endString &= " "
                Case 1
                    endString &= "
"
            End Select
        Next c

        Return endString
    End Function

    'Generates AR Code
    Private Function GenAR()
        ActButt()
        Dim Spot As String = Nothing
        Dim arc As String = "94000130 " & butt & "0000
"
        Select Case GameList.SelectedIndex
            Case 0 'Diamond/Pearl
                arc &= "B2106FC0 00000000
00000288 " & hIDs(1) & hIDs(0) & "
"
                Spot = Hex_Zeros(Hex(&HC370 + (BoxList.SelectedIndex * &HFF0) + (SlotList.SelectedIndex * &H88)), 6)
                lGame.Text = "DP"
            Case 1 'Platinum
                arc &= "B2101D40 00000000
0000008C " & hIDs(1) & hIDs(0) & "
"
                Spot = Hex_Zeros(Hex(&HCF44 + (BoxList.SelectedIndex * &HFF0) + (SlotList.SelectedIndex * &H88)), 6)
                lGame.Text = "Pt"
            Case 2 'HeartGold/SoulSilver
                arc &= "B2111880 00000000
00000084 " & hIDs(1) & hIDs(0) & "
"
                Spot = Hex_Zeros(Hex(&HF710 + (BoxList.SelectedIndex * &H1000) + (SlotList.SelectedIndex * &H88)), 6)
                lGame.Text = "HGSS"
        End Select
        If My.Settings.CCPoke = True Then
            EK4toAR(Local & gender & ComboBox1.Text & ".ek4")
            Dim Poke As String = File.ReadAllText(TempPath & "/lead.txt")
            If File.Exists(TempPath & "/lead.txt") Then
                File.Delete(TempPath & "/lead.txt")
            End If
            arc &= "E0" & Spot & " 00000088
" & Poke
        End If
            arc &= "D2000000 00000000"
        Return arc
    End Function

    'Greys out PictureBoxes
    Private Sub DrawDE(ByVal pb As PictureBox)
        Try
            pb.Enabled = False
            ControlPaint.DrawImageDisabled(pb.CreateGraphics, pb.BackgroundImage, 0, 0, Color.Gray)
        Catch
        End Try
    End Sub

    'Adds text onto the PictureBoxes
    Private Sub DrawTXT(ByVal txt As String, ByVal pb As PictureBox, ByVal pnt As Point, Optional bg As Boolean = True, Optional fnts As Single = 12)
        Dim myfont As Font = New Font("Calibri", fnts, FontStyle.Regular)
        Dim myBrush As Brush = Brushes.Black
        If txt.Contains("\") Then
            txt = txt.Replace("\", "")
            myfont = New Font("Calibri", fnts, FontStyle.Italic)
        ElseIf txt.Contains("*") Then
            txt = txt.Replace("*", "")
            myfont = New Font("Arial Black", fnts, FontStyle.Bold)
            myBrush = Brushes.White
        End If
        Dim img As Bitmap
        If bg = True Then
            img = New Bitmap(pb.BackgroundImage)
        Else
            img = New Bitmap(pb.Image)
        End If
        Using g = Graphics.FromImage(img)
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.DrawString(txt, myfont, myBrush, New PointF(pnt.X, pnt.Y))
        End Using
        'Dispose the existing image if there is one.'
        If bg = True Then
            pb.BackgroundImage?.Dispose()
            pb.BackgroundImage = img
        Else
            pb.Image?.Dispose()
            pb.Image = img
        End If
    End Sub

    'Calls the text changes on the PicBoxes
    Private Sub PicTXT() Handles LeadList.SelectedIndexChanged
        My.Settings.DLead = LeadList.SelectedIndex
        Dim a(,) As String = {
        {"00 Hardy", "01 Lonely", "02 Brave", "03 Adamant", "04 Naughty", "05 Bold", "06 Docile", "07 Relaxed"},
        {"08 Impish", "09 Lax", "0A Timid", "0B Hasty", "0C Serious", "0D Jolly", "0E Naive", "0F Modest"},
        {"10 Mild", "11 Quiet", "12 Bashful", "13 Rash", "14 Calm", "15 Gentle", "16 Sassy", "17 Careful"},
        {"18 Quirky", "", "", "", "", "", "", ""},
        {"", "", "32 Hardy", "33 Lonely", "34 Brave", "35 Adamant", "36 Naughty", "37 Bold"},
        {"38 Docile", "39 Relaxed", "3A Impish", "3B Lax", "3C Timid", "3D Hasty", "3E Serious", "3F Jolly"},
        {"40 Naive", "41 Modest", "42 Mild", "43 Quiet", "44 Bashful", "45 Rash", "46 Calm", "47 Gentle"},
        {"48 Sassy", "49 Careful", "4A Quirky", "\4B Hardy", "\4C Lonely", "\4D Brave", "\4E Adamant", "\4F Naughty"},
        {"\48 Sassy", "\49 Careful", "\4A Quirky", "4B Hardy", "4C Lonely", "4D Brave", "4E Adamant", "4F Naughty"},
        {"50 Bold", "51 Docile", "52 Relaxed", "53 Impish", "54 Lax", "55 Timid", "56 Hasty", "57 Serious"},
        {"58 Jolly", "59 Naive", "5A Modest", "5B Mild", "5C Quiet", "5D Bashful", "5E Rash", "5F Calm"},
        {"60 Gentle", "61 Sassy", "62 Careful", "63 Quirky", "", "", "", ""},
        {"", "", "", "", "", "", "96 Hardy", "97 Lonely"},
        {"98 Brave", "99 Adamant", "9A Naughty", "9B Bold", "9C Docile", "9D Relaxed", "9E Impish", "9F Lax"},
        {"A0 Timid", "A1 Hasty", "A2 Serious", "A3 Jolly", "A4 Naive", "A5 Modest", "A6 Mild", "A7 Quiet"},
        {"A8 Bashful", "A9 Rash", "AA Calm", "AB Gentle", "AC Sassy", "AD Careful", "AE Quirky", ""},
        {"C8 Hardy", "C9 Lonely", "CA Brave", "CB Adamant", "CC Naughty", "CD Bold", "CE Docile", "CF Relaxed"},
        {"D0 Impish", "D1 Lax", "D2 Timid", "D3 Hasty", "D4 Serious", "D5 Jolly", "D6 Naive", "D7 Modest"},
        {"D8 Mild", "D9 Quiet", "DA Bashful", "DB Rash", "DC Calm", "DD Gentle", "DE Sassy", "DF Careful"},
        {"E0 Quirky", "", "", "", "", "", "", ""}
        }
        pg1.BackgroundImage = My.Resources.sg0
        pg2.BackgroundImage = My.Resources.sg0
        pg3.BackgroundImage = My.Resources.sg0
        pg4.BackgroundImage = My.Resources.sg0
        DrawTXT("*Shiny Group 1", pg1, New Point(21, 4),, 9)
        DrawTXT("*Shiny Group 2", pg2, New Point(21, 4),, 9)
        DrawTXT("*Shiny Group 3", pg3, New Point(21, 4),, 9)
        DrawTXT("*Shiny Group 4", pg4, New Point(21, 4),, 9)
        Dim v As Integer = LeadList.SelectedIndex
        If v = 0 Then
            gender = "\Male\"
        Else
            gender = "\Female\"
        End If
        If v = 0 Or v = 2 Or v = 4 Then
            rR.Text = rR.Text.Replace("1", "4")
            rRQ.Text = rRQ.Text.Replace("1", "4")
        ElseIf v = 1 Or v = 3 Then
            rR.Text = rR.Text.Replace("4", "1")
            rRQ.Text = rRQ.Text.Replace("4", "1")
        End If
        For i = 0 To 7 Step 1
            Dim d As String
            If a(0 + (4 * v), i) <> "" Then
                d = "000000"
            Else
                d = ""
            End If
            DrawTXT(d & a(0 + (4 * v), i), pg1, New Point(4, 26 + (i * 22)))
            If a(1 + (4 * v), i) <> "" Then
                d = "000000"
            Else
                d = ""
            End If
            DrawTXT(d & a(1 + (4 * v), i), pg2, New Point(4, 26 + (i * 22)))
            If a(2 + (4 * v), i) <> "" Then
                d = "000000"
            Else
                d = ""
            End If
            DrawTXT(d & a(2 + (4 * v), i), pg3, New Point(4, 26 + (i * 22)))
            If a(3 + (4 * v), i) <> "" Then
                d = "000000"
            Else
                d = ""
            End If
            DrawTXT(d & a(3 + (4 * v), i), pg4, New Point(4, 26 + (i * 22)))
        Next i
        If rC.Checked = True Then
            rC.PerformClick()
            If sg1.Checked = True Then
                sg4.PerformClick()
                sg1.PerformClick()
            ElseIf sg2.Checked = True Then
                sg4.PerformClick()
                sg2.PerformClick()
            ElseIf sg3.Checked = True Then
                sg4.PerformClick()
                sg3.PerformClick()
            ElseIf sg4.Checked = True Then
                sg1.PerformClick()
                sg4.PerformClick()
            End If
        ElseIf rR.Checked = True Then
            rC.PerformClick()
            rR.PerformClick()
        ElseIf rRQ.Checked = True Then
            rC.PerformClick()
            rRQ.PerformClick()
        End If
        GetLeadList()
    End Sub

    'Checks for empty options
    Private Sub Chks()
        If GameList.SelectedItem = "" Then
            GameList.ForeColor = Color.Red
            Label5.ForeColor = Color.Red
            prob = True
        ElseIf Group = 0 And rC.Checked = True Then
            gSG.ForeColor = Color.Red
            prob = True
        ElseIf rnd(0) = Nothing And rC.Checked = False Then
            gRC.ForeColor = Color.Red
            prob = True
        ElseIf nButt = Nothing Then
            gA.ForeColor = Color.Red
            prob = True
        ElseIf cbLead.Checked = True And (BoxList.SelectedItem = Nothing Or SlotList.SelectedItem = Nothing) Then
            gCCL.ForeColor = Color.Red
            prob = True
        Else
            GameList.ForeColor = DefaultForeColor
            Label5.ForeColor = DefaultForeColor
            gRC.ForeColor = DefaultForeColor
            gSG.ForeColor = DefaultForeColor
            gA.ForeColor = DefaultForeColor
            prob = False
        End If
    End Sub

    'Picks IDs
    Private Sub PickID()
        TSVt = 0
        Select Case LeadList.SelectedIndex
            Case 0
                TSVt = 0
            Case 1
                TSVt = 6
            Case 2
                TSVt = 9
            Case 3
                TSVt = 18
            Case 4
                TSVt = 25
        End Select
        TSVt += Group
        If rC.Checked = True Then
            TSVt -= 1
        End If
        Dim gen As New Random
        If TIDchoose = True Then
            IDs(0) = nTID.Value
        Else
            IDs(0) = gen.Next(0, 65536)
        End If
        Dim Diff As Integer = gen.Next(0, 8) + (TSVt << 3)
        IDs(1) = IDs(0) Xor Diff
        Vchk()
        If prob = True Then
            Exit Sub
        End If
        TID.Text = IDs(0)
        SID.Text = IDs(1)
        TID.Show()
        SID.Show()
    End Sub

    'Verifies IDs
    Private Sub Vchk()
        Dim result As Integer = ((IDs(0) Xor IDs(1)) >> 3)
        If result = TSVt Then
            prob = False
        Else
            prob = True
        End If
    End Sub

    'Executes AR code generation
    Private Sub bGO_Click(sender As Object, e As EventArgs) Handles bGO.Click
        Chks()
        If prob = True Then
            Exit Sub
        End If
        If rnd(0) = True Then
            Dim gen As New Random
            If rnd(1) = False Then
                Group = Math.Floor((gen.Next(0, 30)) / 10)
                If rR.Text.Contains("1") Then
                    Group += 1
                End If
            ElseIf rnd(1) = True Then
                Group = Math.Floor((gen.Next(0, 40)) / 10)
            End If
        End If
        PickID()
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
        AR.Text = GenAR()
        AR.Enabled = True
        My.Settings.DGame = GameList.SelectedIndex
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
            DrawDE(pg2)
            DrawDE(pg3)
            DrawDE(pg4)
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
            DrawDE(pg1)
            DrawDE(pg3)
            DrawDE(pg4)
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
            DrawDE(pg2)
            DrawDE(pg1)
            DrawDE(pg4)
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
            DrawDE(pg2)
            DrawDE(pg3)
            DrawDE(pg1)
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
            DrawDE(pg1)
            DrawDE(pg2)
            DrawDE(pg3)
            DrawDE(pg4)
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
#Region "Button Selection"
    Private Sub PL_Click(sender As Object, e As EventArgs) Handles pL.Click
        If pL.Image Is Nothing Then
            pL.Image = My.Resources.shoulderinner
            DrawTXT("L", pL, New Point(12, 1), False)
            nButt &= "7"
        Else
            pL.Image = Nothing
            nButt = nButt.Replace("7", "")
        End If
    End Sub
    Private Sub PR_Click(sender As Object, e As EventArgs) Handles pR.Click
        If pR.Image Is Nothing Then
            pR.Image = My.Resources.shoulderinner
            DrawTXT("R", pR, New Point(11, 1), False)
            nButt &= "9"
        Else
            pR.Image = Nothing
            nButt = nButt.Replace("9", "")
        End If
    End Sub
    Private Sub PStart_Click(sender As Object, e As EventArgs) Handles pStart.Click
        If pStart.Image Is Nothing Then
            pStart.Image = My.Resources.ovalbuttoninner
            DrawTXT("Start", pStart, New Point(8, 1), False, 9)
            nButt &= "1"
        Else
            pStart.Image = Nothing
            nButt = nButt.Replace("1", "")
        End If
    End Sub
    Private Sub PSel_Click(sender As Object, e As EventArgs) Handles pSel.Click
        If pSel.Image Is Nothing Then
            pSel.Image = My.Resources.ovalbuttoninner
            DrawTXT("Select", pSel, New Point(4, 1), False, 9)
            nButt &= "3"
        Else
            pSel.Image = Nothing
            nButt = nButt.Replace("3", "")
        End If
    End Sub
    Private Sub PA_Click(sender As Object, e As EventArgs) Handles pA.Click
        If pA.Image Is Nothing Then
            pA.Image = My.Resources.buttoninner
            DrawTXT("A", pA, New Point(4, 1), False)
            nButt &= "6"
        Else
            pA.Image = Nothing
            nButt = nButt.Replace("6", "")
        End If
    End Sub
    Private Sub PB_Click(sender As Object, e As EventArgs) Handles pB.Click
        If pB.Image Is Nothing Then
            pB.Image = My.Resources.buttoninner
            DrawTXT("B", pB, New Point(5, 1), False)
            nButt &= "2"
        Else
            pB.Image = Nothing
            nButt = nButt.Replace("2", "")
        End If
    End Sub
    Private Sub PX_Click(sender As Object, e As EventArgs) Handles pX.Click
        If pX.Image Is Nothing Then
            pX.Image = My.Resources.buttoninner
            DrawTXT("X", pX, New Point(5, 1), False)
            nButt &= "8"
        Else
            pX.Image = Nothing
            nButt = nButt.Replace("8", "")
        End If
    End Sub
    Private Sub PY_Click(sender As Object, e As EventArgs) Handles pY.Click
        If pY.Image Is Nothing Then
            pY.Image = My.Resources.buttoninner
            DrawTXT("Y", pY, New Point(6, 1), False)
            nButt &= "4"
        Else
            pY.Image = Nothing
            nButt = nButt.Replace("4", "")
        End If
    End Sub
    Private Sub PUp_Click(sender As Object, e As EventArgs) Handles pUp.Click
        If pUp.Image Is Nothing Then
            dpad(sender, e)
            pUp.Image = My.Resources.dpadinner
            DrawTXT("↑", pUp, New Point(0, 0), False)
            nButt &= "W"
        Else
            pUp.Image = Nothing
            nButt = nButt.Replace("W", "")
        End If
    End Sub
    Private Sub PDown_Click(sender As Object, e As EventArgs) Handles pDown.Click
        If pDown.Image Is Nothing Then
            dpad(sender, e)
            pDown.Image = My.Resources.dpadinner
            pDown.Image.RotateFlip(RotateFlipType.RotateNoneFlipY)
            DrawTXT("↓", pDown, New Point(0, 2), False)
            nButt &= "S"
        Else
            pDown.Image = Nothing
            nButt = nButt.Replace("S", "")
        End If
    End Sub
    Private Sub PRight_Click(sender As Object, e As EventArgs) Handles pRight.Click
        If pRight.Image Is Nothing Then
            dpad(sender, e)
            pRight.Image = My.Resources.dpadinner
            pRight.Image.RotateFlip(RotateFlipType.Rotate90FlipNone)
            DrawTXT("→", pRight, New Point(4, 0), False)
            nButt &= "D"
        Else
            pRight.Image = Nothing
            nButt = nButt.Replace("D", "")
        End If
    End Sub
    Private Sub PLeft_Click(sender As Object, e As EventArgs) Handles pLeft.Click
        If pLeft.Image Is Nothing Then
            dpad(sender, e)
            pLeft.Image = My.Resources.dpadinner
            pLeft.Image.RotateFlip(RotateFlipType.Rotate90FlipX)
            DrawTXT("←", pLeft, New Point(0, 0), False)
            nButt &= "A"
        Else
            pLeft.Image = Nothing
            nButt = nButt.Replace("A", "")
        End If
    End Sub
    Private Sub dpad(sender As Object, e As EventArgs)
        If nButt.Contains("W") Then
            PUp_Click(sender, e)
        ElseIf nButt.Contains("A") Then
            PLeft_Click(sender, e)
        ElseIf nButt.Contains("S") Then
            PDown_Click(sender, e)
        ElseIf nButt.Contains("D") Then
            PRight_Click(sender, e)
        End If
    End Sub
#End Region
#Region "Author Details"
    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If My.Computer.Network.IsAvailable Then
            Process.Start("https://github.com/PlasticJustice")
        Else
            MsgBox("No Internet connection!
You can look me up later.", vbOKOnly, "Error 404")
        End If
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Thread.Sleep(200)
        If My.Computer.Network.IsAvailable Then
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UGSCC5VGSGN3E")
        Else
            MsgBox("No Internet connection!
        I appreciate the gesture.", vbOKOnly, "Error 404")
        End If
    End Sub
    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        PictureBox1.Image = My.Resources.ppdbs
    End Sub
    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        PictureBox1.Image = Nothing
    End Sub
#End Region
#Region "Specific TID & Cute Charm Lead"
    'Sets to Random
    Private Sub RTR_CheckedChanged(sender As Object, e As EventArgs) Handles rTR.CheckedChanged
        nTID.Enabled = False
        TIDchoose = False
    End Sub

    'User's Choice
    Private Sub RTC_CheckedChanged(sender As Object, e As EventArgs) Handles rTC.CheckedChanged
        nTID.Enabled = True
        TIDchoose = True
    End Sub

    Private Sub CbLead_CheckedChanged(sender As Object, e As EventArgs) Handles cbLead.CheckedChanged
        If cbLead.Checked = True Then
            My.Settings.CCPoke = True
            BoxList.Enabled = True
            SlotList.Enabled = True
            cbLead.ForeColor = DefaultForeColor
        ElseIf cbLead.Checked = False Then
            My.Settings.CCPoke = False
            BoxList.Enabled = False
            SlotList.Enabled = False
            cbLead.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub SpotList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles BoxList.SelectedIndexChanged, SlotList.SelectedIndexChanged
        My.Settings.PCspot = (BoxList.SelectedIndex + 1) & "/" & (SlotList.SelectedIndex + 1)
    End Sub

    'Converts EK4 to Action Replay Code compatible data
    Private Sub EK4toAR(ByVal myFile As String)
        Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
        Dim txtTemp As New System.Text.StringBuilder()
        For Each myByte As Byte In myBytes
            txtTemp.Append(myByte.ToString("X2"))
        Next
        Dim EK4 As String = txtTemp.ToString()
        EK4 = EK4.Remove(272, 472 - 272).ToArray()
        Dim AR_EK4 As String = toLE(EK4)
        File.WriteAllText(TempPath & "/lead.txt", AR_EK4)
    End Sub

    'Populates List of Lead Pokemon
    Private Sub GetLeadList()
        ComboBox1.Items.Clear()
        Dim path As String = Local
        If LeadList.SelectedIndex = 0 Then
            path &= "\Male"
        Else
            path &= "\Female"
        End If
        Dim di As New IO.DirectoryInfo(path)
        Dim aryFi As IO.FileInfo() = di.GetFiles("*.ek4")
        Dim fi As IO.FileInfo
        For Each fi In aryFi
            ComboBox1.Items.Add(fi.Name.Replace(".ek4", ""))
        Next
    End Sub

    'Import EK4
    Private Sub Importek4ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Importek4ToolStripMenuItem.Click
        Try
            Dim FileSelect As New OpenFileDialog With {.Filter = "Encrypted PK4 (*.ek4)|*.ek4|All files (*.*)|*.*"}
            Dim res As DialogResult = FileSelect.ShowDialog()
            If res = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            Else
                Dim myFile As String = FileSelect.FileName
                Dim FileP() As String = myFile.Split("\")
                Dim Num As Integer = FileP(UBound(FileP)).Remove(3)
                Dim nl As Integer = FileP(UBound(FileP)).Length
                Dim PID As String = FileP(UBound(FileP)).Remove(nl - 4).ToArray().Skip(nl - 12).ToArray()
                Dim g As Integer = Convert.ToInt32(PID.Skip(6).ToArray(), 16)
                Dim a As Boolean = (Convert.ToString(Convert.ToInt32(PID, 16), 2)).EndsWith("0")
                Dim pName As String
                Dim pGR As Integer = 190
                Debug.Print(PID)
                Debug.Print(Num)
                Select Case Num
                    Case 35
                        pName = "Clefairy"
                    Case 36
                        pName = "Clefable"
                    Case 39
                        pName = "Jigglypuff"
                    Case 40
                        pName = "Wigglytuff"
                    Case 173
                        pName = "Cleffa"
                    Case 174
                        pName = "Igglybuff"
                    Case 300
                        pName = "Skitty"
                    Case 301
                        pName = "Delcatty"
                    Case 428
                        pName = "Lopunny"
                        pGR = 126
                    Case Else
                        MsgBox("File isn't named correctly or isn't a Cute Charm Pokémon", MsgBoxStyle.OkOnly, "Error: Invaild Pokémon")
                        Exit Sub
                End Select
                If a = False Then
                    MsgBox("The Pokémon's ability isn't Cute Charm", MsgBoxStyle.OkOnly, "Error")
                    Exit Sub
                End If
                If g <= pGR Then
                    pName &= " - Female"
                    File.Copy(myFile, Local & "\Female\" & FileP(UBound(FileP)))
                Else
                    pName &= " - Male"
                    File.Copy(myFile, Local & "\Male\" & FileP(UBound(FileP)))
                End If
                MsgBox(pName, MsgBoxStyle.OkOnly)
                GetLeadList()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
#End Region
End Class
