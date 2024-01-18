Imports System.Xml
Module SettingsFile
    Dim Settings As XmlDocument

    Public Sub ReadSettings()
        Settings = New XmlDocument
        Settings.Load(Main.Local & "\settings.xml")
        Dim vers As Version = Version.Parse(If(GetValue("Version"), "1.1.5.0"))
        My.Settings.BoxLocation = GetValueOrDefault("BoxLocation")
        My.Settings.AR_Activation = Convert.ToUInt16(If(GetValue("ARButtons"), Hex(My.Settings.PropertyValues("ARButtons").Property.DefaultValue)), 16)
        My.Settings.Default_Game = GetValueOrDefault("DefaultGame")
        My.Settings.Default_Lead = GetValueOrDefault("DefaultLead")
        My.Settings.CuteCharmLead = GetValueOrDefault("UseLead")
        If (My.Application.Info.Version.CompareTo(vers) <= 0) Then
            My.Settings.LanguageUI = GetValueOrDefault("LanguageUI")
            My.Settings.LanguageGame = GetValueOrDefault("LanguageGame")
        Else
            My.Settings.LanguageUI = GetValueOrDefault("Language")
            File.Delete(Main.Local & "\settings.xml")
            WriteSettings()
        End If
    End Sub
    Private Function GetValue(SettingName) As String
        Dim node As XmlNode = Settings.SelectSingleNode("Settings/" & SettingName)
        Return node?.InnerText
    End Function
    Private Function GetValueOrDefault(SettingName) As String
        Dim node As XmlNode = Settings.SelectSingleNode("Settings/" & SettingName)
        Return If(node.InnerText, My.Settings.PropertyValues(SettingName).Property.DefaultValue)
    End Function
    Public Sub WriteSettings()
        If Not File.Exists(Main.Local & "\settings.xml") Then CreateSettings()
        SetValue("Version", My.Application.Info.Version.ToString())
        SetValue("LanguageUI", My.Settings.LanguageUI)
        SetValue("LanguageGame", My.Settings.LanguageGame)
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
        If SettingNode IsNot Nothing Then
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
