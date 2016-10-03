namespace VyrCore.Graphics

open VyrCore
open System

(***************** Shader Types *******************)

/// Describes the shader type used for a shader.
type ShaderType = 
    | VertexShader = 0
    | PixelShader = 1

/// Describes the shader source and type.
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
    abstract GetUniform<'a> : string -> IShaderUniform<'a>
    /// Returns a value which represents a uniform object
    abstract GetUniformValue<'a> : IShaderUniform<'a> -> Maybe<'a>
    /// Sets the value for a uniform object.
    abstract SetUniform<'a> : IShaderUniform<'a>-> 'a -> unit

(***************** Vertex Types *******************)

/// The primitive type to be drawn for vertices of a vertex buffer.
type PrimitiveType = 
    | Triangles = 0

/// Describes a vertex only by position.
[<Struct>]
type VertexPosition<'a>(pos:Vec3<'a>) =
    member this.Position = pos

/// Describes a vertex by position and vertex color.
[<Struct>]
type VertexPositionColor<'a, 'b>(pos:Vec3<'a>, color:Color<'b>) =
    member this.Position = pos
    member this.Color = color

/// Describes a vertex by position and texture coordinates.
[<Struct>]
type VertexPosTex<'a, 'b>(pos:Vec3<'a>, tex:Vec2<'b>) =
    member this.Position = pos
    member this.Texture = tex

(***************** Vertex Buffer Types *******************)

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

/// A vertex buffer consists of a vertex array object and a vertex buffer object
type IVertexBuffer =
    inherit IDisposable

/// A indexed vertex buffer is a composition from a normal vertex buffer and an additional element buffer
type IVertexBufferIndexed =
    inherit IDisposable
    abstract VertexBuffer : IVertexBuffer

(***************** Texture Types *******************)

/// Wrapping modes are used for coordinates below 0 and greater 1.
type TextureWrapMode =
    | Repeat = 0
    | MirroredRepeat = 1
    | ClampEdge = 2
    | ClampBorder = 3

/// This Filter mode is used when a texture is downscaled. Care MIP filter don't work when the bound texture is NPOT.
type TextureMinifyingFilter =
    | Nearest = 0
    | Linear = 1

/// This Filter mode is used when a texture is upscaled
type TextureMagnifyingFilter = 
    | Nearest = 0
    | Linear = 1

/// The target format for the texture. For diffuse textures mostly SRGB/SRGBA and other texture maps RGB/RGBA
type TextureTargetFormat =
    | RGBA8 = 0
    | RGB8 = 1
    | R8 = 2
    | Depth = 3
    | DepthStencil = 4
    | SRGBA8 = 5
    | SRGB8 = 6

/// The original format of the texture.
type TextureOriginalFormat =
    | RGB = 0
    | RGBA = 1
    | R = 2
    | Depth = 3
    | DepthStencil = 4
    | BGR = 5
    | BGRA = 6

/// Describes the wrapping mode for each axis.
type TextureSettings2D =
    {
        /// Defines the target format used by the graphics api
        TargetFormat : TextureTargetFormat
        /// Defines the format which was originally used for the texture data
        OriginalFormat : TextureOriginalFormat
        /// Defines the wrap mode for the s axis
        WrapMode_S : TextureWrapMode
        /// Defines the wrap mode for the t axis
        WrapMode_T : TextureWrapMode
        /// Defines the filter applied when a texture is downscaled
        MinifyingFilter : TextureMinifyingFilter
        /// Defines the filter applied when a texture is upscaled
        MagnifyingFilter : TextureMagnifyingFilter
        /// Defines a color for the border when border wrapping mode is used
        BorderColor : Maybe<Color<float32>>
    }

/// Defines a basic texture with texture settings.
type ITexture2D = 
    inherit IDisposable
    /// Returns the texture settings used for the texture
    abstract TextureSettings : TextureSettings2D
    /// Returns the width of the texture
    abstract Width : uint16
    /// Returns the height of the texture
    abstract Height : uint16

(***************** Renderer Types *******************)

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
    /// Sets the renderer to use srgb framebuffer.
    abstract UseSRGBFramebuffer : unit -> unit
    /// Sets the shader program active for rendering
    abstract UseShader : IShaderProgram -> unit
    /// Sets the texture active for rendering
    abstract UseTexture : ITexture2D -> unit
    /// Sets multiple textures active for rendering
    abstract UseTextures : ITexture2D seq -> unit
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
    /// Creates a texture from texture settings and a string containing the file path
    abstract CreateTexture2D : TextureSettings2D -> string -> Result<ITexture2D, string>
    /// Creates a vertex buffer from an array the buffer usage and an attribute describing parameters to draw this buffer
    abstract CreateVertexBuffer<'T when 'T : struct and 'T :> System.ValueType and 'T : (new : unit -> 'T)> : 'T array -> BufferUsage -> VertexAttribute  seq -> IVertexBuffer
    /// Creates a vertex buffer from an array, indices, the buffer usage and an attribute describing parameters to draw this buffer
    abstract CreateVertexBufferIndexed<'T, 'U when 'T : struct and 'T :> System.ValueType and 'T : (new : unit -> 'T) and 'U : struct and 'U :> System.ValueType and 'U : (new : unit -> 'U)> : 'T array -> 'U array -> BufferUsage -> VertexAttribute seq -> IVertexBufferIndexed
  
[<AutoOpen>]
module RendererHelper =
    /// Creates a vertex attribute for the type VertexPosition
    let inline attributeVertexPosition<'a> =
        maybe{
            let! data = dataType<'a>
            let components = 3
            let stride = components * sizeof<'a>
            let attribute = {Components = 3; DataType = data; Stride = stride;  Offset = 0}
            return [|attribute|]
        }
    /// Creates a vertex attribute for the type VertexPositionColor
    let inline attributeVertexPositionColor<'a, 'b> =
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
    /// Creates a vertex attribute for the type VertexPosTex
    let inline attributeVertexPositionTexture<'a, 'b> =
        maybe{
            let! positionData = dataType<'a>
            let! texData = dataType<'b>
            let positionComponents, texComponents = 3, 2
            let stride = positionComponents * sizeof<'a> + texComponents * sizeof<'b>
            let offset = positionComponents * sizeof<'a>
            let positionAttribute = {Components = positionComponents; DataType = positionData; Stride = stride;  Offset = 0}
            let colorAttribute = {Components = texComponents; DataType = texData; Stride = stride; Offset = offset }
            return [|positionAttribute; colorAttribute|]
        }
    /// Creates a shader program from a list of shader sources
    let inline createShaderProgram (graphics:IGraphics) (shaderSources:ShaderSource seq) =
        result {
            //let! s = result {for s in shaderSources do let! shader = (graphics.CreateShader s.Type s.Source) in yield shader} // create IShader from sources [the for loop makes sure to dispose everything when the loop fails(error result)
            let! s = result {for s in shaderSources do yield! (graphics.CreateShader s.Type s.Source)}
            let! shaderProgram = 
                let p = graphics.CreateShaderProgram s // create program
                s |> Seq.iter (fun x -> x.Dispose()) // dispose used IShader
                p
            return shaderProgram
        }