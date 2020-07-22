﻿Imports System.Threading
Imports System.IO
Imports System.Drawing
Public Class Main
#Region "Variables"
    Public Shared ReadOnly apppath As String = My.Application.Info.DirectoryPath 'Path to .exe directory
    Public Shared ReadOnly res As String = Path.GetFullPath(Application.StartupPath & "\..\..\Resources\") 'Path to Project Resources
    Public Shared ReadOnly TempPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp" 'Path to Temp
    Public Shared ReadOnly Local As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Regnum\CuteCharmIDGenie" 'Path to Local
    ReadOnly Natures() As String = {"Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest",
        "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky"} 'List of Natures
    Public mySettings As New IniFile 'Settings.ini File
    Public AR_Code As New AR 'AR Code Class
    Dim doneLoad As Boolean = False

    '*
    Dim gender As String
    Dim rnd(1) As Boolean '{Is Shiny Group random?, With Quirky?}
    Dim TIDchoose As Boolean = False 'Is TID random?
    Dim TSVt As Short = 0 'Target Trainer Shiny Value
    Dim Group As SByte = 0 'Shiny Group Selection
#End Region
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckLocal()
        UpdateCheck()
        rnd(0) = Nothing
        rnd(1) = Nothing
        cb_LeadList.SelectedIndex = My.Settings.Default_Lead
        If My.Settings.Default_Game <> Nothing Then cb_GameList.SelectedIndex = My.Settings.Default_Game
        PicTXT()
    End Sub
    Private Sub Main_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Default_Form()
        If Directory.Exists(Local.Replace("\Regnum", "")) Then LocalMove()
    End Sub
    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        WriteIni()
    End Sub
    Private Sub LocalMove() 'Moves to the new Local folder
        Dim oldLocal As String = Local.Replace("\Regnum", "")
        If Not Directory.Exists(oldLocal) Then Exit Sub
        Dim folders As New List(Of String)({"\Male", "\Female", "\Other"})
        For Each i As String In folders
            Dim di As New IO.DirectoryInfo(oldLocal & i)
            Dim aryFi As IO.FileInfo() = di.GetFiles("*.ek4")
            For Each fi As IO.FileInfo In aryFi
                If Not File.Exists(Local & i & "\" & fi.Name) Then
                    File.Move(oldLocal & i & "\" & fi.Name, Local & i & "\" & fi.Name)
                Else
                    File.Delete(oldLocal & i & "\" & fi.Name)
                End If
            Next
            Directory.Delete(oldLocal & i)
        Next
        Directory.Delete(oldLocal, True)
    End Sub
#Region "Esentials"
    'Checks For Update
    Private Sub UpdateCheck()
        If File.Exists(TempPath & "\vsn.txt") Then File.Delete(TempPath & "\vsn.txt")
        If File.Exists(TempPath & "\dt.txt") Then File.Delete(TempPath & "\dt.txt")
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
            If Application.ProductVersion <> v Then File.WriteAllText(res & "/date.txt", (System.DateTime.Today.Year & "/" & System.DateTime.Today.Month & "/" & System.DateTime.Today.Day))
        End If
        lklb_Update.Hide()
        MenuStrip1.Location = New Point(0, 0)
#Else
        File.WriteAllText(TempPath & "\date.txt", My.Resources._date)
        Dim dat As String = File.ReadAllText(TempPath & "\date.txt")
        Me.Text = "Cute Charm Glitch ID Generator (" & dat & ")"
        If My.Computer.Network.IsAvailable Then
            Try
                My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/PlasticJustice/CuteCharmIDGenie/master/CuteCharmIDGen/Resources/date.txt", TempPath & "\dt.txt")
            Catch
                File.WriteAllText(TempPath & "\dt.txt", " ")
            End Try
            Dim Reader As New IO.StreamReader(TempPath & "\dt.txt")
            Dim dtt As String = Reader.ReadToEnd
            Reader.Close()
            File.Delete(TempPath & "\dt.txt")
            If dat <> dtt Then
                lklb_Update.Text = "New Update Available! " & dtt
                MenuStrip1.Location = New Point(175, 0)
                lklb_Update.Show()
            Else
                lklb_Update.Hide()
                MenuStrip1.Location = New Point(0, 0)
            End If
        Else
            lklb_Update.Hide()
        End If
        File.Delete(TempPath & "\date.txt")
#End If
    End Sub
    Private Sub Lklb_Update_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lklb_Update.LinkClicked
        If My.Computer.Network.IsAvailable Then
            Process.Start("https://github.com/PlasticJustice/CuteCharmIDGenie/releases/latest")
        Else
            MsgBox("No Internet connection!
You can not update at the moment.", vbOKOnly, "Error 404")
        End If
    End Sub
    Private Sub Lklb_Author_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lklb_Author.LinkClicked
        If My.Computer.Network.IsAvailable Then
            Process.Start("https://github.com/PlasticJustice")
        Else
            MsgBox("No Internet connection!
You can look me up later.", vbOKOnly, "Error 404")
        End If
    End Sub
    Private Sub Pb_Donate_Click(sender As Object, e As EventArgs) Handles pb_Donate.Click, ToolStripMenuItem2.Click
        Thread.Sleep(200)
        If My.Computer.Network.IsAvailable Then
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UGSCC5VGSGN3E")
        Else
            MsgBox("No Internet connection!
        I appreciate the gesture.", vbOKOnly, "Error 404")
        End If
    End Sub
    Private Sub Pb_Donate_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Donate.MouseDown
        pb_Donate.Image = My.Resources.ppdbs
    End Sub
    Private Sub Pb_Donate_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Donate.MouseUp
        pb_Donate.Image = Nothing
    End Sub
#End Region
#Region "Startup"
    Private Sub CreateFolders(ByVal dirs As String())
        Try
            For i = 0 To UBound(dirs) Step 1
                If dirs(i).Contains(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then
                Else
                    dirs(i) = Local & dirs(i)
                End If
                Do While Not Directory.Exists(dirs(i))
                    If Not Directory.Exists(dirs(i)) Then Directory.CreateDirectory(dirs(i))
                Loop
            Next i
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub CreateFiles(ByVal files(,))
        Try
            For i = 0 To UBound(files) Step 1
                If files(i, 0).Contains(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then
                Else
                    files(i, 0) = Local & files(i, 0)
                End If
                Do While Not File.Exists(files(i, 0))
                    If Not File.Exists(files(i, 0)) Then
                        If TypeOf files(i, 1) Is String Then
                            File.WriteAllText(files(i, 0), files(i, 1))
                        Else
                            File.WriteAllBytes(files(i, 0), files(i, 1))
                        End If
                    End If
                Loop
            Next i
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'Checks Local Folders
    Private Sub CheckLocal()
        Dim locals As String() = {Local, Local & "\Male", Local & "\Female", Local & "\Other"}
        CreateFolders(locals)
        CreateFiles({{Local & "\Male\174 - MAGIC - D8D7F7D437DE.ek4", My.Resources.MaleLead}, {Local & "\Female\174 - MAGIC - D8D95E400116.ek4", My.Resources.FemaleLead}})

        If Not File.Exists(Local & "\settings.ini") Then
            File.WriteAllText(Local & "\settings.ini", "")
            WriteIni()
        End If
        ReadIni()
    End Sub

    'Setup Activation Buttons Design
    Private Sub ActDraw() '*
        pb_Down.BackgroundImage.RotateFlip(RotateFlipType.RotateNoneFlipY)
        pb_Left.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipX)
        pb_Left.Size = New Size(24, 20)
        pb_Right.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
        pb_Right.Size = New Size(24, 20)
        DrawTXT("A", pb_A, New Point(4, 1),, 12)
        DrawTXT("B", pb_B, New Point(5, 1),, 12)
        DrawTXT("X", pb_X, New Point(5, 1),, 12)
        DrawTXT("Y", pb_Y, New Point(6, 1),, 12)
        DrawTXT("Select", pb_Select, New Point(4, 1), True, 9)
        DrawTXT("Start", pb_Start, New Point(8, 1), True, 9)
        DrawTXT("L", pb_L, New Point(12, 1),, 12)
        DrawTXT("R", pb_R, New Point(11, 1),, 12)
        DrawTXT("↑", pb_Up, New Point(0, 0),, 12)
        DrawTXT("↓", pb_Down, New Point(0, 2),, 12)
        DrawTXT("→", pb_Right, New Point(4, 0),, 12)
        DrawTXT("←", pb_Left, New Point(0, 0),, 12)
    End Sub

    'Adds text onto the PictureBoxes
    Private Sub DrawTXT(ByVal txt As String, ByVal pb As PictureBox, ByVal pnt As Point, Optional bg As Boolean = True, Optional fnts As Single = 9) '*
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
        'Dispose the existing image if there is one.
        If bg = True Then
            pb.BackgroundImage?.Dispose()
            pb.BackgroundImage = img
        Else
            pb.Image?.Dispose()
            pb.Image = img
        End If
    End Sub

    'Greys out PictureBoxes
    Private Sub DrawDE(pb As PictureBox)
        Try
            pb.Enabled = False
            If pb IsNot Nothing Then ControlPaint.DrawImageDisabled(pb.CreateGraphics, pb.BackgroundImage, 0, 0, Color.Gray)
        Catch
        End Try
    End Sub

    'Calls the text changes on the PicBoxes
    Private Sub PicTXT() Handles cb_LeadList.SelectedIndexChanged
        My.Settings.Default_Lead = cb_LeadList.SelectedIndex
        Dim index As Byte = 1
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup3, pb_ShinyGroup4})
        For Each i As PictureBox In table
            i.BackgroundImage = My.Resources.sg
            DrawTXT("*Shiny Group " & index, i, New Point(11, 2),, 8)
            index += 1
        Next
        Select Case cb_LeadList.SelectedIndex
            Case 0
                gender = "\Male\"
                rb_RandomFixed.Text = rb_RandomFixed.Text.Replace("1", "4")
                rb_RandomPure.Text = rb_RandomPure.Text.Replace("1", "4")
            Case Else
                Select Case cb_LeadList.SelectedIndex
                    Case 2, 4
                        rb_RandomFixed.Text = rb_RandomFixed.Text.Replace("1", "4")
                        rb_RandomPure.Text = rb_RandomPure.Text.Replace("1", "4")
                    Case 1, 3
                        rb_RandomFixed.Text = rb_RandomFixed.Text.Replace("4", "1")
                        rb_RandomPure.Text = rb_RandomPure.Text.Replace("4", "1")
                End Select
                gender = "\Female\"
        End Select
        index = 0
        Dim buffers(,) As Byte = {{0, 7, 0}, {2, 0, &H30}, {0, 4, &H48}, {6, 1, &H90}, {0, 7, &HC8}}
        For Each i As PictureBox In table
            For n = buffers(cb_LeadList.SelectedIndex, 2) + (index * 8) + If(index = 0, buffers(cb_LeadList.SelectedIndex, 0), 0) To buffers(cb_LeadList.SelectedIndex, 2) + (index * 8) + (7 - If(index = 3, buffers(cb_LeadList.SelectedIndex, 1), 0))
                Dim y = n Mod (buffers(cb_LeadList.SelectedIndex, 2) + (index * 8) + If(index = 0, If(cb_LeadList.SelectedIndex = 0, 8, 0), 0))
                Dim x = n Mod (buffers(cb_LeadList.SelectedIndex, 0) + buffers(cb_LeadList.SelectedIndex, 2) + If(cb_LeadList.SelectedIndex = 0, 25, 0))
                If cb_LeadList.SelectedIndex = 1 And index = 3 And (y > 2) Then
                    DrawTXT("\000000" & Hex(n) & " " & Natures(x Mod 25), i, New Point(3, 22 + (y * 17.5)))
                ElseIf cb_LeadList.SelectedIndex = 2 And index = 0 And (y < 3) Then
                    DrawTXT("\000000" & Hex(n) & " " & Natures(x + 22), i, New Point(3, 22 + (y * 17.5)))
                Else
                    DrawTXT("000000" & Hex(n) & " " & Natures(x - If(cb_LeadList.SelectedIndex = 2, 3, 0)), i, New Point(3, 22 + (y * 17.5)))
                End If
            Next
            index += 1
        Next
        '*
        If rb_Choose.Checked = True Then
            rb_Choose.PerformClick()
            If rb_ShinyGroup1.Checked = True Then
                rb_ShinyGroup4.PerformClick()
                rb_ShinyGroup1.PerformClick()
            ElseIf rb_ShinyGroup2.Checked = True Then
                rb_ShinyGroup4.PerformClick()
                rb_ShinyGroup2.PerformClick()
            ElseIf rb_ShinyGroup3.Checked = True Then
                rb_ShinyGroup4.PerformClick()
                rb_ShinyGroup3.PerformClick()
            ElseIf rb_ShinyGroup4.Checked = True Then
                rb_ShinyGroup1.PerformClick()
                rb_ShinyGroup4.PerformClick()
            End If
        ElseIf rb_RandomFixed.Checked = True Then
            rb_Choose.PerformClick()
            rb_RandomFixed.PerformClick()
        ElseIf rb_RandomPure.Checked = True Then
            rb_Choose.PerformClick()
            rb_RandomPure.PerformClick()
        End If
        GetLeadList()
    End Sub
    Private Sub Default_Form() '*
        rb_ShinyGroup1.Checked = False
        gb_ShinyGroups.Enabled = False
        rb_RandomFixed.PerformClick()
        rb_RandomTID.PerformClick()
        rtb_ARCodeOutput.Enabled = False

        ActDraw()

        For i = 1 To 18 Step 1
            cb_BoxList.Items.Add("Box " & i)
        Next i
        For i = 1 To 30 Step 1
            cb_SlotList.Items.Add("Slot " & i)
        Next i
        Default_Values()

        If My.Settings.CuteCharmLead = True Then
            cx_Lead.Checked = True
            Cx_Lead_CheckedChanged(0, New EventArgs)
        ElseIf My.Settings.CuteCharmLead = False Then
            cx_Lead.Checked = False
            Cx_Lead_CheckedChanged(0, New EventArgs)
        End If
        doneLoad = True
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup3, pb_ShinyGroup4})
        ListDE(table)
    End Sub
    Private Sub Default_Values()
        Dim defaultSlot() As String = My.Settings.BoxLocation.Split("/")
        cb_BoxList.SelectedIndex = defaultSlot(LBound(defaultSlot)) - 1
        cb_SlotList.SelectedIndex = defaultSlot(UBound(defaultSlot)) - 1

        AR_Code.ActivateButtons = My.Settings.AR_Activation
        Dim btns As New List(Of PictureBox)({pb_Y, pb_X, pb_L, pb_R, pb_Down, pb_Up, pb_Right, pb_Left, pb_Start, pb_Select, pb_B, pb_A})
        For Each i As PictureBox In btns
            ClickButton(i)
        Next

        cb_GameList.SelectedIndex = My.Settings.Default_Game
        cb_LeadList.SelectedIndex = My.Settings.Default_Lead
    End Sub
    Private Sub ClickButton(btn As PictureBox) ', text As String, x As Byte, y As Byte, fill_Img As Bitmap, btn_Bool As Boolean)
        Dim text As String = Nothing
        Dim xy(1) As Byte
        Dim fill_Img As Bitmap = Nothing
        Dim Btn_Bool As Boolean
        Dim Size As Byte = 12
        If btn Is pb_Y Then
            text = "Y"
            xy = {6, 1}
            fill_Img = My.Resources.buttoninner
            Btn_Bool = AR_Code.Button_Y
        ElseIf btn Is pb_X Then
            text = "X"
            xy = {5, 1}
            fill_Img = My.Resources.buttoninner
            Btn_Bool = AR_Code.Button_X
        ElseIf btn Is pb_L Then
            text = "L"
            xy = {12, 1}
            fill_Img = My.Resources.shoulderinner
            Btn_Bool = AR_Code.Button_L
        ElseIf btn Is pb_R Then
            text = "R"
            xy = {11, 1}
            fill_Img = My.Resources.shoulderinner
            Btn_Bool = AR_Code.Button_R
        ElseIf btn Is pb_Down Then
            text = "↓"
            xy = {0, 2}
            fill_Img = My.Resources.dpadinner
            fill_Img.RotateFlip(RotateFlipType.RotateNoneFlipY)
            Btn_Bool = AR_Code.Button_Down
        ElseIf btn Is pb_Up Then
            text = "↑"
            xy = {0, 0}
            fill_Img = My.Resources.dpadinner
            Btn_Bool = AR_Code.Button_Up
        ElseIf btn Is pb_Right Then
            text = "→"
            xy = {4, 0}
            fill_Img = My.Resources.dpadinner
            fill_Img.RotateFlip(RotateFlipType.Rotate90FlipNone)
            Btn_Bool = AR_Code.Button_Right
        ElseIf btn Is pb_Left Then
            text = "←"
            xy = {0, 0}
            fill_Img = My.Resources.dpadinner
            fill_Img.RotateFlip(RotateFlipType.Rotate90FlipX)
            Btn_Bool = AR_Code.Button_Left
        ElseIf btn Is pb_Start Then
            text = "Start"
            xy = {8, 1}
            Size = 9
            fill_Img = My.Resources.ovalbuttoninner
            Btn_Bool = AR_Code.Button_Start
        ElseIf btn Is pb_Select Then
            text = "Select"
            xy = {4, 1}
            Size = 9
            fill_Img = My.Resources.ovalbuttoninner
            Btn_Bool = AR_Code.Button_Select
        ElseIf btn Is pb_B Then
            text = "B"
            xy = {5, 1}
            fill_Img = My.Resources.buttoninner
            Btn_Bool = AR_Code.Button_B
        ElseIf btn Is pb_A Then
            text = "A"
            xy = {4, 1}
            fill_Img = My.Resources.buttoninner
            Btn_Bool = AR_Code.Button_A
        End If
        My.Settings.AR_Activation = AR_Code.ActivateButtons
        If Btn_Bool = True Then
            btn.Image = fill_Img
            DrawTXT(text, btn, New Point(xy(0), xy(1)), False, Size)
        Else
            btn.Image = Nothing
        End If
    End Sub
    'Populates List of Lead Pokemon
    Private Sub GetLeadList() '*
        cb_EK4List.Items.Clear()
        Dim path As String = Local
        If cb_LeadList.SelectedIndex = 0 Then
            path &= "\Male"
        Else
            path &= "\Female"
        End If
        Dim di As New IO.DirectoryInfo(path)
        Dim aryFi As IO.FileInfo() = di.GetFiles("*.ek4")
        For Each fi As IO.FileInfo In aryFi
            cb_EK4List.Items.Add(fi.Name.Replace(".ek4", ""))
        Next
        Dim di2 As New IO.DirectoryInfo(Local & "\Other")
        aryFi = di2.GetFiles("*.ek4")
        For Each fi As IO.FileInfo In aryFi
            cb_EK4List.Items.Add("『" & fi.Name.Replace(".ek4", "") & "』")
        Next
    End Sub
#End Region
#Region "Menu"
    'Opens Local Folder where Leads are stored
    Private Sub Bt_Storage_Click(sender As Object, e As EventArgs) Handles bt_Storage.Click
        Process.Start(Local)
    End Sub

    'Donate within the Options tab
    Private Sub ToolStripMenuItem2_MouseHover(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.MouseHover
        ToolStripMenuItem2.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
    End Sub
    Private Sub ToolStripMenuItem2_MouseLeave(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.MouseLeave
        ToolStripMenuItem2.DisplayStyle = ToolStripItemDisplayStyle.Image
    End Sub
#End Region
#Region "AR Code"
    'Generates AR Code
    Private Function GenAR()
        Select Case cb_GameList.SelectedIndex
            Case 0 'DP
                AR_Code.Pointer = AR.Pointer_DP
                AR_Code.ID_Location = AR.ID_Location_DP
                AR_Code.Box_Location = AR.Box_Location_DP + (cb_BoxList.SelectedIndex * AR.Box_Buffer_DPPt) + (cb_SlotList.SelectedIndex * AR.EK4_Length)
                lb_ARGame.Text = "DP"
            Case 1 'Pt
                AR_Code.Pointer = AR.Pointer_Pt
                AR_Code.ID_Location = AR.ID_Location_Pt
                AR_Code.Box_Location = AR.Box_Location_Pt + (cb_BoxList.SelectedIndex * AR.Box_Buffer_DPPt) + (cb_SlotList.SelectedIndex * AR.EK4_Length)
                lb_ARGame.Text = "Pt"
            Case 2 'HGSS
                AR_Code.Pointer = AR.Pointer_HGSS
                AR_Code.ID_Location = AR.ID_Location_HGSS
                AR_Code.Box_Location = AR.Box_Location_HGSS + (cb_BoxList.SelectedIndex * AR.Box_Buffer_HGSS) + (cb_SlotList.SelectedIndex * AR.EK4_Length)
                lb_ARGame.Text = "HGSS"
        End Select
        If My.Settings.CuteCharmLead = True Then
            If cb_EK4List.Text.Contains("『") Or cb_EK4List.Text.Contains("』") Then
                EK4toAR(Local & "\Other\" & cb_EK4List.Text.Replace("『", "").Replace("』", "") & ".ek4")
            Else
                EK4toAR(Local & gender & cb_EK4List.Text & ".ek4")
            End If
        End If
        Return AR_Code.Build(My.Settings.CuteCharmLead)
    End Function

    'Converts EK4 to Action Replay Code compatible data
    Private Sub EK4toAR(ByVal myFile As String) '*
        Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
        Dim EK4 As String = ByteArrayToHexString(myBytes)
        EK4 = EK4.Remove(272, 472 - 272).ToArray()
        AR_Code.Lead_EK4 = HexStringToByteArray(ToLE(EK4))
    End Sub

    Private Function SpacingAR(hexstring As String)
        Dim newline As Boolean = False
        Dim tempStr As String = Nothing
        For i = 8 To hexstring.Length Step 8
            If newline Then
                tempStr &= hexstring.Skip(i - 8).Take(8).ToArray() & "
"
            Else
                tempStr &= hexstring.Skip(i - 8).Take(8).ToArray() & " "
            End If
            newline = Not newline
        Next
        Return tempStr
    End Function

    'Import EK4
    Private Sub Importek4ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Importek4ToolStripMenuItem.Click, bt_Import.Click '*
        Try
            Dim FileSelect As New OpenFileDialog With {.Filter = "Encrypted PK4 (*.ek4)|*.ek4|All files (*.*)|*.*"}
            Dim res As DialogResult = FileSelect.ShowDialog()
            If res = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            Else
                Dim m As Boolean() = {False, False, False, False} '{Message?, Not CC?, Not CC PKM?, Gender?}
                Dim myFile As String = FileSelect.FileName
                Dim FileP() As String = myFile.Split("\")
                Dim Num As Integer
                Dim g As Integer
                Dim a As Boolean
                Try
                    Num = FileP(UBound(FileP)).Remove(3) 'Dex Number
                Catch ex As Exception
                    Num = 0
                    m(0) = True
                    m(2) = True
                End Try
                Try
                    Dim nl As Integer = FileP(UBound(FileP)).Length
                    Dim PID As String = FileP(UBound(FileP)).Remove(nl - 4).ToArray().Skip(nl - 12).ToArray()
                    g = Convert.ToInt32(PID.Skip(6).ToArray(), 16) 'Gender
                    a = (Convert.ToString(Convert.ToInt32(PID, 16), 2)).EndsWith("0") 'Ability
                Catch ex As Exception
                    g = -1
                    a = False
                    m(0) = True
                    m(1) = True
                    m(3) = True
                End Try
                Dim pName As String = Nothing
                Dim pGR As Integer = 190

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
                        m(0) = True
                        m(2) = True
                End Select
                If a = False Then
                    m(0) = True
                    m(1) = True
                End If
                If g = -1 Then
                    m(0) = True
                    m(3) = True
                    If m(0) = True Then
                        Dim message As String = "Couldn't determine "
                        If m(2) = True Then message += "Species, "
                        If m(3) = True Then message += "Gender, "
                        If m(1) = True Then message += "Ability. "
                        message += "Do you still want to import this Pokémon?"
                        Dim ans = MsgB(message, 2, "Yes", "No",, "Error")
                        Select Case ans
                            Case 6
                                File.Copy(myFile, Local & "\Other\" & FileP(UBound(FileP)))
                                MsgB("Pokémon was put in the 'Other' folder.")
                            Case 7
                                Exit Sub
                        End Select
                    End If
                Else
                    If g <= pGR Then
                        pName &= " - Female"
                        File.Copy(myFile, Local & "\Female\" & FileP(UBound(FileP)))
                    Else
                        pName &= " - Male"
                        File.Copy(myFile, Local & "\Male\" & FileP(UBound(FileP)))
                    End If
                    MsgB(pName)
                End If
                GetLeadList()
            End If
        Catch ex As Exception
            MsgB(ex.Message)
        End Try
    End Sub
#End Region
#Region "Pick ID"
    'Picks IDs
    Private Sub PickID() '*
        TSVt = 0
        Select Case cb_LeadList.SelectedIndex
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
        If rb_Choose.Checked = True Then TSVt -= 1
        Dim gen As New Random
        If TIDchoose = True Then
            AR_Code.TrainerID = nud_TrainerID.Value
        Else
            AR_Code.TrainerID = gen.Next(0, 65536)
        End If
        Dim Diff As Integer = gen.Next(0, 8) + (TSVt << 3)
        AR_Code.SecretID = AR_Code.TrainerID Xor Diff
        If ValueCheck() Then Exit Sub
        lb_TIDValue.Text = AR_Code.TrainerID
        lb_SIDValue.Text = AR_Code.SecretID
        lb_TIDValue.Show()
        lb_SIDValue.Show()
    End Sub

    'Verifies IDs
    Private Function ValueCheck()
        Dim result As Integer = ((AR_Code.TrainerID Xor AR_Code.SecretID) >> 3)
        If result = TSVt Then
            Return False
        Else
            Return True
        End If
    End Function

    'Sets to Random
    Private Sub Rb_RandomTID_CheckedChanged(sender As Object, e As EventArgs) Handles rb_RandomTID.CheckedChanged
        nud_TrainerID.Enabled = False
        TIDchoose = False
    End Sub

    'User's Choice
    Private Sub Rb_ChooseTID_CheckedChanged(sender As Object, e As EventArgs) Handles rb_ChooseTID.CheckedChanged
        nud_TrainerID.Enabled = True
        TIDchoose = True
    End Sub
#End Region
#Region "Controls"
#Region "Shiny Group Selection"
    'User Chooses Shiny Group
    Private Sub Rb_Choose_CheckedChanged(sender As Object, e As EventArgs) Handles rb_Choose.CheckedChanged
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup3, pb_ShinyGroup4})
        If rb_Choose.Checked = True Then
            rnd(0) = False
            gb_ShinyGroups.Enabled = True
            ListEnable(table)
        Else
            rnd(0) = Nothing
            gb_ShinyGroups.Enabled = False
            ListDE(table)
        End If
    End Sub

    'Shiny Group is random without Group 4
    Private Sub Rb_RandomFixed_CheckedChanged(sender As Object, e As EventArgs) Handles rb_RandomFixed.CheckedChanged
        If rb_RandomFixed.Checked = True Then
            rnd(0) = True
            rnd(1) = False
        Else
            rnd(0) = Nothing
            rnd(1) = Nothing
        End If
    End Sub

    'Shiny Group is random with Group 4
    Private Sub Rb_RandomPure_CheckedChanged(sender As Object, e As EventArgs) Handles rb_RandomPure.CheckedChanged
        If rb_RandomPure.Checked = True Then
            rnd(0) = True
            rnd(1) = True
        Else
            rnd(0) = Nothing
            rnd(1) = Nothing
        End If
    End Sub
#End Region
#Region "Specific Shiny Group RadioButtons"
    Private Sub ListDE(table As List(Of PictureBox))
        For Each i As PictureBox In table
            If doneLoad Then DrawDE(i)
        Next
    End Sub
    Private Sub ListEnable(table As List(Of PictureBox))
        For Each i As PictureBox In table
            i.Enabled = True
        Next
    End Sub
    'Shiny Group 1
    Private Sub Rb_ShinyGroup1_CheckedChanged(sender As Object, e As EventArgs) Handles pb_ShinyGroup1.Click, rb_ShinyGroup1.CheckedChanged
        Dim table As New List(Of PictureBox)({pb_ShinyGroup2, pb_ShinyGroup3, pb_ShinyGroup4})
        If rb_ShinyGroup1.Checked = True Then
            Group = 1
            ListDE(table)
        Else
            Group = 0
            ListEnable(table)
        End If
    End Sub

    'Shiny Group 2
    Private Sub Rb_ShinyGroup2_CheckedChanged(sender As Object, e As EventArgs) Handles pb_ShinyGroup2.Click, rb_ShinyGroup2.CheckedChanged
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup3, pb_ShinyGroup4})
        If rb_ShinyGroup2.Checked = True Then
            Group = 2
            ListDE(table)
        Else
            Group = 0
            ListEnable(table)
        End If
    End Sub

    'Shiny Group 3
    Private Sub Rb_ShinyGroup3_CheckedChanged(sender As Object, e As EventArgs) Handles pb_ShinyGroup3.Click, rb_ShinyGroup3.CheckedChanged
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup4})
        If rb_ShinyGroup3.Checked = True Then
            Group = 3
            ListDE(table)
        Else
            Group = 0
            ListEnable(table)
        End If
    End Sub

    'Shiny Group 4
    Private Sub Rb_ShinyGroup4_CheckedChanged(sender As Object, e As EventArgs) Handles pb_ShinyGroup4.Click, rb_ShinyGroup4.CheckedChanged
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup3})
        If rb_ShinyGroup4.Checked = True Then
            Group = 4
            ListDE(table)
        Else
            Group = 0
            ListEnable(table)
        End If
    End Sub
#End Region
#Region "Button Selection"
    Private Sub Pb_Y_Click(sender As Object, e As EventArgs) Handles pb_Y.Click
        AR_Code.Button_Y = Not AR_Code.Button_Y
        ClickButton(pb_Y)
    End Sub
    Private Sub Pb_X_Click(sender As Object, e As EventArgs) Handles pb_X.Click
        AR_Code.Button_X = Not AR_Code.Button_X
        ClickButton(pb_X)
    End Sub
    Private Sub Pb_L_Click(sender As Object, e As EventArgs) Handles pb_L.Click
        AR_Code.Button_L = Not AR_Code.Button_L
        ClickButton(pb_L)
    End Sub
    Private Sub Pb_R_Click(sender As Object, e As EventArgs) Handles pb_R.Click
        AR_Code.Button_R = Not AR_Code.Button_R
        ClickButton(pb_R)
    End Sub
    Private Sub Pb_Down_Click(sender As Object, e As EventArgs) Handles pb_Down.Click
        If AR_Code.Button_Down = False Then Dpad(sender, e)
        AR_Code.Button_Down = Not AR_Code.Button_Down
        ClickButton(pb_Down)
    End Sub
    Private Sub Pb_Up_Click(sender As Object, e As EventArgs) Handles pb_Up.Click
        If AR_Code.Button_Up = False Then Dpad(sender, e)
        AR_Code.Button_Up = Not AR_Code.Button_Up
        ClickButton(pb_Up)
    End Sub
    Private Sub Pb_Right_Click(sender As Object, e As EventArgs) Handles pb_Right.Click
        If AR_Code.Button_Right = False Then Dpad(sender, e)
        AR_Code.Button_Right = Not AR_Code.Button_Right
        ClickButton(pb_Right)
    End Sub
    Private Sub Pb_Left_Click(sender As Object, e As EventArgs) Handles pb_Left.Click
        If AR_Code.Button_Left = False Then Dpad(sender, e)
        AR_Code.Button_Left = Not AR_Code.Button_Left
        ClickButton(pb_Left)
    End Sub
    Private Sub Pb_Start_Click(sender As Object, e As EventArgs) Handles pb_Start.Click
        AR_Code.Button_Start = Not AR_Code.Button_Start
        ClickButton(pb_Start)
    End Sub
    Private Sub Pb_Sel_Click(sender As Object, e As EventArgs) Handles pb_Select.Click
        AR_Code.Button_Select = Not AR_Code.Button_Select
        ClickButton(pb_Select)
    End Sub
    Private Sub Pb_B_Click(sender As Object, e As EventArgs) Handles pb_B.Click
        AR_Code.Button_B = Not AR_Code.Button_B
        ClickButton(pb_B)
    End Sub
    Private Sub Pb_A_Click(sender As Object, e As EventArgs) Handles pb_A.Click
        AR_Code.Button_A = Not AR_Code.Button_A
        ClickButton(pb_A)
    End Sub

    Private Sub Dpad(sender As Object, e As EventArgs)
        If AR_Code.Button_Down Then
            Pb_Down_Click(sender, e)
        ElseIf AR_Code.Button_Up Then
            Pb_Up_Click(sender, e)
        ElseIf AR_Code.Button_Right Then
            Pb_Right_Click(sender, e)
        ElseIf AR_Code.Button_Left Then
            Pb_Left_Click(sender, e)
        End If
    End Sub
#End Region
#Region "Lead"
    'Is Lead Enabled?
    Private Sub Cx_Lead_CheckedChanged(sender As Object, e As EventArgs) Handles cx_Lead.CheckedChanged
        If cx_Lead.Checked = True Then
            cx_Lead.Text = "Enabled"
            My.Settings.CuteCharmLead = True
            cb_BoxList.Enabled = True
            cb_SlotList.Enabled = True
            gb_PC.Enabled = True
            cb_EK4List.Enabled = True
            lb_CuteCharmLead.Enabled = True
            bt_Import.Enabled = True
            bt_Storage.Enabled = True
            gb_Storage.Enabled = True
            cx_Lead.ForeColor = DefaultForeColor
        ElseIf cx_Lead.Checked = False Then
            cx_Lead.Text = "Disabled"
            My.Settings.CuteCharmLead = False
            cb_BoxList.Enabled = False
            cb_SlotList.Enabled = False
            gb_PC.Enabled = False
            cb_EK4List.Enabled = False
            lb_CuteCharmLead.Enabled = False
            bt_Import.Enabled = False
            bt_Storage.Enabled = False
            gb_Storage.Enabled = False
            cx_Lead.ForeColor = Color.Gray
        End If
    End Sub

    'PC Slot
    Private Sub Cb_SpotList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_BoxList.SelectedIndexChanged, cb_SlotList.SelectedIndexChanged
        My.Settings.BoxLocation = (cb_BoxList.SelectedIndex + 1) & "/" & (cb_SlotList.SelectedIndex + 1)
    End Sub
#End Region

    'Copy AR Code to clipboard
    Private Sub Rtb_ARCodeOutput_Click(sender As Object, e As EventArgs) Handles rtb_ARCodeOutput.Click, rtb_ARCodeOutput.MouseClick, rtb_ARCodeOutput.DoubleClick, rtb_ARCodeOutput.MouseDoubleClick
        Clipboard.Clear()
        Clipboard.SetText(rtb_ARCodeOutput.Text, TextDataFormat.Text)
    End Sub

    'Checks for empty options
    Private Function Checks()
        If cb_GameList.SelectedItem = "" Then
            cb_GameList.ForeColor = Color.Red
            lb_Game.ForeColor = Color.Red
            Return True
        ElseIf Group = 0 And rb_Choose.Checked = True Then
            gb_ShinyGroups.ForeColor = Color.Red
            Return True
        ElseIf rnd(0) = Nothing And rb_Choose.Checked = False Then
            gb_RandomChoice.ForeColor = Color.Red
            Return True
        ElseIf AR_Code.ActivateButtons = 0 Then
            gb_ARButtons.ForeColor = Color.Red
            Return True
        ElseIf cx_Lead.Checked = True And (cb_BoxList.SelectedItem = Nothing Or cb_SlotList.SelectedItem = Nothing) Then
            gb_PC.ForeColor = Color.Red
            Return True
        ElseIf cx_Lead.Checked = True And (cb_EK4List.SelectedItem = Nothing) Then
            lb_CuteCharmLead.ForeColor = Color.Red
            Return True
        Else
            cb_GameList.ForeColor = DefaultForeColor
            lb_Game.ForeColor = DefaultForeColor
            gb_RandomChoice.ForeColor = DefaultForeColor
            gb_ShinyGroups.ForeColor = DefaultForeColor
            gb_ARButtons.ForeColor = DefaultForeColor
            gb_PC.ForeColor = DefaultForeColor
            lb_CuteCharmLead.ForeColor = DefaultForeColor
            Return False
        End If
    End Function

    'Executes AR code generation
    Private Sub Bt_Calculate_Click(sender As Object, e As EventArgs) Handles bt_Calculate.Click
        If Checks() Then Exit Sub
        If rnd(0) = True Then
            Dim gen As New Random
            If rnd(1) = False Then
                Group = Math.Floor((gen.Next(0, 30)) / 10)
                If rb_RandomFixed.Text.Contains("1") Then Group += 1
            ElseIf rnd(1) = True Then
                Group = Math.Floor((gen.Next(0, 40)) / 10)
            End If
        End If
        PickID()
        Thread.Sleep(300)
        rtb_ARCodeOutput.Text = SpacingAR(ByteArrayToHexString(GenAR()))
        rtb_ARCodeOutput.Enabled = True
        My.Settings.Default_Game = cb_GameList.SelectedIndex
    End Sub
#End Region
End Class
