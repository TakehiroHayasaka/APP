Public Interface IMaterialStreamProvider

    Function Open(materialFilePath As String) As System.IO.Stream

End Interface

Public Class MaterialLibraryLoader
    Inherits ObjLoaderBase

    Private ReadOnly _materialLibrary As ObjDataStore

    Private _currentMaterial As ObjMaterial

    Private ReadOnly _parseActionDictionary As Dictionary(Of String, Action(Of String)) = New Dictionary(Of String, Action(Of String))()

    Private ReadOnly _unrecognizedLines As List(Of String) = New List(Of String)()

    Public Sub New(materialLibrary As ObjDataStore)
        _materialLibrary = materialLibrary
        AddParseAction("newmtl", AddressOf PushMaterial)
        AddParseAction("Ka", Function(d) ObjTool.InlineAssignHelper(CurrentMaterial.AmbientColor, ParseVec3(d)))
        AddParseAction("Kd", Function(d) ObjTool.InlineAssignHelper(CurrentMaterial.DiffuseColor, ParseVec3(d)))
        AddParseAction("Ks", Function(d) ObjTool.InlineAssignHelper(CurrentMaterial.SpecularColor, ParseVec3(d)))
        AddParseAction("Ns", Function(d) ObjTool.InlineAssignHelper(CurrentMaterial.SpecularCoefficient, ObjTool.ParseInvariantFloat(d)))
        AddParseAction("d", Function(d) ObjTool.InlineAssignHelper(CurrentMaterial.Transparency, ObjTool.ParseInvariantFloat(d)))
        AddParseAction("Tr", Function(d) ObjTool.InlineAssignHelper(CurrentMaterial.Transparency, ObjTool.ParseInvariantFloat(d)))
        AddParseAction("illum", Function(i) ObjTool.InlineAssignHelper(CurrentMaterial.IlluminationModel, ObjTool.ParseInvariantInt(i)))
        AddParseAction("map_Ka", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.AmbientTextureMap, m))
        AddParseAction("map_Kd", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.DiffuseTextureMap, m))
        AddParseAction("map_Ks", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.SpecularTextureMap, m))
        AddParseAction("map_Ns", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.SpecularHighlightTextureMap, m))
        AddParseAction("map_d", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.AlphaTextureMap, m))
        AddParseAction("map_bump", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.BumpMap, m))
        AddParseAction("bump", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.BumpMap, m))
        AddParseAction("disp", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.DisplacementMap, m))
        AddParseAction("decal", Function(m) ObjTool.InlineAssignHelper(CurrentMaterial.StencilDecalMap, m))
    End Sub

    Private ReadOnly Property CurrentMaterial As ObjMaterial
        Get
            Return _currentMaterial
        End Get
    End Property

    Private Sub AddParseAction(key As String, action As Action(Of String))
        _parseActionDictionary.Add(key.ToLowerInvariant(), action)
    End Sub

    Protected Overloads Overrides Sub ParseLine(keyword As String, data As String)
        Dim parseAction = GetKeywordAction(keyword)
        If parseAction Is Nothing Then
            _unrecognizedLines.Add(keyword & " " & data)
            Exit Sub
        End If
        parseAction(data)
    End Sub

    Private Function GetKeywordAction(keyword As String) As Action(Of String)
        Dim action As Action(Of String)
        _parseActionDictionary.TryGetValue(keyword.ToLowerInvariant(), action)
        Return action
    End Function

    Private Sub PushMaterial(materialName As String)
        _currentMaterial = New ObjMaterial(materialName)
        _materialLibrary.Push(_currentMaterial)
    End Sub

    Private Function ParseVec3(data As String) As ObjVec3
        Dim parts As String() = data.Split(" ")
        Dim x As Single = ObjTool.ParseInvariantFloat(parts(0))
        Dim y As Single = ObjTool.ParseInvariantFloat(parts(1))
        Dim z As Single = ObjTool.ParseInvariantFloat(parts(2))
        Return New ObjVec3(x, y, z)
    End Function

    Public Sub Load(filePath As String)
        StartLoad(filePath)
    End Sub

End Class

Public Class MaterialLibraryLoaderFacade

    Private ReadOnly _loader As MaterialLibraryLoader

    Public Sub New(loader As MaterialLibraryLoader)
        _loader = loader
    End Sub

    Public Sub Load(materialFileName As String)
        _loader.Load(materialFileName)
    End Sub


End Class