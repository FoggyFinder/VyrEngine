namespace VyrCore.Graphics

open VyrCore
open System

type PrimitiveType = 
    | Triangles = 0

type ShaderType = 
    | VertexShader = 0
    | PixelShader = 1

type ShaderSource =
    {
        Type : ShaderType
        Source : string
    }

/// A shader object used to link shader programs
type IShader =
    inherit IDisposable
    abstract Type : ShaderType

/// A shader uniform in order to update shaders with custom values
type IShaderUniform<'a> = interface end

/// A shader program which can be used for actual draw calls
type IShaderProgram = 
    inherit IDisposable
    /// Returns a uniform object which can be used for specifying shader data; this uniform is bound to a specific type.
    abstract GetUniform<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)> : string -> IShaderUniform<'a>
    /// Returns a value which represents a uniform object
    abstract GetUniformValue<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)> : IShaderUniform<'a> -> Maybe<'a>
    /// Sets the value for a uniform object.
    abstract SetUniform<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)> : IShaderUniform<'a>-> 'a -> unit

/// Describes the usage for the vertex buffer
type BufferUsage =
    | StaticDraw = 0
    | DynamicDraw = 1
    | StreamDraw = 2

/// A vertex attribute describing the layout for the vertices in shaders
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

[<Struct>]
type VertexPosition<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)>(pos:Vec3<'a>) =
    member this.Position = pos

[<Struct>]
type VertexPositionColor<'a, 'b when 
    'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a) and 
    'b : struct and 'b :> System.ValueType and 'b : (new : unit -> 'b)>(pos:Vec3<'a>, color:Color<'b>) =
    member this.Position = pos
    member this.Color = color

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
    abstract ClearColor : Color<float32> -> unit
    /// Draw an active vertex buffer, specifies the primitive type, start index and primitive count
    abstract DrawVertexBuffer : PrimitiveType -> int -> int -> unit
    /// Draw an active indiced vertex buffer, specifies the primitive type, index count
    abstract DrawVertexBufferIndexed : PrimitiveType -> int -> unit
    /// Stop all functionalities
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
  
[<AutoOpen>]
module RendererHelper =
    /// Creates a vertex attribute for the type VertexPosition
    let attributeVertexPosition<'a> =
        maybe{
            let! data = dataType<'a>
            let components = 3
            let stride = components * sizeof<'a>
            let attribute = {Components = 3; DataType = data; Stride = stride;  Offset = 0}
            return [|attribute|]
        }

    /// Creates a vertex attribute for the type VertexPositionColor
    let attributeVertexPositionColor<'a, 'b> =
        maybe{
            let! positionData = dataType<'a>
            let! colorData = dataType<'b>
            let positionComponents, colorComponents = 3, 4
            let stride = positionComponents * sizeof<'a> + colorComponents * sizeof<'b>
            let offset = positionComponents * sizeof<'a>
            let positionAttribute = {Components = positionComponents; DataType = positionData; Stride = stride;  Offset = 0}
            let colorAttribute = {Components = colorComponents; DataType = colorData; Stride = stride; Offset = offset }
            return [|positionAttribute; colorAttribute|]
        }

    /// Creates a shader program from a list of shader sources
    let createShaderProgram (graphics:IGraphics) (shaderSources:ShaderSource seq) =
        result {
            //let! s = result {for s in shaderSources do let! shader = (graphics.CreateShader s.Type s.Source) in yield shader} // create IShader from sources [the for loop makes sure to dispose everything when the loop fails(error result)
            let! s = result {for s in shaderSources do yield! (graphics.CreateShader s.Type s.Source)}
            let! shaderProgram = 
                let p = graphics.CreateShaderProgram s // create program
                s |> Seq.iter (fun x -> x.Dispose()) // dispose used IShader
                p
            return shaderProgram
        }