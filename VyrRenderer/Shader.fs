namespace VyrRenderer

open FSharp.NativeInterop
open VyrCore
open OpenTK
open OpenTK.Graphics.OpenGL

type GLShader =
    {
        Shader : int
        Type : VyrCore.ShaderType
    }
    interface IShader with
        member this.Type = this.Type
        member this.Dispose() = GL.DeleteShader(this.Shader)

type GLShaderProgram =
    {
        Program : int
    }
    interface IShaderProgram with
        member this.Dispose() = GL.DeleteProgram(this.Program)

module Shader =
    /// Creates a shader by using a shader type and the source code
    let createShader shaderType source = 
        let t = toShaderType shaderType
        let shader = GL.CreateShader(t)
        GL.ShaderSource(shader, source)
        GL.CompileShader(shader)
        let result = ref -1
        GL.GetShader(shader, ShaderParameter.CompileStatus, result)
        if result.Value = 1 then Result<IShader, string>.Success {Shader = shader; Type = shaderType}
        else
            let log = GL.GetShaderInfoLog(shader)
            Result<IShader, string>.Error log
    /// Creates a shader program from a sequence of shaders
    let createShaderProgram (shaders : IShader seq) = 
        let program = GL.CreateProgram()
        shaders |> Seq.map (fun s -> s :?> GLShader) |> Seq.iter (fun s -> GL.AttachShader(program, s.Shader))
        GL.LinkProgram(program)
        let result = ref -1
        GL.GetProgram(program, ProgramParameter.LinkStatus, result)
        if result.Value = 1 then Result<IShaderProgram, string>.Success {Program = program}
        else 
            let log = GL.GetProgramInfoLog(program)
            Result<IShaderProgram, string>.Error log
