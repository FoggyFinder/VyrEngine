namespace VyrRenderer

open VyrCore
open OpenTK
open OpenTK.Graphics.OpenGL

type GLVertexBuffer =
    {
        VAO : int
        EBO : int VyrCore.Option
        Buffer : int
    }
    interface IVertexBuffer with
        member this.Dispose() = 
            GL.DeleteVertexArray(this.VAO)
            GL.DeleteBuffer(this.Buffer)
            match this.EBO with
            | Some ebo -> GL.DeleteBuffer(ebo)
            | _ -> ()
        member this.ContainsIndexBuffer = isSome this.EBO

module VertexBuffer = 
    /// Sets the buffer data for a vertex buffer object and sets the first attribute
    let private setBufferData (buffer:int) vertices bufferUsage attribute =
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer) // bind buffer
        let size = nativeint ((Array.length vertices) * sizeof<'T>)
        GL.BufferData(BufferTarget.ArrayBuffer, size, vertices, bufferUsage) // set the vertex data
        GL.EnableVertexAttribArray(0) // set attribute, we have only 1 so the index of this attribute is 0
        let attribType = toAttribPointer attribute.DataType
        GL.VertexAttribPointer(0, attribute.Components, attribType, false, attribute.Stride, attribute.Offset)
    /// Creates a vertex buffer and vertex array object and binds them specified by buffer usage and attribute
    let createVertexBuffer (vertices:'T array) bufferUsage attribute =
        let usage = toBufferUsage bufferUsage
        let vao = GL.GenVertexArray()
        let buffer = GL.GenBuffer()
        GL.BindVertexArray(vao) // bind vertex array
        setBufferData buffer vertices usage attribute
        GL.BindVertexArray(0) // debind vertex array again
        {VAO = vao; Buffer = buffer; EBO = None}
    /// Creates a vertex buffer, vertex array and element buffer and binds them together
    let createVertexBufferIndexed (vertices:'T array) (indices:uint32 array) bufferUsage attribute =
        let usage = toBufferUsage bufferUsage
        let vao = GL.GenVertexArray()
        let buffer = GL.GenBuffer()
        let ebo = GL.GenBuffer()
        GL.BindVertexArray(vao) // bind vertex array
        setBufferData buffer vertices usage attribute
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo) // bind ebo
        let size = nativeint ((Array.length indices) * sizeof<uint32>)
        GL.BufferData(BufferTarget.ElementArrayBuffer, size, indices, usage)
        GL.BindVertexArray(0) // debind vertex array again
        {VAO = vao; Buffer = buffer; EBO = Some ebo}