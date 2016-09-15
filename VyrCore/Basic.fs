namespace VyrCore

/// TODO: Add struct attribute in later f# version
(*type Color = 
    {
        R : float32
        G : float32
        B : float32
        A : float32
    }*)
[<Struct>]
type Color<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)>(r:'a, g:'a, b:'a, a:'a) =
    member this.R = r
    member this.G = g
    member this.B = b
    member this.A = a

[<Struct>]
type Vec2<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)>(x:'a, y:'a) =
    member this.X = x
    member this.Y = y

[<Struct>]
type Vec3<'a when 'a : struct and 'a :> System.ValueType and 'a : (new : unit -> 'a)>(x:'a, y:'a, z:'a) =
    member this.X = x
    member this.Y = y
    member this.Z = z

/// TODO: Add struct attribute in later f# version
(*type Vec2<'a> =
    {
        X : 'a
        Y : 'a
    }

/// TODO: Add struct attribute in later f# version
type Vec3<'a> =
    {
        X : 'a
        Y : 'a
        Z : 'a
    }*)

/// TODO: Add struct attribute in later f# version
type Size<'a> =
    {
        Width : 'a
        Height : 'a
    }

type DataType = 
    | Int = 0
    | Short = 1
    | Single = 2
    | Double = 3
    | UnsignedInt = 4

[<AutoOpen>]
module Basic =
    let dataType<'a> = 
        match typeof<'a> with
        | x when x = typeof<int> -> Just DataType.Int
        | x when x = typeof<int16> -> Just DataType.Short
        | x when x = typeof<uint32> -> Just DataType.UnsignedInt
        | x when x = typeof<float> -> Just DataType.Double
        | x when x = typeof<single> -> Just DataType.Single
        | _ -> Nothing