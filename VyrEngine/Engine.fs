namespace VyrEngine

module Engine =

#if _WIN32
    /// Initializes a new graphics window (param data is inside engineMode) (probably use OpenTK-Window? or Glew??)
    let private initializeWindow = 0

    /// Initializes a new graphics control which is parented to a host control. (param data is inside engine mode) (use OpenTK-GLControl)
    let private initializeControl (parent:obj) = 1
#endif

#if __ANDROID__
    let private initializeWindow = failwith "Not implemented yet" 
#endif

    /// <summary> Initializes the VyrEngine. </summary>
    let initialize = 0

    /// <summary> Creates a new window. </summary>
    /// <param name="surfaceMode"> The Mode in which the surface is initialized.(Either a new Window or Control)</param>
    let initializeSurface engineMode surfaceMode graphicsAPI = 
        match surfaceMode with
        | SurfaceMode.Control(args) -> initializeControl args.Sender
        | SurfaceMode.Window -> initializeWindow

    