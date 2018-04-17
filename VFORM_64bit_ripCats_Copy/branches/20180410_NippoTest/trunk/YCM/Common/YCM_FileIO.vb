Imports System.IO
Imports System.IO.Directory
Module YCM_FileIO
    Public Sub YCM_CopyDir(ByVal srcPath As String, ByVal aimPath As String)
        ' 2012.12.26 aimPath：「名前を付けて保存」の新しく作成したフォルダのパス

        Try
            If aimPath(aimPath.Length - 1) <> Path.DirectorySeparatorChar Then
                '2012.12.26 Length：現在の System.String オブジェクト内の文字数を取得します。

                'DirectorySeparatorChar：階層ファイル システム編成を反映するパス文字列の、ディレクトリ レベルを区切るために使用する、

                'プラットフォーム固有の文字を提供します。


                aimPath += Path.DirectorySeparatorChar
            End If

            If (Not Directory.Exists(srcPath)) Then Exit Sub
            'Directory：ディレクトリやサブディレクトリを通じて、作成、移動、および列挙するための静的メソッドを公開します。

            'このクラスは継承できません。


            '2012.12.26 Exists：指定したパスがディスク上の既存のディレクトリを参照しているかどうかを確認します。

            'srcPath：現在開いているファイルのパス
            If (Not Directory.Exists(aimPath)) Then Directory.CreateDirectory(aimPath)
            '2012.12.26 CreateDirectory：path で指定したすべてのディレクトリとサブディレクトリを作成します。


           
            Dim fileList() As String = Directory.GetFileSystemEntries(srcPath)
            '2012.12.26　GetFileSystemEntries：指定したディレクトリ内のすべてのファイル名とサブディレクトリ名を返します。

            '★現在開いているフォルダの中にあるファイル・フォルダを「名前を付けて保存」で新しく作成したフォルダ（例：A）にコピー
            '★⇒「名前をつけて保存」で新しく作成したフォルダ（例：A）自身もコピーの対象になる⇒Aの中にAができる
            For Each FileName As String In fileList
                If Directory.Exists(FileName) Then

                    '2012.12.26=========================「名前を付けて保存」の無限ループ

                    YCM_CopyDir(FileName, aimPath + Path.GetFileName(FileName))
                    '⇒YCM_CopyDir自分自身の中身もコピー
                    '（現在は）当たったら、Tryに戻る

                    
                    'My.Computer.FileSystem.CopyDirectory(FileName, aimPath + Path.GetFileName(FileName)) '2012.12.26
                Else
                    File.Copy(FileName, aimPath + Path.GetFileName(FileName), True)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Public Sub YCM_DeleteDir(ByVal aimPath As String)
        Try
            If (aimPath(aimPath.Length - 1) <> Path.DirectorySeparatorChar) Then
                aimPath += Path.DirectorySeparatorChar
            End If
            If (Not Directory.Exists(aimPath)) Then Exit Sub
            Dim fileList() As String = Directory.GetFileSystemEntries(aimPath)
            For Each FileName As String In fileList
                If (Directory.Exists(FileName)) Then
                    YCM_DeleteDir(aimPath + Path.GetFileName(FileName))
                Else
                    File.Delete(aimPath + Path.GetFileName(FileName))
                End If
            Next
            System.IO.Directory.Delete(aimPath, True)
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub
    Public Sub YCM_CopyFile(ByVal srcPath As String, ByVal aimPath As String)
        If Dir(srcPath) <> "" Then
            FileCopy(srcPath, aimPath & "\計測データ.mdb")
        Else
            MsgBox("システムフォルダから計測データが見つかりませんので、新規できません", MsgBoxStyle.Critical)
        End If
    End Sub

    'SUURI ADD START 20140912
    Public Function YCM_GetFolderPath_fromFullPath(ByVal strPath As String) As String
        YCM_GetFolderPath_fromFullPath = IO.Path.GetDirectoryName(strPath)
    End Function
    'SUURI ADD END 20140912
End Module
