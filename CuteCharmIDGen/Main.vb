﻿Imports System.Threading

Public Class Main
#Region "Variables"
    Public Shared ReadOnly apppath As String = My.Application.Info.DirectoryPath 'Path to .exe directory
    Public Shared ReadOnly res As String = Path.GetFullPath(Application.StartupPath & "\..\..\Resources\") 'Path to Project Resources
    Public Shared ReadOnly TempPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp" 'Path to Temp
    Public Shared ReadOnly Local As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\RenegadeRaven\CuteCharmIDGenie" 'Path to Local
    Private Natures() As String = {"Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest",
        "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky"} 'List of Natures
    Public AR_Code As New AR 'AR Code Class
    Public Shared LangRes As Resources.ResourceManager
    Dim doneLoad As Boolean = False 'Is it done loading?

    '* means potential future improvement
    Dim TSVt As Short = 0 'Target Trainer Shiny Value
    Dim Group As SByte = 0 'Shiny Group Selection
#End Region
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckLocal()
        LoadSettings()
        UpdateCheck()
    End Sub
    Private Sub Main_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Default_Form()
    End Sub
    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        WriteSettings()
    End Sub
    Private Sub LoadSettings()
        tscb_LanguageUI.Text = My.Settings.LanguageUI
        tscb_LanguageGame.Text = My.Settings.LanguageGame
        cb_LeadList.SelectedIndex = My.Settings.Default_Lead
        If My.Settings.Default_Game <> Nothing Then cb_GameList.SelectedIndex = My.Settings.Default_Game
        CheckLang()
    End Sub

#Region "Language"
    Private Sub CheckLang()
        My.Settings.LanguageUI = tscb_LanguageUI.Text
        Select Case tscb_LanguageUI.Text
            Case "English"
                LangRes = New Resources.ResourceManager("CuteCharmIDGen.English", Reflection.Assembly.GetExecutingAssembly())
                lklb_Author.Location = New Point(17, 303)
                cb_GameList.DropDownWidth = 139
            Case "Français"
                LangRes = New Resources.ResourceManager("CuteCharmIDGen.Français", Reflection.Assembly.GetExecutingAssembly())
                lklb_Author.Location = New Point(21, 303)
                cb_GameList.DropDownWidth = 165
        End Select
        Me.Text = LangRes.GetString("Title") & " (" & My.Resources._date & ")"
        Natures = {LangRes.GetString("Hardy"), LangRes.GetString("Lonely"), LangRes.GetString("Brave"), LangRes.GetString("Adamant"),
            LangRes.GetString("Naughty"), LangRes.GetString("Bold"), LangRes.GetString("Docile"), LangRes.GetString("Relaxed"),
            LangRes.GetString("Impish"), LangRes.GetString("Lax"), LangRes.GetString("Timid"), LangRes.GetString("Hasty"),
            LangRes.GetString("Serious"), LangRes.GetString("Jolly"), LangRes.GetString("Naive"), LangRes.GetString("Modest"),
            LangRes.GetString("Mild"), LangRes.GetString("Quiet"), LangRes.GetString("Bashful"), LangRes.GetString("Rash"),
            LangRes.GetString("Calm"), LangRes.GetString("Gentle"), LangRes.GetString("Sassy"), LangRes.GetString("Careful"),
            LangRes.GetString("Quirky")}
        gb_ShinyGroups.Text = LangRes.GetString("Shiny Group")
        gb_RandomChoice.Text = LangRes.GetString("Shiny Group Selection")
        rb_RandomFixed.Text = LangRes.GetString("Random") & " (" & LangRes.GetString("w/o") & " " & LangRes.GetString("Group") & " 4)"
        rb_RandomPure.Text = LangRes.GetString("Random") & " (" & LangRes.GetString("w/") & " " & LangRes.GetString("Group") & " 4)"
        rb_Choose.Text = LangRes.GetString("Choose")
        cb_LeadList.Items.Clear()
        cb_LeadList.Items.Add(LangRes.GetString("Male") & " (" & LangRes.GetString("Any") & "% ♀)")
        cb_LeadList.Items.Add(LangRes.GetString("Female") & " (87.5% ♂)")
        cb_LeadList.Items.Add(LangRes.GetString("Female") & " (75% ♂)")
        cb_LeadList.Items.Add(LangRes.GetString("Female") & " (50% ♂)")
        cb_LeadList.Items.Add(LangRes.GetString("Female") & " (25% ♂)")
        lb_Lead.Text = LangRes.GetString("Lead") & ":"
        cb_LeadList.SelectedIndex = My.Settings.Default_Lead
        cb_GameList.Items.Clear()
        cb_GameList.Items.Add(LangRes.GetString("Diamond") & "/" & LangRes.GetString("Pearl"))
        cb_GameList.Items.Add(LangRes.GetString("Platinum"))
        cb_GameList.Items.Add(LangRes.GetString("HeartGold") & "/" & LangRes.GetString("SoulSilver"))
        lb_Game.Text = LangRes.GetString("Version") & ":"
        cb_GameList.SelectedIndex = My.Settings.Default_Game
        gb_TrainerID.Text = LangRes.GetString("Trainer ID")
        rb_RandomTID.Text = LangRes.GetString("Random")
        lklb_Update.Text = LangRes.GetString("Update")
        lb_By.Text = LangRes.GetString("By")
        gb_CuteCharmLead.Text = LangRes.GetString("Cute Charm Pokemon")
        cx_Lead.Text = If(cx_Lead.Checked, LangRes.GetString("Enabled"), LangRes.GetString("Disabled"))
        lb_CuteCharmLead.Text = LangRes.GetString("Lead") & ":"
        gb_Storage.Text = LangRes.GetString("Manage Leads")
        gb_PC.Text = LangRes.GetString("PCLocation")
        FillPC()
        bt_Import.Text = LangRes.GetString("Import")
        bt_Storage.Text = LangRes.GetString("Storage")
        gb_ARButtons.Text = LangRes.GetString("ARButtons")
        lb_ARCodeOutput.Text = LangRes.GetString("ARCode") & ":"
        bt_Calculate.Text = LangRes.GetString("Calculate")
        tsmi_Language.Text = LangRes.GetString("Language")
        tsmi_LangGame.Text = LangRes.GetString("Game")
        Importek4ToolStripMenuItem.Text = LangRes.GetString("Import") & " PKM"
        ToolStripMenuItem2.Text = "                  " & LangRes.GetString("Donate")
        MenuStrip1.Refresh()
    End Sub
    Private Sub ChangeLangUI() Handles tscb_LanguageUI.TextChanged, tscb_LanguageUI.SelectedIndexChanged, tscb_LanguageUI.TextUpdate
        My.Settings.LanguageUI = tscb_LanguageUI.Text
        CheckLang()
    End Sub
    Private Sub ChangeLangGame() Handles tscb_LanguageGame.TextChanged, tscb_LanguageGame.SelectedIndexChanged, tscb_LanguageGame.TextUpdate
        My.Settings.LanguageGame = tscb_LanguageGame.Text
    End Sub
#End Region
#Region "Essentials"
    'Link to Update version
    Private Sub Lklb_Update_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lklb_Update.LinkClicked
        If My.Computer.Network.IsAvailable And Pinger() Then
            Process.Start("https://github.com/RenegadeRaven/CuteCharmIDGenie/releases/latest")
        Else
            MsgBox(LangRes.GetString("No Internet connection") & "
" & LangRes.GetString("NoUpdate"), 1,,,, LangRes.GetString("Error") & " 404")
        End If
    End Sub

    'Link the Author's, yours truly, Github Page
    Private Sub Lklb_Author_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lklb_Author.LinkClicked
        If My.Computer.Network.IsAvailable And Pinger() Then
            Process.Start("https://github.com/RenegadeRaven")
        Else
            MsgBox(LangRes.GetString("No Internet connection") & "
" & LangRes.GetString("LookMeUp"), 1,,,, LangRes.GetString("Error") & " 404")
        End If
    End Sub

    'PayPal Donate Button
    Private Sub Pb_Donate_Click(sender As Object, e As EventArgs) Handles pb_Donate.Click, ToolStripMenuItem2.Click
        Thread.Sleep(200)
        If My.Computer.Network.IsAvailable And Pinger() Then
            Process.Start("https://www.paypal.com/donate/?hosted_button_id=V3U6Q93MJ9MZC")
        Else
            MsgBox(LangRes.GetString("No Internet connection") & "
" & LangRes.GetString("Gesture"), 1,,,, LangRes.GetString("Error") & " 404")
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
    'Checks Local Folders
    Private Sub CheckLocal()
        Dim locals As String() = {Local, Local & "\Leads", Local & "\Leads\Male", Local & "\Leads\Female", Local & "\Leads\Other"}
        CreateFolders(locals)
        CreateFiles({{Local & "\Leads\Male\174 - MAGIC - D8D7F7D437DE.ek4", My.Resources.MaleLead}, {Local & "\Leads\Female\174 - MAGIC - D8D95E400116.ek4", My.Resources.FemaleLead}})

        If Not File.Exists(Local & "\settings.xml") Then WriteSettings()
        ReadSettings()
    End Sub

    'Adds text onto the PictureBoxes
    Private Sub DrawTXT(ByVal txt As String, ByVal pb As PictureBox, ByVal pnt As Point, Optional bg As Boolean = True, Optional fnts As Single = 9) '*
        Dim myfont As New Font("Calibri", fnts, FontStyle.Regular)
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
            DrawTXT("*" & LangRes.GetString("Shiny Group") & " " & index, i, New Point(LangRes.GetString("TableHeaderX"), LangRes.GetString("TableHeaderY")),, LangRes.GetString("TableHeaderFont"))
            index += 1
        Next
        Select Case cb_LeadList.SelectedIndex
            Case 0
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

    'Sets up the default Form, etc. on load
    Private Sub Default_Form() '*
        rb_ShinyGroup1.Checked = False
        gb_ShinyGroups.Enabled = False
        rb_RandomFixed.PerformClick()
        rb_RandomTID.PerformClick()
        rtb_ARCodeOutput.Enabled = False

        pb_Down.BackgroundImage.RotateFlip(RotateFlipType.RotateNoneFlipY)
        pb_Left.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipX)
        pb_Left.Size = New Size(24, 20)
        pb_Right.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
        pb_Right.Size = New Size(24, 20)

        FillPC()
        Default_Values()

        If My.Settings.CuteCharmLead = True Then
            cx_Lead.Checked = True
            Cx_Lead_CheckedChanged(0, New EventArgs)
        ElseIf My.Settings.CuteCharmLead = False Then
            cx_Lead.Checked = False
            Cx_Lead_CheckedChanged(0, New EventArgs)
        End If
        doneLoad = True
        PicTXT()
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup3, pb_ShinyGroup4})
        ListDE(table)
    End Sub
    Private Sub FillPC()
        cb_BoxList.Items.Clear()
        For i = 1 To 18 Step 1
            cb_BoxList.Items.Add(LangRes.GetString("Box") & " " & i)
        Next i
        cb_SlotList.Items.Clear()
        For i = 1 To 30 Step 1
            cb_SlotList.Items.Add(LangRes.GetString("Slot") & " " & i)
        Next i
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

    'Activation Button filling
    Private Sub ClickButton(btn As PictureBox, Optional bg As Boolean = False)
        Dim text As String = Nothing
        Dim xy(1) As Byte
        Dim fill_Img As Bitmap = Nothing
        Dim empty_Img As Bitmap = Nothing
        Dim Btn_Bool As Boolean
        Dim Size As Byte = 12
        If btn Is pb_Y Then
            text = "Y"
            xy = {6, 1}
            fill_Img = My.Resources.buttoninner
            empty_Img = My.Resources.button
            Btn_Bool = AR_Code.Button_Y
        ElseIf btn Is pb_X Then
            text = "X"
            xy = {5, 1}
            fill_Img = My.Resources.buttoninner
            empty_Img = My.Resources.button
            Btn_Bool = AR_Code.Button_X
        ElseIf btn Is pb_L Then
            text = "L"
            xy = {12, 1}
            fill_Img = My.Resources.shoulderinner
            empty_Img = My.Resources.shoulder
            Btn_Bool = AR_Code.Button_L
        ElseIf btn Is pb_R Then
            text = "R"
            xy = {11, 1}
            fill_Img = My.Resources.shoulderinner
            empty_Img = My.Resources.shoulder
            Btn_Bool = AR_Code.Button_R
        ElseIf btn Is pb_Down Then
            text = "↓"
            xy = {0, 2}
            fill_Img = My.Resources.dpadinner
            fill_Img.RotateFlip(RotateFlipType.RotateNoneFlipY)
            empty_Img = My.Resources.dpad
            empty_Img.RotateFlip(RotateFlipType.RotateNoneFlipY)
            Btn_Bool = AR_Code.Button_Down
        ElseIf btn Is pb_Up Then
            text = "↑"
            xy = {0, 0}
            fill_Img = My.Resources.dpadinner
            empty_Img = My.Resources.dpad
            Btn_Bool = AR_Code.Button_Up
        ElseIf btn Is pb_Right Then
            text = "→"
            xy = {4, 0}
            fill_Img = My.Resources.dpadinner
            fill_Img.RotateFlip(RotateFlipType.Rotate90FlipNone)
            empty_Img = My.Resources.dpad
            empty_Img.RotateFlip(RotateFlipType.Rotate90FlipNone)
            Btn_Bool = AR_Code.Button_Right
        ElseIf btn Is pb_Left Then
            text = "←"
            xy = {0, 0}
            fill_Img = My.Resources.dpadinner
            fill_Img.RotateFlip(RotateFlipType.Rotate90FlipX)
            empty_Img = My.Resources.dpad
            empty_Img.RotateFlip(RotateFlipType.Rotate90FlipX)
            Btn_Bool = AR_Code.Button_Left
        ElseIf btn Is pb_Start Then
            text = "Start"
            xy = {8, 1}
            Size = 9
            fill_Img = My.Resources.ovalbuttoninner
            empty_Img = My.Resources.ovalbutton
            Btn_Bool = AR_Code.Button_Start
        ElseIf btn Is pb_Select Then
            text = "Select"
            xy = {4, 1}
            Size = 9
            fill_Img = My.Resources.ovalbuttoninner
            empty_Img = My.Resources.ovalbutton
            Btn_Bool = AR_Code.Button_Select
        ElseIf btn Is pb_B Then
            text = "B"
            xy = {5, 1}
            fill_Img = My.Resources.buttoninner
            empty_Img = My.Resources.button
            Btn_Bool = AR_Code.Button_B
        ElseIf btn Is pb_A Then
            text = "A"
            xy = {4, 1}
            fill_Img = My.Resources.buttoninner
            empty_Img = My.Resources.button
            Btn_Bool = AR_Code.Button_A
        End If
        My.Settings.AR_Activation = AR_Code.ActivateButtons
        If Btn_Bool = True Then
            btn.Image = fill_Img
            DrawTXT(text, btn, New Point(xy(0), xy(1)), False, Size)
        Else
            btn.Image = empty_Img
            DrawTXT(text, btn, New Point(xy(0), xy(1)), bg, Size)
        End If
    End Sub

    'Populates List of Lead Pokemon
    Private Sub GetLeadList()
        cb_EK4List.Items.Clear()
        Dim di As New IO.DirectoryInfo(Local & "\Leads" & If(cb_LeadList.SelectedIndex = 0, "\Male", "\Female"))
        Dim aryFi As IO.FileInfo() = di.GetFiles("*.ek4")
        For Each fi As IO.FileInfo In aryFi
            cb_EK4List.Items.Add(fi.Name.Replace(".ek4", ""))
        Next
        Dim di2 As New IO.DirectoryInfo(Local & "\Leads" & "\Other")
        aryFi = di2.GetFiles("*.ek4")
        For Each fi As IO.FileInfo In aryFi
            cb_EK4List.Items.Add("『" & fi.Name.Replace(".ek4", "") & "』")
        Next
    End Sub
#End Region
#Region "Menu"
    'Opens Local Folder where Leads are stored
    Private Sub Bt_Storage_Click(sender As Object, e As EventArgs) Handles bt_Storage.Click
        Process.Start(Local & "\Leads")
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
    Private Function GenAR() As Byte()
        Select Case cb_GameList.SelectedIndex
            Case 0 'DP
                AR_Code.Pointer = AR.Pointer_DP(tscb_LanguageGame.SelectedIndex)
                AR_Code.ID_Location = AR.ID_Location_DP
                AR_Code.Box_Location = AR.Box_Location_DP + (cb_BoxList.SelectedIndex * AR.Box_Buffer_DPPt) + (cb_SlotList.SelectedIndex * AR.EK4_Length)
                lb_ARGame.Text = "DP"
            Case 1 'Pt
                AR_Code.Pointer = AR.Pointer_Pt(tscb_LanguageGame.SelectedIndex)
                AR_Code.ID_Location = AR.ID_Location_Pt
                AR_Code.Box_Location = AR.Box_Location_Pt + (cb_BoxList.SelectedIndex * AR.Box_Buffer_DPPt) + (cb_SlotList.SelectedIndex * AR.EK4_Length)
                lb_ARGame.Text = "Pt"
            Case 2 'HGSS
                AR_Code.Pointer = AR.Pointer_HGSS(tscb_LanguageGame.SelectedIndex)
                AR_Code.ID_Location = AR.ID_Location_HGSS
                AR_Code.Box_Location = AR.Box_Location_HGSS + (cb_BoxList.SelectedIndex * AR.Box_Buffer_HGSS) + (cb_SlotList.SelectedIndex * AR.EK4_Length)
                lb_ARGame.Text = "HGSS"
        End Select
        If My.Settings.CuteCharmLead = True Then
            If cb_EK4List.Text.Contains("『") Or cb_EK4List.Text.Contains("』") Then
                EK4toAR(Local & "\Leads" & "\Other\" & cb_EK4List.Text.Replace("『", "").Replace("』", "") & ".ek4")
            Else
                Select Case cb_LeadList.SelectedIndex
                    Case 0
                        EK4toAR(Local & "\Leads" & "\Male\" & cb_EK4List.Text & ".ek4")
                    Case Else
                        EK4toAR(Local & "\Leads" & "\Female\" & cb_EK4List.Text & ".ek4")
                End Select
            End If
        End If
        Return AR_Code.Build(My.Settings.CuteCharmLead)
    End Function

    'Converts EK4 to Action Replay Code compatible data
    Private Sub EK4toAR(ByVal myFile As String) '*
        Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
        AR_Code.Lead_EK4 = LittleEndian.LittleEndian(myBytes.Take(AR.EK4_Length).ToArray())
    End Sub

    'Formats string into AR Code
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
#End Region
#Region "Pick ID"
    'Picks IDs
    Private Sub PickID()
        TSVt = Math.Floor(-0.708 * cb_LeadList.SelectedIndex ^ 4 + 5.75 * cb_LeadList.SelectedIndex ^ 3 + -13.792 * cb_LeadList.SelectedIndex ^ 2 + 14.75 * cb_LeadList.SelectedIndex)
        'TSVt = Math.Floor(-0.70834335 * cb_LeadList.SelectedIndex ^ 4 + 5.7501018 * cb_LeadList.SelectedIndex ^ 3 + -13.791902 * cb_LeadList.SelectedIndex ^ 2 + 14.7501436 * cb_LeadList.SelectedIndex)
        TSVt += If(rb_Choose.Checked = True, Group - 1, Group)
        Dim gen As New Random
        AR_Code.TrainerID = If(rb_ChooseTID.Checked = True, nud_TrainerID.Value, gen.Next(0, 65536))
        AR_Code.SecretID = AR_Code.TrainerID Xor (gen.Next(0, 8) + (TSVt << 3))
        If ValueCheck() Then Exit Sub
        lb_TIDValue.Text = AR_Code.TrainerID
        lb_SIDValue.Text = AR_Code.SecretID
        lb_TIDValue.Show()
        lb_SIDValue.Show()
    End Sub

    'Verifies IDs
    Private Function ValueCheck()
        Dim result As Integer = ((AR_Code.TrainerID Xor AR_Code.SecretID) >> 3)
        Return Not (result = TSVt)
    End Function

    'Picks Group
    Private Sub PickGroup()
        Dim gen As New Random
        If rb_RandomFixed.Checked = True Then
            Group = Math.Floor((gen.Next(0, 30)) / 10)
            If rb_RandomFixed.Text.Contains("1") Then Group += 1
        ElseIf rb_RandomPure.Checked = True Then
            Group = Math.Floor((gen.Next(0, 40)) / 10)
        End If
    End Sub

    'Sets to Random
    Private Sub Rb_RandomTID_CheckedChanged(sender As Object, e As EventArgs) Handles rb_RandomTID.CheckedChanged
        nud_TrainerID.Enabled = False
    End Sub

    'User's Choice
    Private Sub Rb_ChooseTID_CheckedChanged(sender As Object, e As EventArgs) Handles rb_ChooseTID.CheckedChanged
        nud_TrainerID.Enabled = True
    End Sub
#End Region
#Region "Controls"
#Region "Shiny Group Selection"
    'User Chooses Shiny Group
    Private Sub Rb_Choose_CheckedChanged(sender As Object, e As EventArgs) Handles rb_Choose.CheckedChanged
        Dim table As New List(Of PictureBox)({pb_ShinyGroup1, pb_ShinyGroup2, pb_ShinyGroup3, pb_ShinyGroup4})
        If rb_Choose.Checked = True Then
            gb_ShinyGroups.Enabled = True
            ListEnable(table)
        Else
            gb_ShinyGroups.Enabled = False
            ListDE(table)
        End If
    End Sub
#End Region
#Region "Specific Shiny Group RadioButtons"
    'Enable or disable table columns
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
        If AR_Code.Button_Down = False Then Dpad()
        AR_Code.Button_Down = Not AR_Code.Button_Down
        ClickButton(pb_Down)
    End Sub
    Private Sub Pb_Up_Click(sender As Object, e As EventArgs) Handles pb_Up.Click
        If AR_Code.Button_Up = False Then Dpad()
        AR_Code.Button_Up = Not AR_Code.Button_Up
        ClickButton(pb_Up)
    End Sub
    Private Sub Pb_Right_Click(sender As Object, e As EventArgs) Handles pb_Right.Click
        If AR_Code.Button_Right = False Then Dpad()
        AR_Code.Button_Right = Not AR_Code.Button_Right
        ClickButton(pb_Right)
    End Sub
    Private Sub Pb_Left_Click(sender As Object, e As EventArgs) Handles pb_Left.Click
        If AR_Code.Button_Left = False Then Dpad()
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

    'Only one dpad button for you!
    Private Sub Dpad()
        If AR_Code.Button_Down Then
            Pb_Down_Click(0, New EventArgs)
        ElseIf AR_Code.Button_Up Then
            Pb_Up_Click(0, New EventArgs)
        ElseIf AR_Code.Button_Right Then
            Pb_Right_Click(0, New EventArgs)
        ElseIf AR_Code.Button_Left Then
            Pb_Left_Click(0, New EventArgs)
        End If
    End Sub
#End Region
#Region "Lead"
    'Is Lead Enabled?
    Private Sub Cx_Lead_CheckedChanged(sender As Object, e As EventArgs) Handles cx_Lead.CheckedChanged
        If cx_Lead.Checked = True Then
            cx_Lead.Text = LangRes.GetString("Enabled")
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
            cx_Lead.Text = LangRes.GetString("Disabled")
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

    'Import Error Message Class
    Private Class ImportMessage
        Public NotCCAbility As Boolean = False
        Public NotCCpkm As Boolean = False
        Public UnknownGender As Boolean = False

        Public Function NeedMsg(im As ImportMessage)
            If im.NotCCAbility Or im.NotCCpkm Or im.UnknownGender Then Return True
            Return False
        End Function

        Public Function Message(im As ImportMessage)
            If NeedMsg(im) Then
                Dim msg As String = LangRes.GetString("Undetermined") & "
"
                If im.NotCCpkm = True Then msg += " - " & LangRes.GetString("Species") & "
"
                If im.UnknownGender = True Then msg += " - " & LangRes.GetString("Gender") & "
"
                If im.NotCCAbility = True Then msg += " - " & LangRes.GetString("Ability") & "
"
                msg += LangRes.GetString("ImportMon")
                Return msg
            Else
                Return Nothing
            End If
        End Function
    End Class

    'List of Cute Charm Pokemon
    Public Class Pokemon
        Public Property ID As Short
        Public Property Name As String
        Public Function Pokemons() As List(Of Pokemon)
            Return New List(Of Pokemon) From {
               New Pokemon With {.ID = 35, .Name = LangRes.GetString("Clefairy")},
               New Pokemon With {.ID = 36, .Name = LangRes.GetString("Clefable")},
               New Pokemon With {.ID = 39, .Name = LangRes.GetString("Jigglypuff")},
               New Pokemon With {.ID = 40, .Name = LangRes.GetString("Wigglytuff")},
               New Pokemon With {.ID = 173, .Name = LangRes.GetString("Cleffa")},
               New Pokemon With {.ID = 174, .Name = LangRes.GetString("Igglybuff")},
               New Pokemon With {.ID = 300, .Name = LangRes.GetString("Skitty")},
               New Pokemon With {.ID = 301, .Name = LangRes.GetString("Delcatty")},
               New Pokemon With {.ID = 428, .Name = LangRes.GetString("Lopunny")},
               New Pokemon With {.ID = -1, .Name = "???"}
               }
        End Function
        Public Function GetPokeName(Dex As Integer)
            Dim list As List(Of Pokemon) = Pokemons()
            For Each i As Pokemon In list
                If i.ID = Dex Then Return i.Name
            Next
            Return Nothing
        End Function
    End Class

    'To Encrypt, or to decrypt?
    Private Function Crypto(filePath As String)
        Dim b As Byte() = File.ReadAllBytes(filePath)
        If filePath.EndsWith(".ek4") Then
            Return DecryptIfEncrypted45(b)
        ElseIf filePath.EndsWith(".pk4") Then
            Return EncryptIfDecrypted45(b)
        End If
        Return b
    End Function

    'Import PK4/EK4
    Private Sub Importek4ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Importek4ToolStripMenuItem.Click, bt_Import.Click
        Try
            Dim FileSelect As New OpenFileDialog With {.Filter = "PKM File (*.pk4;*.ek4)|*.pk4;*.ek4|All files (*.*)|*.*"}
            Dim res As DialogResult = FileSelect.ShowDialog()
            If res = Windows.Forms.DialogResult.Cancel Then Exit Sub
            Dim pkm As New PK4
            Dim ekm As Byte() = Nothing
            Dim crypt As Boolean
            Dim LeadPath As String = Local & "\Leads\Gender\"
            If FileSelect.FileName.EndsWith(".ek4") Then
                crypt = True
                pkm.Data = Crypto(FileSelect.FileName)
            ElseIf FileSelect.FileName.EndsWith(".pk4") Then
                crypt = False
                pkm.Data = File.ReadAllBytes(FileSelect.FileName)
                ekm = EncryptIfDecrypted45(pkm.Data)
            End If
            Dim t As New Pokemon With {.ID = pkm.Dex}
            Dim im As New ImportMessage
            With pkm
                Dim CPoke As Boolean = t.Pokemons().Any(Function(x) x.ID = pkm.Dex)
                Dim Species As String = t.GetPokeName(If(CPoke = True, pkm.Dex, -1))
                Dim GenderRatio As SByte = If(pkm.Dex = 428, 126, If(CPoke = True, 190, -1))
                Dim Ability As Boolean = (Convert.ToString(.PID, 2)).EndsWith("0")
                Dim Gender As UInteger = .PID Mod 256
                If Ability = False Then im.NotCCAbility = True
                If GenderRatio = -1 Then im.UnknownGender = True
                If Species = "???" Then im.NotCCpkm = True
                If im.NeedMsg(im) Then
                    Select Case MsgBox(im.Message(im), 2, LangRes.GetString("Yes"), LangRes.GetString("No"),, LangRes.GetString("Error"))
                        Case 6
                            LeadPath = LeadPath.Replace("\Leads\Gender\", "\Leads\Other\")
                            PlaceLeadFile(FileSelect, ekm, LeadPath, crypt)
                            MsgBox(LangRes.GetString("OtherMon"))
                        Case 7
                            Exit Sub
                    End Select
                Else
                    Select Case Gender <= GenderRatio
                        Case True
                            Species &= " - " & LangRes.GetString("Female")
                            LeadPath = LeadPath.Replace("\Leads\Gender\", "\Leads\Female\")
                        Case False
                            Species &= " - " & LangRes.GetString("Male")
                            LeadPath = LeadPath.Replace("\Leads\Gender\", "\Leads\Male\")
                    End Select
                    PlaceLeadFile(FileSelect, ekm, LeadPath, crypt)
                    MsgBox(Species)
                End If
            End With
            GetLeadList()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub PlaceLeadFile(FileSelect As OpenFileDialog, ekm As Byte(), LeadPath As String, crypt As Boolean)
        If Not File.Exists(LeadPath & FileSelect.SafeFileName) Then
            Select Case crypt
                Case True
                    File.Copy(FileSelect.FileName, LeadPath & FileSelect.SafeFileName)
                Case False
                    File.WriteAllBytes(LeadPath & FileSelect.SafeFileName.Replace(".pk4", ".ek4"), ekm)
            End Select
        End If
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
        ElseIf cb_LeadList.SelectedItem = "" Then
            cb_LeadList.ForeColor = Color.Red
            lb_Lead.ForeColor = Color.Red
            Return True
        ElseIf Group = 0 And rb_Choose.Checked = True Then
            gb_ShinyGroups.ForeColor = Color.Red
            Return True
        ElseIf rb_RandomFixed.Checked = False And rb_RandomPure.Checked = False And rb_Choose.Checked = False Then
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
            cb_LeadList.ForeColor = DefaultForeColor
            lb_Game.ForeColor = DefaultForeColor
            lb_Lead.ForeColor = DefaultForeColor
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
        PickGroup()
        PickID()
        Thread.Sleep(300)
        rtb_ARCodeOutput.Text = SpacingAR(ByteArrayToHexString(GenAR()))
        rtb_ARCodeOutput.Enabled = True
        My.Settings.Default_Game = cb_GameList.SelectedIndex
    End Sub
#End Region
End Class
