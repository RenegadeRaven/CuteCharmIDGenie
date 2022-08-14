Imports Newtonsoft.Json.Linq
Module SettingsFile
    Dim Settings As JObject

    Public Sub ReadSettings()
        Dim lang As String = File.ReadAllText(Main.Local & "\settings.json")
        If lang IsNot Nothing Then Settings = JObject.Parse(lang)
        'LangData("New Update Available! ").ToString()
        My.Settings.BoxLocation = Settings("BoxLocation")
        My.Settings.AR_Activation = Convert.ToUInt16(Settings("ActivationButtons"), 16)
        My.Settings.Default_Game = Settings("DefaultGame")
        My.Settings.Default_Lead = Settings("DefaultLead")
        My.Settings.CuteCharmLead = Settings("CuteCharmLead")
    End Sub
    Public Sub WriteSettings()
        If Not File.Exists(Main.Local & "\settings.json") Then CreateSettings()
        Dim lang As String = File.ReadAllText(Main.Local & "\settings.json")
        If lang IsNot Nothing Then Settings = JObject.Parse(lang)
        Settings("BoxLocation") = My.Settings.BoxLocation
        Settings("ActivationButtons") = Hex(My.Settings.AR_Activation)
        Settings("DefaultGame") = My.Settings.Default_Game
        Settings("DefaultLead") = My.Settings.Default_Lead
        Settings("CuteCharmLead") = My.Settings.CuteCharmLead
        File.WriteAllText(Main.Local & "\settings.json", Settings.ToString)
    End Sub
    Public Sub CreateSettings()
        Dim dSettings As Object = New JObject()
        dSettings.BoxLocation = My.Settings.BoxLocation
        dSettings.ActivationButtons = Hex(My.Settings.AR_Activation)
        dSettings.DefaultGame = My.Settings.Default_Game
        dSettings.DefaultLead = My.Settings.Default_Lead
        dSettings.CuteCharmLead = My.Settings.CuteCharmLead
        File.WriteAllText(Main.Local & "\settings.json", dSettings.ToString)
    End Sub
End Module
