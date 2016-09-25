namespace VyrEngine

open VyrCore
open VyrCore.Graphics

module Engine =

#if _WIN32

    let initializeGraphics renderingAPI =
        match renderingAPI with
        | RenderingAPI.OpenGL -> VyrRenderer.GLGraphics() :> IGraphics
        | _ -> failwith "Not yet implemented"

#endif

// TODO:: Other platforms

#if __ANDROID__
        /// Initializes a new graphics window (param data is inside engineMode) (probably use OpenTK-Window? or Glew??)
    let initializeWindow renderingAPI = failwith "Not yet implemented"

    let initializeControl renderingAPI = failwith "Not yet implemented"
#endif