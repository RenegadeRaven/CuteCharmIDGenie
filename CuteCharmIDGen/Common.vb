Module Common
    'Does Little Endian
    Public Function LittleEndian(ByVal hex_array As Byte()) As Byte()
        Dim new_array(hex_array.Length) As Byte
        For i As Integer = 0 To (new_array.Length - 2) Step 4
            For j As Integer = 3 To 0 Step -1
                new_array(i + (3 - j)) = hex_array(i + j)
            Next j
        Next i
        Return new_array
    End Function
    Public Function ByteArrayToHexString(ByVal myBytes As Byte()) As String
        Dim txtTemp As New System.Text.StringBuilder()
        For Each myByte As Byte In myBytes
            txtTemp.Append(myByte.ToString("X2"))
        Next
        Return txtTemp.ToString()
    End Function
End Module
