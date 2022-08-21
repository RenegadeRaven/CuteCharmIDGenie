Module Common
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
