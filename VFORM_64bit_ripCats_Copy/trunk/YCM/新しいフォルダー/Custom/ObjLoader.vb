Imports System.IO

Public Class ObjLoader
    Inherits ObjLoaderBase

    Private ReadOnly _dataStore As ObjDataStore

    Private ReadOnly _typeParsers As List(Of ITypeParser) = New List(Of ITypeParser)

    Private ReadOnly _unrecognizedLines As List(Of String) = New List(Of String)

    Public Sub New(dataStore As ObjDataStore, faceParser As FaceParser, groupParser As GroupParser, normaLParser As NormalParser, textureParser As TextureParser, vertexParser As VertexParser, materialLibraryParser As MaterialLibraryParser, useMaterialParser As UseMaterialParser)
        _dataStore = dataStore
        SetupTypeParsers( _
                vertexParser, _
                faceParser, _
                normaLParser, _
                textureParser, _
                groupParser, _
                materialLibraryParser, _
                useMaterialParser)
    End Sub

    Private Sub SetupTypeParsers(ParamArray parsers As ITypeParser())
        For Each parser As ITypeParser In parsers
            _typeParsers.Add(parser)
        Next
    End Sub

    Protected Overloads Overrides Sub ParseLine(keyword As String, data As String)
        For Each typeParser As ITypeParser In _typeParsers
            If typeParser.CanParse(keyword) Then
                typeParser.Parse(data)
                Exit Sub
            End If
        Next
        _unrecognizedLines.Add(keyword & " " & data)
    End Sub

    Public Function Load(fileName As String) As LoadResult
        StartLoad(fileName)
        Return CreateResult()
    End Function

    Public Function CreateResult() As LoadResult
        Dim result = New LoadResult()
        result.Vertices = _dataStore.Vertices
        result.Textures = _dataStore.Textures
        result.Normals = _dataStore.Normals
        result.Groups = _dataStore.Groups
        result.Materials = _dataStore.Materials
        Return result
    End Function

End Class

Public MustInherit Class ObjLoaderBase

    Private _lineStreamReader As StreamReader

    Public Sub StartLoad(filePath As String)
        _lineStreamReader = New StreamReader(filePath)
        While Not _lineStreamReader.EndOfStream
            ParseLine()
        End While
        _lineStreamReader.Close()
    End Sub

    Public Sub ParseLine()
        Dim currentLine = _lineStreamReader.ReadLine()
        If String.IsNullOrWhiteSpace(currentLine) Or currentLine(0) = "#" Then
            Exit Sub
        End If

        Dim fields As String() = currentLine.Trim().Split(Nothing, 2)
        Dim keyword As String = fields(0).Trim()
        Dim data As String = fields(1).Trim()
        ParseLine(keyword, data)
    End Sub


    Protected MustOverride Sub ParseLine(keyword As String, data As String)

End Class


Public Class ObjLoaderFactory

    Public Function Create() As ObjLoader
        Dim dataStore = New ObjDataStore()
        Dim faceParser = New FaceParser(dataStore)
        Dim groupParser = New GroupParser(dataStore)
        Dim normalParser = New NormalParser(dataStore)
        Dim textureParser = New TextureParser(dataStore)
        Dim vertexParser = New VertexParser(dataStore)
        Dim materialLibraryLoader = New MaterialLibraryLoader(dataStore)
        Dim materialLibraryLoaderFacade = New MaterialLibraryLoaderFacade(materialLibraryLoader)
        Dim materialLibraryParser = New MaterialLibraryParser(materialLibraryLoaderFacade)
        Dim useMaterialParser = New UseMaterialParser(dataStore)
        Return New ObjLoader(dataStore, faceParser, groupParser, normalParser, textureParser, vertexParser, materialLibraryParser, useMaterialParser)
    End Function

End Class

Public Class LoadResult

    Public Property Vertices As IList(Of ObjVertex)

    Public Property Textures As IList(Of ObjTexture)

    Public Property Normals As IList(Of ObjNormal)

    Public Property Groups As IList(Of ObjGroup)

    Public Property Materials As IList(Of ObjMaterial)

End Class





