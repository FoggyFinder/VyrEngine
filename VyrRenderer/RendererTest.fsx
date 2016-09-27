#r @"F:\Projects\VyrDevelopment\packages\OpenTK.2.0.0\lib\net20\OpenTK.dll"

open OpenTK
open OpenTK.Graphics.OpenGL

let nextTextureUnitEnum (i:int) = (int TextureUnit.Texture0) |> (+) i |> enum<TextureUnit>

let textureEnums = seq {for i in 0..50 do yield nextTextureUnitEnum i}

printfn "%A" (textureEnums |> Seq.toList)