namespace VyrViewModel

open GalaSoft.MvvmLight
open GalaSoft.MvvmLight.Ioc
open GalaSoft.MvvmLight.Command
open Microsoft.Practices.ServiceLocation

open VyrEngine
open System.Windows.Input

/// The ViewModel for the main window.
type MainWindowVM() = 
    inherit ViewModelBase()
    member this.InitializeEngineCommand = 
        RelayCommand(fun () -> 
            System.Windows.MessageBox.Show("Jay212..") |> ignore
            System.Diagnostics.Debug.WriteLine("hmm..212")
            Engine.initialize |> ignore) :> ICommand
    /// Initializes the graphics context for the window. Puts the parent control as the parameter.
    member this.InitializeGraphicsCommand : ICommand = 
        RelayCommand<obj>(fun (sender:obj) ->
            System.Windows.MessageBox.Show("Jay..") |> ignore
            System.Diagnostics.Trace.WriteLine("hmm..1")
            let controlArgs = {ControlArgs.Sender = sender}
            (Engine.initializeSurface 0 (SurfaceMode.Control controlArgs) 0) |> ignore) :> ICommand

/// The ViewModelLocator is used by the Views to initialize their individual DataContext
type ViewModelLocator() =
    do
        // set the locator provider to the SimpleIoc
        let serviceLocator = (fun () -> SimpleIoc.Default :> IServiceLocator) 
        ServiceLocator.SetLocatorProvider(ServiceLocatorProvider serviceLocator)
        (* Not used here, cause no data service is needed yet (care c# version)
        if (ViewModelBase.IsInDesignModeStatic)
            SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
        else
            SimpleIoc.Default.Register<IDataService, DataService>();         
        *)
        SimpleIoc.Default.Register<MainWindowVM>()
    /// Return an instance of a new MainWindowVM
    member this.Main = SimpleIoc.Default.GetInstance<MainWindowVM>()

