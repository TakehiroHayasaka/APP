Public Interface ITypeParser

    Function CanParse(keyword As String) As Boolean

    Sub Parse(line As String)

End Interface

Public MustInherit Class TypeParserBase
    Implements ITypeParser

    Protected MustOverride ReadOnly Property Keyword As String

    Public Function CanParse(keyword As String) As Boolean Implements ITypeParser.CanParse
        Return ObjTool.EqualsInvariantCultureIgnoreCase(keyword, Me.Keyword)
    End Function

    Public MustOverride Sub Parse(line As String) Implements ITypeParser.Parse

End Class

Public Class FaceParser
    Inherits TypeParserBase

    Private ReadOnly _faceGroup As ObjDataStore

    Public Sub New(faceGroup As ObjDataStore)
        Me._faceGroup = faceGroup
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "f"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        Dim vertices As String() = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim face As ObjFace = New ObjFace
        For Each vertexString As String In vertices
            Dim faceVertex = ParseFaceVertex(vertexString)
            face.AddVertex(faceVertex)
        Next
        _faceGroup.addFace(face)
    End Sub

    Private Function ParseFaceVertex(vertexString As String)
        Dim fields = vertexString.Split(New Char() {"/"c}, StringSplitOptions.None)
        Dim vertexIndex = ObjTool.ParseInvariantInt(fields(0))
        Dim faceVertex As ObjFaceVertex = New ObjFaceVertex(vertexIndex, 0, 0)
        If fields.Length > 1 Then
            faceVertex.TextureIndex = IIf(fields(1).Length = 0, 0, ObjTool.ParseInvariantInt(fields(1)))
        End If
        If fields.Length > 2 Then
            faceVertex.NormalIndex = IIf(fields(2).Length = 0, 0, ObjTool.ParseInvariantInt(fields(2)))
        End If
        ParseFaceVertex = faceVertex
    End Function
End Class

Public Class GroupParser
    Inherits TypeParserBase

    Private ReadOnly _groupDataStore As ObjDataStore

    Public Sub New(groupDataStore As ObjDataStore)
        _groupDataStore = groupDataStore
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "g"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        _groupDataStore.PushGroup(line)
    End Sub
End Class

Public Class NormalParser
    Inherits TypeParserBase

    Private ReadOnly _normalDataStore As ObjDataStore

    Public Sub New(normalDataStore As ObjDataStore)
        _normalDataStore = normalDataStore
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "vn"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        Dim parts As String() = line.Split(" ")
        Dim x As Single = ObjTool.ParseInvariantFloat(parts(0))
        Dim y As Single = ObjTool.ParseInvariantFloat(parts(1))
        Dim z As Single = ObjTool.ParseInvariantFloat(parts(2))
        Dim normal As ObjNormal = New ObjNormal(x, y, z)
        _normalDataStore.AddNormal(normal)
    End Sub
End Class

Public Class TextureParser
    Inherits TypeParserBase

    Private ReadOnly _textureDataStore As ObjDataStore

    Public Sub New(textureDataStore As ObjDataStore)
        _textureDataStore = textureDataStore
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "vt"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        Dim parts As String() = line.Split(" ")
        Dim x As Single = ObjTool.ParseInvariantFloat(parts(0))
        Dim y As Single = ObjTool.ParseInvariantFloat(parts(1))
        Dim texture As ObjTexture = New ObjTexture(x, y)
        _textureDataStore.AddTexture(texture)
    End Sub

End Class

Public Class VertexParser
    Inherits TypeParserBase

    Private ReadOnly _vertexDataStore As ObjDataStore

    Public Sub New(vertexDataStore As ObjDataStore)
        _vertexDataStore = vertexDataStore
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "v"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        Dim parts As String() = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim x = ObjTool.ParseInvariantFloat(parts(0))
        Dim y = ObjTool.ParseInvariantFloat(parts(1))
        Dim z = ObjTool.ParseInvariantFloat(parts(2))
        Dim r = ObjTool.ParseInvariantFloat(parts(3))
        Dim g = ObjTool.ParseInvariantFloat(parts(4))
        Dim b = ObjTool.ParseInvariantFloat(parts(5))
        Dim vertex As ObjVertex = New ObjVertex(x, y, z, r, g, b)
        _vertexDataStore.AddVertex(vertex)
    End Sub
End Class


Public Class MaterialLibraryParser
    Inherits TypeParserBase

    Private ReadOnly _libraryLoaderFacede As MaterialLibraryLoaderFacade

    Public Sub New(libraryLoaderFacade As MaterialLibraryLoaderFacade)
        _libraryLoaderFacede = libraryLoaderFacade
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "usemtl"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        _libraryLoaderFacede.Load(line)
    End Sub
End Class

Public Class UseMaterialParser
    Inherits TypeParserBase

    Private ReadOnly _elementGroup As ObjDataStore

    Public Sub New(elementGroup As ObjDataStore)
        _elementGroup = elementGroup
    End Sub

    Protected Overrides ReadOnly Property Keyword As String
        Get
            Return "usemtl"
        End Get
    End Property

    Public Overrides Sub Parse(line As String)
        _elementGroup.SetMaterial(line)
    End Sub
End Class