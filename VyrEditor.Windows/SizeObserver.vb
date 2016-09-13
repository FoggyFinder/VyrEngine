Public Class SizeObserver
    Public Shared ObserveProperty As DependencyProperty = DependencyProperty.RegisterAttached(
        "Observe",
        GetType(Boolean),
        GetType(SizeObserver),
        New FrameworkPropertyMetadata(AddressOf OnObserveChanged))

    Public Shared ObservedWidthProperty As DependencyProperty = DependencyProperty.RegisterAttached(
        "ObservedWidth",
        GetType(Double),
        GetType(SizeObserver))

    Public Shared ObservedHeightProperty As DependencyProperty = DependencyProperty.RegisterAttached(
        "ObservedHeight",
        GetType(Double),
        GetType(SizeObserver))

    Shared Function GetObserve(frameworkElement As FrameworkElement) As Boolean
        Return frameworkElement.GetValue(ObserveProperty)
    End Function

    Shared Sub SetObserve(frameworkElement As FrameworkElement, observe As Boolean)
        frameworkElement.SetValue(ObserveProperty, observe)
    End Sub

    Shared Function GetObservedWidth(frameworkElement As FrameworkElement) As Double
        Return frameworkElement.GetValue(ObservedWidthProperty)
    End Function

    Shared Sub SetObservedWidth(frameworkElement As FrameworkElement, width As Double)
        frameworkElement.SetValue(ObservedWidthProperty, width)
    End Sub

    Shared Function GetObservedHeight(frameworkElement As FrameworkElement) As Double
        Return frameworkElement.GetValue(ObservedHeightProperty)
    End Function

    Shared Sub SetObservedHeight(frameworkElement As FrameworkElement, height As Double)
        frameworkElement.SetValue(ObservedHeightProperty, height)
    End Sub

    Private Shared Sub OnObserveChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim f As FrameworkElement = d
        Dim b As Boolean = e.NewValue
        If (b) Then
            AddHandler f.SizeChanged, AddressOf OnUpdate
            Update(f)
        Else
            RemoveHandler f.SizeChanged, AddressOf OnUpdate
        End If
    End Sub

    Private Shared Sub OnUpdate(sender, e)
        Update(sender)
    End Sub

    Private Shared Sub Update(frameworkElement As FrameworkElement)
        frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth)
        frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight)
    End Sub
End Class

