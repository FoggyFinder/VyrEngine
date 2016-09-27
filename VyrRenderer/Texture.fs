namespace VyrRenderer

open VyrCore
open VyrCore.Graphics
open OpenTK

#if _WIN32
open OpenTK.Graphics.OpenGL
#endif

type Texture2D =
    {
        ID : int
        Settings : TextureSettings2D
        Width : uint16
        Height : uint16
    }
    interface ITexture2D with
        member this.TextureSettings = this.Settings
        member this.Width = this.Width
        member this.Height = this.Height
        member this.Dispose() = GL.DeleteTexture(this.ID)

module Texture =
    /// activates tex settings for a currently set texture and indicates if the texture uses mip maps
    let useTexSettings settings mip =
            // convert wrap modes for s and t axis, convert filters
            let s = toWrapMode settings.WrapMode_S
            let t = toWrapMode settings.WrapMode_T
            // apply mip map filter for downscaled textures only
            match mip with
            | true -> 
                let minFilter = match settings.MinifyingFilter with TextureMinifyingFilter.Nearest -> TextureMinFilter.NearestMipmapNearest | _ -> TextureMinFilter.LinearMipmapLinear
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, int minFilter)
            | false ->
                let minFilter = toMinFilterMode settings.MinifyingFilter
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, int minFilter)
            // use normal specified magnifying filter
            let magFilter = toMagFilterMode settings.MagnifyingFilter
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, int magFilter)
            // apply parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, int s)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, int t)
            /// set border color
            match settings.BorderColor with
            | Just color -> GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, toColorArray color)
            | _ -> GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, [|0.f; 0.f; 0.f; 0.f|])
    /// Creates a texture from settings, width, height and a pointer to the data [byte array].
    let createTex2DPtr settings width height (data:nativeint) =
            // generate opengl texture, bind data
            let internalFormat = toPixelInternalFormat settings.TargetFormat
            let originalFormat = toPixelFormat settings.OriginalFormat
            let tex = GL.GenTexture()
            GL.BindTexture(TextureTarget.Texture2D, tex)
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, originalFormat, PixelType.UnsignedByte, data)
            // if texture is not NPOT generate mipmaps
            let mip = match Math.isPowerOf2 width, Math.isPowerOf2 height with true, true -> GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); true | _ -> false
            useTexSettings settings mip
            GL.BindTexture(TextureTarget.Texture2D, 0)
            { Texture2D.ID = tex; Texture2D.Settings = settings; Texture2D.Width = uint16 width; Texture2D.Height = uint16 height}
    /// Creates a texture from a source and applies settings
    let createTex2D settings (source:string) =
        try
            // load image based on different platforms
            #if _WIN32
            // load image
            use img = new System.Drawing.Bitmap(source, true)
            let width, height = img.Width, img.Height
            let rectangle = System.Drawing.Rectangle(0, 0, width, height)
            // read image data bits and create opengl texture
            let data = img.LockBits(rectangle, System.Drawing.Imaging.ImageLockMode.ReadOnly, img.PixelFormat)
            let tex = createTex2DPtr settings width height data.Scan0
            img.UnlockBits(data)
            // return result
            Result<ITexture2D, string>.Success tex
            #endif
        with
        | _ -> Result<ITexture2D, string>.Error "Failed to load the image."
