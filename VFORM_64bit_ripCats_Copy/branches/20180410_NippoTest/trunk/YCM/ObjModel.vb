
Public Class ObjVertex
    Public Property x As Single

    Public Property y As Single

    Public Property z As Single

    Public Property r As Single

    Public Property g As Single

    Public Property b As Single


    Public Sub New(x As Single, y As Single, z As Single, r As Single, g As Single, b As Single)
        Me.x = x
        Me.y = y
        Me.z = z
        Me.r = r
        Me.g = g
        Me.b = b
    End Sub
    Public Sub New(ByVal objOther As ObjVertex)
        Me.x = objOther.x
        Me.y = objOther.y
        Me.z = objOther.z
        Me.r = objOther.r
        Me.g = objOther.g
        Me.b = objOther.b
    End Sub

    Public Sub SetScale(ByVal dScale As Double)
        Me.x = Me.x / dScale
        Me.y = Me.y / dScale
        Me.z = Me.z / dScale
    End Sub
 
End Class

Public Class ObjVec3
    Public Property x As Single

    Public Property y As Single

    Public Property z As Single

    Public Sub New(x As Single, y As Single, z As Single)
        Me.x = x
        Me.y = y
        Me.z = z
    End Sub

End Class

Public Class ObjNormal

    Public Property x As Single

    Public Property y As Single

    Public Property z As Single

    Public Sub New(x As Single, y As Single, z As Single)
        Me.x = x
        Me.y = y
        Me.z = z
    End Sub

End Class

Public Class ObjTexture
    Public Property x As Single

    Public Property y As Single

    Public Sub New(x As Single, y As Single)
        Me.x = x
        Me.y = y
    End Sub

End Class

Public Class ObjFace

    Private ReadOnly _vertices As List(Of ObjFaceVertex) = New List(Of ObjFaceVertex)

    Public Sub AddVertex(vertex As ObjFaceVertex)
        _vertices.Add(vertex)
    End Sub

    Default Public ReadOnly Property Item(i As Integer) As ObjFaceVertex
        Get
            Return _vertices(i)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return _vertices.Count
        End Get
    End Property

End Class

Public Class ObjFaceVertex

    Public Sub New(vertexIndex As Int32, textureIndex As Int32, normalIndex As Int32)
        Me.VertexIndex = vertexIndex
        Me.TextureIndex = textureIndex
        Me.NormalIndex = normalIndex
    End Sub

    Public Property VertexIndex As Int32

    Public Property TextureIndex As Int32

    Public Property NormalIndex As Int32

End Class


Public Class ObjMaterial
    Public Sub New(materialName As String)
        Name = materialName
    End Sub

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(value As String)
            m_Name = value
        End Set
    End Property
    Private m_Name As String

    Public Property AmbientColor() As ObjVec3
        Get
            Return m_AmbientColor
        End Get
        Set(value As ObjVec3)
            m_AmbientColor = value
        End Set
    End Property
    Private m_AmbientColor As ObjVec3

    Public Property DiffuseColor() As ObjVec3
        Get
            Return m_DiffuseColor
        End Get
        Set(value As ObjVec3)
            m_DiffuseColor = value
        End Set
    End Property
    Private m_DiffuseColor As ObjVec3

    Public Property SpecularColor() As ObjVec3
        Get
            Return m_SpecularColor
        End Get
        Set(value As ObjVec3)
            m_SpecularColor = value
        End Set
    End Property
    Private m_SpecularColor As ObjVec3

    Public Property SpecularCoefficient() As Single
        Get
            Return m_SpecularCoefficient
        End Get
        Set(value As Single)
            m_SpecularCoefficient = value
        End Set
    End Property
    Private m_SpecularCoefficient As Single

    Public Property Transparency() As Single
        Get
            Return m_Transparency
        End Get
        Set(value As Single)
            m_Transparency = value
        End Set
    End Property
    Private m_Transparency As Single

    Public Property IlluminationModel() As Integer
        Get
            Return m_IlluminationModel
        End Get
        Set(value As Integer)
            m_IlluminationModel = value
        End Set
    End Property
    Private m_IlluminationModel As Integer

    Public Property AmbientTextureMap() As String
        Get
            Return m_AmbientTextureMap
        End Get
        Set(value As String)
            m_AmbientTextureMap = value
        End Set
    End Property
    Private m_AmbientTextureMap As String

    Public Property DiffuseTextureMap() As String
        Get
            Return m_DiffuseTextureMap
        End Get
        Set(value As String)
            m_DiffuseTextureMap = value
        End Set
    End Property
    Private m_DiffuseTextureMap As String

    Public Property SpecularTextureMap() As String
        Get
            Return m_SpecularTextureMap
        End Get
        Set(value As String)
            m_SpecularTextureMap = value
        End Set
    End Property
    Private m_SpecularTextureMap As String

    Public Property SpecularHighlightTextureMap() As String
        Get
            Return m_SpecularHighlightTextureMap
        End Get
        Set(value As String)
            m_SpecularHighlightTextureMap = value
        End Set
    End Property
    Private m_SpecularHighlightTextureMap As String

    Public Property BumpMap() As String
        Get
            Return m_BumpMap
        End Get
        Set(value As String)
            m_BumpMap = value
        End Set
    End Property
    Private m_BumpMap As String

    Public Property DisplacementMap() As String
        Get
            Return m_DisplacementMap
        End Get
        Set(value As String)
            m_DisplacementMap = value
        End Set
    End Property
    Private m_DisplacementMap As String

    Public Property StencilDecalMap() As String
        Get
            Return m_StencilDecalMap
        End Get
        Set(value As String)
            m_StencilDecalMap = value
        End Set
    End Property
    Private m_StencilDecalMap As String

    Public Property AlphaTextureMap() As String
        Get
            Return m_AlphaTextureMap
        End Get
        Set(value As String)
            m_AlphaTextureMap = value
        End Set
    End Property
    Private m_AlphaTextureMap As String

End Class


Public Class ObjGroup

    Private ReadOnly _faces As List(Of ObjFace) = New List(Of ObjFace)

    Public Property Name As String

    Public Property Material As ObjMaterial


    Public Sub New(name As String)
        Me.Name = name
    End Sub

    Public ReadOnly Property Faces As IList(Of ObjFace)
        Get
            Return _faces
        End Get
    End Property

    Public Sub addFace(face As ObjFace)
        _faces.Add(face)
    End Sub

End Class

