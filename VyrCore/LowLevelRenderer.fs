namespace VyrCore

open System

type ShaderType = 
    | VertexShader = 0
    | PixelShader = 1

/// A shader object used to link shader programs
type IShader =
    inherit IDisposable
    abstract Type : ShaderType

/// A shader program which can be used for actual draw calls
type IShaderProgram = 
    inherit IDisposable

type BufferUsage =
    | StaticDraw = 0
    | DynamicDraw = 1
    | StreamDraw = 2

type VertexAttribute =
    {
        // component count -> Vec2 = 2, Vec3 = 3
        Components : int
        // Type of vertex data
        DataType : DataType
        // How much byte do you need to get from one vertex to another (vertex! not component)
        Stride : int
        // Initial byte offset, usually 0
        Offset : int
    }

/// A vertex buffer consists of a vertex array object and a vertex buffer object
type IVertexBuffer =
    inherit IDisposable

/// A indexed vertex buffer is a composition from a normal vertex buffer and an additional element buffer
type IVertexBufferIndexed =
    inherit IDisposable
    abstract VertexBuffer : IVertexBuffer

/// This renderer has basic capabilities for drawing primitives.
type IRenderer =
    inherit IDisposable
    /// Start drawing functionalities
    abstract Begin : unit -> unit
    /// Clear the color buffer with a set color
    abstract Clear : unit -> unit
    /// Set the clear color buffer color
    abstract ClearColor : Color -> unit
    /// Draw an active vertex buffer, specifies the primitive type, start index and primitive count
    abstract DrawVertexBuffer : PrimitiveType -> int -> int -> unit
    /// Draw an active indiced vertex buffer, specifies the primitive type, index count
    abstract DrawVertexBufferIndexed : PrimitiveType -> int -> unit
    /// Stop drawing functionalities
    abstract End : unit -> unit
    /// Sets the shader program active for rendering
    abstract UseShader : IShaderProgram -> unit
    /// Sets the vertex buffer active for rendering
    abstract UseVertexBuffer : IVertexBuffer -> unit
    /// Swap the buffer and present it to the screen
    abstract SwapBuffers : unit -> unit
    /// Sets the viewport for this rendering context with the lower left position, width and height
    abstract Viewport : Vec2<int16> -> Size<int16> -> unit

/// Abstract factory which creates objects according to a specified api
type IGraphics =
    /// Creates a renderer for a window. The window is specified by a handle.
    abstract CreateRenderer : nativeint -> IRenderer
    /// Creates a shader of a certain type and the source code
    abstract CreateShader : ShaderType -> string -> Result<IShader, string>
    /// Creates a shader program from a sequence of shaders
    abstract CreateShaderProgram : IShader seq -> Result<IShaderProgram, string>
    /// Creates a vertex buffer from an array the buffer usage and an attribute describing parameters to draw this buffer
    abstract CreateVertexBuffer<'T when 'T : struct and 'T :> System.ValueType and 'T : (new : unit -> 'T)> : 'T array -> BufferUsage -> VertexAttribute  seq -> IVertexBuffer
    /// Creates a vertex buffer from an array, indices, the buffer usage and an attribute describing parameters to draw this buffer
    abstract CreateVertexBufferIndexed<'T, 'U when 'T : struct and 'T :> System.ValueType and 'T : (new : unit -> 'T) and 'U : struct and 'U :> System.ValueType and 'U : (new : unit -> 'U)> : 'T array -> 'U array -> BufferUsage -> VertexAttribute seq -> IVertexBufferIndexed