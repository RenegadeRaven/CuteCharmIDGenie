Module Startup
    'Creates Local Files and Folders
    Public Sub CreateFolders(ByVal dirs As String())
        Try
            For i = 0 To UBound(dirs) Step 1
                If Not dirs(i).Contains(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then dirs(i) = Main.Local & dirs(i)
                Do While Not Directory.Exists(dirs(i))
                    If Not Directory.Exists(dirs(i)) Then Directory.CreateDirectory(dirs(i))
                Loop
            Next i
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub CreateFiles(ByVal files(,))
        Try
            For i = 0 To UBound(files) Step 1
                If Not files(i, 0).Contains(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then files(i, 0) = Main.Local & files(i, 0)
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
            MsgBox(ex.Message)
        End Try
    End Sub
End Module
