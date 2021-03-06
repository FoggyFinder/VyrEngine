﻿namespace VyrRenderer

#nowarn "9"

open FSharp.NativeInterop
open VyrCore
open VyrCore.Graphics
open OpenTK

#if _WIN32
open OpenTK.Graphics.OpenGL
#endif

            
/// Initializes a OpenGL-Renderer based on a window handle.
type GLRenderer(windowHandle : nativeint) =
    inherit Disposable()
#if _WIN32
    // initialize opengl context
    let windowInfo = OpenTK.Platform.Utilities.CreateWindowsWindowInfo windowHandle
    let context = new OpenTK.Graphics.GraphicsContext(Graphics.GraphicsMode.Default, windowInfo)
    do
        context.MakeCurrent(windowInfo)
        context.LoadAll()

    override this.OnDispose() = context.Dispose()
#endif
    interface IRenderer with
        /// Begins rendering by setting the context to current window info. This is important to ensure renderin from concurrent threads.
        member this.Begin() = context.MakeCurrent(windowInfo)
        /// Clears the color buffer
        member this.Clear() = GL.Clear(ClearBufferMask.ColorBufferBit)
        /// Sets the clear color
        member this.ClearColor(color) = GL.ClearColor(color.R, color.G, color.B, color.A)
        member this.DrawVertexBuffer primitiveType startIndex vertexCount = GL.DrawArrays(toPrimitiveMode primitiveType, startIndex, vertexCount)
        /// Draw an active indiced vertex buffer, specifies the primitive type, index count
        member this.DrawVertexBufferIndexed primitiveType indexCount = GL.DrawElements(toPrimitiveMode primitiveType, indexCount, DrawElementsType.UnsignedInt, 0)
        /// Ends the rendering and resets the window handle context
        member this.End() = context.MakeCurrent(null)
        /// Activates srgb framebuffers
        member this.UseSRGBFramebuffer() = GL.Enable(EnableCap.FramebufferSrgb)
        /// Activates the program for rendering
        member this.UseShader program = 
            let p = program :?> GLShaderProgram
            GL.UseProgram(p.Program)
        /// Set one texture active
        member this.UseTexture t =
            let tex = t :?> Texture2D
            GL.BindTexture(TextureTarget.Texture2D, tex.ID)
        /// Sets multiple textures active
        member this.UseTextures ts = ts |> Seq.iteri2 (fun i tu t -> GL.ActiveTexture(tu); GL.BindTexture(TextureTarget.Texture2D, (t :?> Texture2D).ID)) textureUnitsSeq 
        /// Activates a vertex buffer for rendering
        member this.UseVertexBuffer vb = 
            let v = vb :?> GLVertexBuffer
            GL.BindVertexArray(v.VAO)
        /// Swaps the renderered buffer to the window
        member this.SwapBuffers() = context.SwapBuffers()
        /// Sets the viewport to the given parameters
        member this.Viewport pos size = GL.Viewport(int pos.X, int pos.Y, int size.Width, int size.Height)

type GLGraphics() = 
    interface IGraphics with
        /// Creates a renderer based on a window handle.
        member this.CreateRenderer(windowHandle) = new GLRenderer(windowHandle) :> IRenderer
        /// Creates a shader of a certain type
        member this.CreateShader shaderType source = Shader.createShader shaderType source
        /// Creates a shader program by a sequence of shaders
        member this.CreateShaderProgram shader = Shader.createShaderProgram shader
        member this.CreateTexture2D settings path = Texture.createTex2D settings path
        /// Creates a vertex buffer
        member this.CreateVertexBuffer vertices bufferUsage attribute = VertexBuffer.createVertexBuffer vertices bufferUsage attribute :> IVertexBuffer 
        /// Creates a indexed vertex buffer
        member this.CreateVertexBufferIndexed vertices indices bufferUsage attribute = VertexBuffer.createVertexBufferIndexed vertices indices bufferUsage attribute :> IVertexBufferIndexed