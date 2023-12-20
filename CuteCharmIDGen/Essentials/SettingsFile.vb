Imports System.Xml
Module SettingsFile
    Dim Settings As XmlDocument

    Public Sub ReadSettings()
        Settings = New XmlDocument
        Settings.Load(Main.Local & "\settings.xml")
        My.Settings.LanguageUI = GetValue("LanguageUI")
        My.Settings.Language = GetValue("Language")
        My.Settings.BoxLocation = GetValue("BoxLocation")
        My.Settings.AR_Activation = Convert.ToUInt16(GetValue("ARButtons"), 16)
        My.Settings.Default_Game = GetValue("DefaultGame")
        My.Settings.Default_Lead = GetValue("DefaultLead")
        My.Settings.CuteCharmLead = GetValue("UseLead")
    End Sub
    Private Function GetValue(SettingName) As String
        Return Settings.SelectSingleNode("Settings/" & SettingName).InnerText
    End Function
    Public Sub WriteSettings()
        If Not File.Exists(Main.Local & "\settings.xml") Then CreateSettings()
        SetValue("LanguageUI", My.Settings.LanguageUI)
        SetValue("Language", My.Settings.Language)
        SetValue("BoxLocation", My.Settings.BoxLocation)
        SetValue("ARButtons", Hex(My.Settings.AR_Activation))
        SetValue("DefaultGame", My.Settings.Default_Game)
        SetValue("DefaultLead", My.Settings.Default_Lead)
        SetValue("UseLead", My.Settings.CuteCharmLead)
        Settings.Save(Main.Local & "\settings.xml")
    End Sub
    Private Sub SetValue(SettingName As String, SettingValue As String)
        Dim SettingNode As XmlElement
        Try
            SettingNode = DirectCast(Settings.SelectSingleNode("Settings/" & SettingName), XmlElement)
        Catch
            SettingNode = Nothing
        End Try
        If Not SettingNode Is Nothing Then
            SettingNode.InnerText = SettingValue
        Else
            SettingNode = Settings.CreateElement(SettingName)
            SettingNode.InnerText = SettingValue
            Settings.SelectSingleNode("Settings").AppendChild(SettingNode)
        End If
    End Sub
    Private Sub CreateSettings()
        Settings = New XmlDocument
        Dim dec As XmlDeclaration = Settings.CreateXmlDeclaration("1.0", "utf-8", String.Empty)
        Settings.AppendChild(dec)

        Dim nodeRoot As XmlNode
        nodeRoot = Settings.CreateNode(XmlNodeType.Element, "Settings", "")
        Settings.AppendChild(nodeRoot)
    End Sub
End Module
