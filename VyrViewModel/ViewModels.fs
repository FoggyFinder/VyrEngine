namespace VyrViewModel

open GalaSoft.MvvmLight
open GalaSoft.MvvmLight.Ioc
open GalaSoft.MvvmLight.CommandWpf
open Microsoft.Practices.ServiceLocation

open VyrEngine
open System
open System.Windows.Input

/// The ViewModel for the main window.
type MainWindowVM() = 
    inherit ViewModelBase()
    let test() = System.Windows.MessageBox.Show("hm") |> ignore
    /// Function to initialize the engine.
    let initializeEngine() = 
        System.Diagnostics.Debug.WriteLine("Initialize Engine..")
        Engine.initialize |> ignore
    /// Function to initialize the graphics control.
    let initializeControl sender = 
        System.Diagnostics.Debug.WriteLine("Initialize Surface..")
        let controlArgs = {ControlArgs.Sender = sender}
        Engine.initializeSurface 0 (SurfaceMode.Control controlArgs) 1 |> ignore
    /// Initializes the engine subsystems.
    member this.InitializeEngineCommand with get() = new RelayCommand(new System.Action(test))
    //member this.InitializeEngineCommand = new RelayCommand(new Action(initializeEngine))
    /// Initializes the graphics context for the window. Puts the parent control as the parameter.
    //member this.InitializeGraphicsCommand = RelayCommand<obj>(Action<obj>(initializeControl)) :> ICommand

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

