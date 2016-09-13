namespace VyrRenderer

open VyrCore
open OpenTK
open OpenTK.Graphics
open OpenTK.Graphics.OpenGL

[<AutoOpen>]
module Basic =
    /// Converts a VyrCore.DataType to OpenTK Vertex Attribute
    let toAttribPointer t =
        match t with
        | VyrCore.DataType.Double -> VertexAttribPointerType.Double
        | VyrCore.DataType.Int -> VertexAttribPointerType.Int
        | VyrCore.DataType.Short -> VertexAttribPointerType.Short
        | VyrCore.DataType.Single -> VertexAttribPointerType.Float
        | _ -> failwith "Not yet implemented"

    /// Converts a VyrCore.Primitive to a OpenTK Primitive
    let toPrimitiveMode t =
        match t with
        | VyrCore.PrimitiveType.Triangles -> BeginMode.Triangles
        | _ -> failwith "Not yet implemented"

    let toBufferUsage t =
        match t with
        | VyrCore.BufferUsage.StaticDraw -> BufferUsageHint.StaticDraw
        | VyrCore.BufferUsage.DynamicDraw -> BufferUsageHint.DynamicDraw
        | VyrCore.BufferUsage.StreamDraw -> BufferUsageHint.StreamDraw
        | _ -> failwith "Not yet implemented"

    let toShaderType t =
        match t with
        | VyrCore.ShaderType.VertexShader -> ShaderType.VertexShader
        | VyrCore.ShaderType.PixelShader -> ShaderType.FragmentShader
        | _ -> failwith "Not yet implemented"

