Public Class ObjToMeshConverter

    Private _loadResult As LoadResult
    Private _modelGroup As ObjGroup
    Private _meshes As List(Of ObjMesh)
    Private _mesh As ObjMesh
    Private _face As ObjFace

    Public Function Convert(loadResult As LoadResult) As List(Of ObjMesh)
        _loadResult = loadResult
        _meshes = New List(Of ObjMesh)
        ConvertGroups()
        Return _meshes
    End Function

    Private Sub ConvertGroups()
        Dim groups = _loadResult.Groups
        For Each modelGroup In groups
            _modelGroup = modelGroup
            ConvertGroup()
        Next
    End Sub

    Private Sub ConvertGroup()
        _mesh = New ObjMesh()
        _mesh.Name = _modelGroup.Name

        For Each face In _modelGroup.Faces
            _face = face
            ConvertFace()
        Next
        ' CalculateNormals()

        _meshes.Add(_mesh)

    End Sub

    Private Sub ConvertFace()
        If Not _face.Count = 3 Then
            Throw New NotImplementedException("Only triangles boys")
        End If

        For i = 0 To _face.Count - 1
            Dim faceVertex = _face(i)
            Dim vertex As ObjVertex = _loadResult.Vertices(faceVertex.VertexIndex - 1)

            '_mesh.Triangles.Add(New ObjVec3(vertex.x / ScaleToMM, vertex.y / ScaleToMM, vertex.z / ScaleToMM))
            Try
                ' _mesh.Triangles.Add(New ObjVertex(vertex.x / ScaleToMM, vertex.y / ScaleToMM, vertex.z / ScaleToMM, vertex.r, vertex.g, vertex.b))
                _mesh.Triangles.Add(vertex)
            Catch ex As Exception
                Debug.Print("dd")
            End Try

        Next
    End Sub

    Private Sub CalculateNormals()
        For i = 0 To _mesh.Triangles.Count - 1 Step 3
            Dim a = _mesh.Triangles(i)
            Dim b = _mesh.Triangles(i + 1)
            Dim c = _mesh.Triangles(i + 2)
            Dim u = New ObjVec3(b.x - a.x, b.y - a.y, b.z - a.z)
            Dim v = New ObjVec3(c.x - a.x, c.y - a.y, c.z - a.z)
            Dim n = New ObjVec3(u.y * v.z - u.z * v.y, u.z * v.x - u.x * v.z, u.x * v.y - u.y * v.x)

            _mesh.Normals.Add(n)
            _mesh.Normals.Add(n)
            _mesh.Normals.Add(n)
        Next
    End Sub

End Class

Public Class ObjMesh

    Public Property Name As String

    Public Property Triangles As List(Of ObjVertex)

    Public Property Normals As List(Of ObjVec3)

    Public Sub New()
        Triangles = New List(Of ObjVertex)
        Normals = New List(Of ObjVec3)
    End Sub

End Class
