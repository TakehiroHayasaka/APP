Public Class ObjDataStore

    Private _currentGroup As ObjGroup

    Private ReadOnly _groups As New List(Of ObjGroup)()
    Private ReadOnly _materials As New List(Of ObjMaterial)()

    Private ReadOnly _vertices As New List(Of ObjVertex)()
    Private ReadOnly _textures As New List(Of ObjTexture)()
    Private ReadOnly _normals As New List(Of ObjNormal)()

    Public ReadOnly Property Vertices() As IList(Of ObjVertex)
        Get
            Return _vertices
        End Get
    End Property

    Public ReadOnly Property Textures() As IList(Of ObjTexture)
        Get
            Return _textures
        End Get
    End Property

    Public ReadOnly Property Normals() As IList(Of ObjNormal)
        Get
            Return _normals
        End Get
    End Property

    Public ReadOnly Property Materials() As IList(Of ObjMaterial)
        Get
            Return _materials
        End Get
    End Property

    Public ReadOnly Property Groups() As IList(Of ObjGroup)
        Get
            Return _groups
        End Get
    End Property

    Public Sub AddFace(face As ObjFace)
        PushGroupIfNeeded()

        _currentGroup.addFace(face)
    End Sub

    Public Sub PushGroup(groupName As String)
        _currentGroup = New ObjGroup(groupName)
        _groups.Add(_currentGroup)
    End Sub

    Private Sub PushGroupIfNeeded()
        If _currentGroup Is Nothing Then
            PushGroup("default")
        End If
    End Sub

    Public Sub AddVertex(vertex As ObjVertex)
        _vertices.Add(vertex)
    End Sub

    Public Sub AddTexture(texture As ObjTexture)
        _textures.Add(texture)
    End Sub

    Public Sub AddNormal(normal As ObjNormal)
        _normals.Add(normal)
    End Sub

    Public Sub Push(material As ObjMaterial)
        _materials.Add(material)
    End Sub

    Public Sub SetMaterial(materialName As String)
        Dim material = _materials.SingleOrDefault(Function(x) ObjTool.EqualsInvariantCultureIgnoreCase(x.Name, materialName))
        PushGroupIfNeeded()
        _currentGroup.Material = material
    End Sub
End Class
