namespace VyrRenderer

open FSharp.NativeInterop
open VyrCore
open OpenTK

#if _WIN32
open OpenTK.Graphics.OpenGL
#endif

type GLShader =
    {
        Shader : int
        Type : VyrCore.ShaderType
    }
    interface IShader with
        member this.Type = this.Type
        member this.Dispose() = GL.DeleteShader(this.Shader)

type GLShaderUniform<'a> =
    {
        Location : int
    }
    interface IShaderUniform<'a>

type GLShaderProgram =
    {
        Program : int
    }
    interface IShaderProgram with
        member this.Dispose() = GL.DeleteProgram(this.Program)
        member this.GetUniform<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)>(s) = 
            let location = GL.GetUniformLocation(this.Program, s)
            {GLShaderUniform.Location = location} :> IShaderUniform<'a>
        member this.GetUniformValue(u:IShaderUniform<'a>) = 
            let uniform = u :?> GLShaderUniform<'a>
            match typeof<'a> with
            | x when x = typeof<int> -> 
                let i = ref -1
                GL.GetUniform(this.Program, uniform.Location, i)
                Just (unbox i.Value)
            | _ -> Nothing
        member this.SetUniform u v = 
            let uniform = u :?> GLShaderUniform<_>
            match box v with
            | :? int as i  -> GL.ProgramUniform1(this.Program, uniform.Location, i)
            | :? single as s -> GL.ProgramUniform1(this.Program, uniform.Location, s)
            | :? float as f -> GL.ProgramUniform1(this.Program, uniform.Location, f)
            | _ -> failwith "Type not supported."

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
            GL.DeleteProgram(program)
            Result<IShaderProgram, string>.Error log
