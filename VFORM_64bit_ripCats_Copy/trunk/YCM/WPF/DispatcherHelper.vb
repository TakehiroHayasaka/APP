Public Class DispatcherHelper

    Public Shared Sub DoEvents()
        Dim frame As New System.Windows.Threading.DispatcherFrame
        System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, _
                                                                          New System.Windows.Threading.DispatcherOperationCallback(AddressOf ExitFrames), _
                                                                          frame)
        Try
            System.Windows.Threading.Dispatcher.PushFrame(frame)
        Catch ex As InvalidOperationException
        End Try
    End Sub

    Private Shared Function ExitFrames(ByVal frame As Object) As Object
        Dim f As System.Windows.Threading.DispatcherFrame = frame
        f.Continue = False
        Return Nothing
    End Function
End Class
