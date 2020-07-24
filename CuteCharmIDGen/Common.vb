Imports System.Text
Module Common
    'Custom MsgBox
    Public Function MsgB(ByVal mes As String, Optional ByVal numB As Integer = 1, Optional ByVal but1 As String = "OK", Optional ByVal but2 As String = "Cancel", Optional ByVal but3 As String = "", Optional ByVal head As String = "")
        Dim msg As New CustomMessageBox(mes, numB, but1, but2, but3, head)
        Dim result = msg.ShowDialog()
        Dim Ans As Integer
        If result = Windows.Forms.DialogResult.Yes Then
            'user clicked "B1"
            Ans = 6
        ElseIf result = Windows.Forms.DialogResult.No Then
            'user clicked "B2"
            Ans = 7
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            'user clicked "B3"
            Ans = 8
        Else
            'user closed the window without clicking a button
            Ans = -1
            'Form4.Close()
            Form.ActiveForm.Close()
        End If
        Return Ans
    End Function
    Public Function GetString(data As Byte(), offset As Byte, count As UShort)
        Return Encoding.Unicode.GetString(data, offset, count).Replace(ChrW(&HFFFF), "").Replace(ChrW(&H0), "").Replace(ChrW(&HFFFE), vbLf)
    End Function
    Public Function SetString(ByVal value As String, ByVal maxLength As Integer)
        ' value = value.Replace(vbLf, ChrW(&HFFFE))
        If value.Length > maxLength Then value = value.Substring(0, maxLength)
        Do While value.Length < maxLength
            value &= ChrW(&HFFFF)
        Loop
        Dim temp As String = value & ChrW(&HFFFF)
        Return Encoding.Unicode.GetBytes(temp)
    End Function
    Public Sub ReadIni()
        With Main.mySettings
            .Filename = Main.Local & "\settings.ini"
            If .OpenIniFile() Then
                My.Settings.BoxLocation = .GetValue("Box Location")
                My.Settings.AR_Activation = Convert.ToUInt16(.GetValue("Activation Buttons"), 16)
                My.Settings.Default_Game = .GetValue("Default Game")
                My.Settings.Default_Lead = .GetValue("Default Lead")
                My.Settings.CuteCharmLead = .GetValue("Cute Charm Lead?")
                '.SetValue("Ticket", My.Settings.ticket)
                If Not .SaveIni Then
                    MsgB("Trouble writing Ini-File")
                End If
            Else
                MsgB("No Ini-File found")
            End If
        End With
    End Sub
    Public Sub WriteIni()
        With Main.mySettings
            .Filename = Main.Local & "\settings.ini"
            If .OpenIniFile() Then
                'Dim MyValue As String = .GetValue("MyKey")
                .SetValue("Box Location", My.Settings.BoxLocation)
                .SetValue("Activation Buttons", Hex(My.Settings.AR_Activation))
                .SetValue("Default Game", My.Settings.Default_Game)
                .SetValue("Default Lead", My.Settings.Default_Lead)
                .SetValue("Cute Charm Lead?", My.Settings.CuteCharmLead)
                If Not .SaveIni Then
                    MsgB("Trouble by writing Ini-File")
                End If
            Else
                MsgB("No Ini-File found")
            End If
        End With
    End Sub
    'Adds needed zeros to hex string
    Public Function Hex_Zeros(ByVal hex_value As String, ByVal length As Integer) '*
        Dim Str As String = hex_value.ToUpper
        Do While Str.Length < length
            Str = "0" & Str
        Loop
        Return Str
    End Function

    'Does Little Endian
    Public Function LittleEndian(ByVal hex_value As String, ByVal length As Integer) '*
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
    Public Function ToLE(ByVal hex_value As String) '*
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
        Next c
        Return endString
    End Function
    Public Function HexStringToByteArray(ByRef strInput As String) As Byte() '*
        Dim length As Integer
        Dim bOutput As Byte()
        Dim c(1) As Integer
        length = strInput.Length / 2
        ReDim bOutput(length - 1)
        For i As Integer = 0 To (length - 1)
            For j As Integer = 0 To 1
                c(j) = Asc(strInput.Chars(i * 2 + j))
                If ((c(j) >= Asc("0")) And (c(j) <= Asc("9"))) Then
                    c(j) = c(j) - Asc("0")
                ElseIf ((c(j) >= Asc("A")) And (c(j) <= Asc("F"))) Then
                    c(j) = c(j) - Asc("A") + &HA
                ElseIf ((c(j) >= Asc("a")) And (c(j) <= Asc("f"))) Then
                    c(j) = c(j) - Asc("a") + &HA
                End If
            Next j
            bOutput(i) = (c(0) * &H10 + c(1))
        Next i
        Return (bOutput)
    End Function
    Public Function ByteArrayToHexString(ByVal myBytes As Byte())
        Dim txtTemp As New System.Text.StringBuilder()
        For Each myByte As Byte In myBytes
            txtTemp.Append(myByte.ToString("X2"))
        Next
        Return txtTemp.ToString()
    End Function
End Module
