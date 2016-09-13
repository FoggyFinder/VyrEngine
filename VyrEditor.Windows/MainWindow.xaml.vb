Namespace VyrEditor.Windows
    Partial Public Class MainWindow
        Inherits System.Windows.Window
        Public Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
            AddHandler CompositionTarget.Rendering, Sub(s, eArgs)
                                                        Dim context As VyrVM.MainViewModel = DataContext
                                                        context.UpdateSurface.Execute(Nothing)
                                                    End Sub
        End Sub
    End Class
End Namespace

