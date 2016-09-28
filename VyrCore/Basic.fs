namespace VyrCore

/// TODO: Add struct attribute in later f# version

/// Module providing functions for handling the generic types
[<AutoOpen>]
module internal GenericType =
    let inline zeroOf (a:'a) = LanguagePrimitives.GenericZero<'a>
    let inline oneOf (a:'a) = LanguagePrimitives.GenericOne<'a>
    let inline negativeOneOf (a:'a) = zeroOf a - oneOf a
    let inline genericTwo() = LanguagePrimitives.GenericOne + LanguagePrimitives.GenericOne

/// Basic color type with rgba values
[<Struct>]
type Color<'a>(r:'a, g:'a, b:'a, a:'a) =
    member this.R = r
    member this.G = g
    member this.B = b
    member this.A = a

/// Basic vector 2 type with typical operators.
[<Struct>]
type Vec2<'a>(x:'a, y:'a) =
    member this.X = x
    member this.Y = y
    static member inline (*) (a, v:Vec2<_>) = Vec2<_>(a * v.X, a * v.Y)
    static member inline (*) (v:Vec2<_>, a) = Vec2<_>(a * v.X, a * v.Y)
    static member inline (+) (v1:Vec2<_>, v2:Vec2<_>) = Vec2<_>(v1.X + v2.X, v1.Y + v2.Y)
    static member inline (-) (v1:Vec2<_>, v2:Vec2<_>) = Vec2<_>(v1.X - v2.X, v1.Y - v2.Y)
    static member inline (~-) (v:Vec2<_>) = let negOne = negativeOneOf v.X in Vec2<_>(negOne * v.X, negOne * v.Y) 

/// Basic vector 3 type with typical operators.
[<Struct>]
type Vec3<'a>(x:'a, y:'a, z:'a) =
    member this.X = x
    member this.Y = y
    member this.Z = z
    static member inline (*) (a, v:Vec3<_>) = Vec3<_>(a * v.X, a * v.Y, a * v.Z)
    static member inline (*) (v:Vec3<_>, a) = Vec3<_>(a * v.X, a * v.Y, a * v.Z)
    static member inline (+) (v1:Vec3<_>, v2:Vec3<_>) = Vec3<_>(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z)

/// Basic vector 4 type with typical operators.
[<Struct>]
type Vec4<'a>(x:'a,y:'a,z:'a,w:'a) =
    member this.X = x
    member this.Y = y
    member this.Z = z
    member this.W = w
    static member inline (*) (a, v:Vec4<_>) = Vec4<_>(a * v.X, a * v.Y, a * v.Z, a * v.W)
    static member inline (*) (v:Vec4<_>, a) = Vec4<_>(a * v.X, a * v.Y, a * v.Z, a * v.W)
    static member inline (+) (v1:Vec4<_>, v2:Vec4<_>) = Vec4<_>(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W)

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
module TypeOperations =
    /// Returns the data type for a generic type. This function works only on primitive types like int short etc.
    let dataType<'a> =
        match typeof<'a> with
        | x when x = typeof<int> -> Just DataType.Int
        | x when x = typeof<int16> -> Just DataType.Short
        | x when x = typeof<uint32> -> Just DataType.UnsignedInt
        | x when x = typeof<float> -> Just DataType.Double
        | x when x = typeof<single> -> Just DataType.Single
        | _ -> Nothing
    /// Returns a color struct as array
    let toColorArray (c:Color<'a>) = [|c.R; c.G; c.B; c.A|]