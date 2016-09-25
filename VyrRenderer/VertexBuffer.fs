namespace VyrRenderer

open VyrCore
open VyrCore.Graphics
open OpenTK

#if _WIN32
open OpenTK.Graphics.OpenGL
#endif

type GLVertexBuffer =
    {
        VAO : int
        Buffer : int
    }
    interface IVertexBuffer with
        member this.Dispose() = 
            GL.DeleteVertexArray(this.VAO)
            GL.DeleteBuffer(this.Buffer)

type GLVertexBufferIndexed = 
    {
        VertexBuffer : GLVertexBuffer
        EBO : int
    }
    interface IVertexBufferIndexed with
        member this.Dispose() = 
            (this.VertexBuffer :> System.IDisposable).Dispose()
            GL.DeleteBuffer(this.EBO)
        member this.VertexBuffer = (this.VertexBuffer :> IVertexBuffer)

module VertexBuffer = 
    /// Sets the buffer data for a vertex buffer object and sets the first attribute
    let private setBufferData (buffer:int) (vertices:'T array) bufferUsage attribute =
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer) // bind buffer
        let size = nativeint ((Array.length vertices) * sizeof<'T>)
        GL.BufferData(BufferTarget.ArrayBuffer, size, vertices, bufferUsage) // set the vertex data
        // set the opengl attributes (offsets, components, strides etc.)
        attribute
        |> Seq.iteri
            (fun i attrib -> 
                GL.EnableVertexAttribArray(i)
                let attribType = toAttribPointer attrib.DataType
                GL.VertexAttribPointer(i, attrib.Components, attribType, false, attrib.Stride, attrib.Offset))
    /// Sets the element buffer data for an index array
    let private setEBOData (ebo:int) (indices:'T array) bufferUsage =
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo) // bind ebo
        let size = nativeint ((Array.length indices) * sizeof<'T>)
        GL.BufferData(BufferTarget.ElementArrayBuffer, size, indices, bufferUsage)
    /// Creates a vertex buffer and vertex array object and binds them specified by buffer usage and attribute
    let createVertexBuffer (vertices:'T array) bufferUsage attribute =
        let usage = toBufferUsage bufferUsage
        let vao = GL.GenVertexArray()
        let buffer = GL.GenBuffer()
        GL.BindVertexArray(vao) // bind vertex array
        setBufferData buffer vertices usage attribute
        GL.BindVertexArray(0) // debind vertex array again
        {VAO = vao; Buffer = buffer}
    /// Creates a vertex buffer, vertex array and element buffer and binds them together
    let createVertexBufferIndexed (vertices:'T array) (indices:'U array) bufferUsage attribute =
        let usage = toBufferUsage bufferUsage
        let vao = GL.GenVertexArray()
        let buffer = GL.GenBuffer()
        GL.BindVertexArray(vao) // bind vertex array
        setBufferData buffer vertices usage attribute // set normal buffer
        let ebo = GL.GenBuffer() // set ebo
        setEBOData ebo indices usage
        GL.BindVertexArray(0) // debind vertex array again
        {GLVertexBufferIndexed.VertexBuffer = {VAO = vao; Buffer = buffer}; EBO = ebo}