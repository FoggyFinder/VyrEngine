namespace VyrEngine

//type EditorArgs = {}

/// The Main Mode in which the engine runs in. 
type EngineMode = 
    | Editor// of  
    | Runtime

type ControlArgs = {Sender:obj}

/// The Surface Mode in which the graphics are drawn.
type SurfaceMode =
    | Window
    | Control of ControlArgs

/// The utilized rendering api used for the graphics engine.
type RenderingAPI = OpenGL | OpenGL_ES | WebGL | Vulkan