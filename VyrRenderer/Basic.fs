namespace VyrRenderer

open OpenTK
open OpenTK.Graphics

#if _WIN32
open OpenTK.Graphics.OpenGL
#endif

[<AutoOpen>]
module Basic =
    /// Converts a VyrCore.DataType to OpenTK Vertex Attribute
    let internal toAttribPointer t =
        match t with
        | VyrCore.DataType.Double -> VertexAttribPointerType.Double
        | VyrCore.DataType.Int -> VertexAttribPointerType.Int
        | VyrCore.DataType.Short -> VertexAttribPointerType.Short
        | VyrCore.DataType.Single -> VertexAttribPointerType.Float
        | VyrCore.DataType.UnsignedInt -> VertexAttribPointerType.UnsignedInt
        | _ -> failwith "Not yet implemented"

    /// Converts a VyrCore.Primitive to a OpenTK Primitive
    let internal toPrimitiveMode t =
        match t with
        | VyrCore.Graphics.PrimitiveType.Triangles -> PrimitiveType.Triangles
        | _ -> failwith "Not yet implemented"

    /// Converts VyrCore.BufferUsage to a OpenTK BufferUsageHint
    let internal toBufferUsage t =
        match t with
        | VyrCore.Graphics.BufferUsage.StaticDraw -> BufferUsageHint.StaticDraw
        | VyrCore.Graphics.BufferUsage.DynamicDraw -> BufferUsageHint.DynamicDraw
        | VyrCore.Graphics.BufferUsage.StreamDraw -> BufferUsageHint.StreamDraw
        | _ -> failwith "Not yet implemented"

    /// Converts a VyrCore.ShaderType to a OpenTK ShaderType
    let internal toShaderType t =
        match t with
        | VyrCore.Graphics.ShaderType.VertexShader -> ShaderType.VertexShader
        | VyrCore.Graphics.ShaderType.PixelShader -> ShaderType.FragmentShader
        | _ -> failwith "Not yet implemented"

    /// Converts a wrap mode to a OpenTK WrapMode
    let internal toWrapMode t =
        match t with
        | VyrCore.Graphics.TextureWrapMode.Repeat -> TextureWrapMode.Repeat
        | VyrCore.Graphics.TextureWrapMode.MirroredRepeat -> TextureWrapMode.MirroredRepeat
        | VyrCore.Graphics.TextureWrapMode.ClampBorder -> TextureWrapMode.ClampToBorder
        | VyrCore.Graphics.TextureWrapMode.ClampEdge -> TextureWrapMode.ClampToEdge
        | _ -> failwith "Not yet implemented"

    /// Converts a filter mode to a OpenTK Min Filter
    let internal toMinFilterMode t =
        match t with 
        | VyrCore.Graphics.TextureMinifyingFilter.Nearest -> TextureMinFilter.Nearest
        | VyrCore.Graphics.TextureMinifyingFilter.Linear -> TextureMinFilter.Linear 
        | _ -> failwith "Not yet implemented"

    /// Converts a filter mode to a OpenTK Mag Filter
    let internal toMagFilterMode t =
        match t with 
        | VyrCore.Graphics.TextureMagnifyingFilter.Nearest -> TextureMagFilter.Nearest
        | VyrCore.Graphics.TextureMagnifyingFilter.Linear -> TextureMagFilter.Linear
        | _ -> failwith "Not yet implemented"        

    /// Converts a target format to OpenTK pixel internal format
    let internal toPixelInternalFormat t =
        match t with
        | VyrCore.Graphics.TextureTargetFormat.RGB8 -> PixelInternalFormat.Rgb8
        | VyrCore.Graphics.TextureTargetFormat.RGBA8 -> PixelInternalFormat.Rgba8
        | VyrCore.Graphics.TextureTargetFormat.R8 -> PixelInternalFormat.R8
        | VyrCore.Graphics.TextureTargetFormat.Depth -> PixelInternalFormat.DepthComponent
        | VyrCore.Graphics.TextureTargetFormat.DepthStencil -> PixelInternalFormat.DepthStencil
        | VyrCore.Graphics.TextureTargetFormat.SRGB8 -> PixelInternalFormat.Srgb8
        | VyrCore.Graphics.TextureTargetFormat.SRGBA8 -> PixelInternalFormat.Srgb8Alpha8
        | _ -> failwith "Format not supported"

    /// Converts a original format to a pixel format
    let internal toPixelFormat t =
        match t with
        | VyrCore.Graphics.TextureOriginalFormat.RGB -> PixelFormat.Rgb
        | VyrCore.Graphics.TextureOriginalFormat.RGBA -> PixelFormat.Rgba
        | VyrCore.Graphics.TextureOriginalFormat.R -> PixelFormat.Red
        | VyrCore.Graphics.TextureOriginalFormat.Depth -> PixelFormat.DepthComponent
        | VyrCore.Graphics.TextureOriginalFormat.DepthStencil -> PixelFormat.DepthStencil
        | VyrCore.Graphics.TextureOriginalFormat.BGR -> PixelFormat.Bgr
        | VyrCore.Graphics.TextureOriginalFormat.BGRA -> PixelFormat.Bgra
        | _ -> failwith "Format not supported"

    /// Converts an index to an instance of a texture unit enum
    let toTextureUnit (i:int) = (int TextureUnit.Texture0) |> (+) i |> enum<TextureUnit>

    /// Returns a sequence to all possible texture unit enums
    let textureUnitsSeq = seq {for i in 0..System.Enum.GetValues(typeof<TextureUnit>).Length do yield toTextureUnit i} |> Seq.cache