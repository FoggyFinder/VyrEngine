namespace VyrRenderer

open VyrCore
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
        | VyrCore.PrimitiveType.Triangles -> BeginMode.Triangles
        | _ -> failwith "Not yet implemented"

    /// Converts VyrCore.BufferUsage to a OpenTK BufferUsageHint
    let internal toBufferUsage t =
        match t with
        | VyrCore.BufferUsage.StaticDraw -> BufferUsageHint.StaticDraw
        | VyrCore.BufferUsage.DynamicDraw -> BufferUsageHint.DynamicDraw
        | VyrCore.BufferUsage.StreamDraw -> BufferUsageHint.StreamDraw
        | _ -> failwith "Not yet implemented"

    /// Converts a VyrCore.ShaderType to a OpenTK ShaderType
    let internal toShaderType t =
        match t with
        | VyrCore.ShaderType.VertexShader -> ShaderType.VertexShader
        | VyrCore.ShaderType.PixelShader -> ShaderType.FragmentShader
        | _ -> failwith "Not yet implemented"

