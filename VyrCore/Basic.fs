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
    override this.ToString() = this.X.ToString() + " | " + this.Y.ToString()
    static member inline (*) (a, v:Vec2<_>) = Vec2<_>(a * v.X, a * v.Y)
    static member inline (*) (v:Vec2<_>, a) = Vec2<_>(a * v.X, a * v.Y)
    static member inline (/) (v:Vec2<'T>, a:'T) : Vec2<'T> = Vec2<_>(v.X / a, v.Y / a)
    static member inline (+) (v1:Vec2<_>, v2:Vec2<_>) = Vec2<_>(v1.X + v2.X, v1.Y + v2.Y)
    static member inline (-) (v1:Vec2<_>, v2:Vec2<_>) = Vec2<_>(v1.X - v2.X, v1.Y - v2.Y)
    static member inline (~-) (v:Vec2<_>) = let negOne = negativeOneOf v.X in Vec2<_>(negOne * v.X, negOne * v.Y) 

/// Basic vector 3 type with typical operators.
[<Struct>]
type Vec3<'a>(x:'a, y:'a, z:'a) =
    member this.X = x
    member this.Y = y
    member this.Z = z
    override this.ToString() = this.X.ToString() + " | " + this.Y.ToString() + " | " + this.Z.ToString()
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

/// Interface for each matrix using the matrix math module.
type IMatrix<'a> =
    abstract Data : 'a array
    abstract ColumnCount : int
    abstract RowCount : int

/// A arbitrary matrix with user defined column and row count.
[<Struct>]
type Matrix<'a>(arr : 'a array, rowCount : int, columnCount : int) =
    member this.Data = arr
    member this.RowCount = rowCount
    member this.ColumnCount = columnCount
    override this.ToString() =
        let colCount = this.ColumnCount 
        let inline toString i v = v.ToString() + (if ((i + 1) % colCount) = 0 then "\n" else " ")
        arr
        |> Array.mapi toString
        |> String.concat ""
        |> (+) "\n"
    interface IMatrix<'a> with
        member this.Data = arr
        member this.ColumnCount = columnCount
        member this.RowCount = rowCount
    /// Multiplies two matrices naively.
    static member inline (*) (m1:Matrix<_>, m2:Matrix<_>) = 
        if m1.ColumnCount = m2.RowCount then
            let inline index row col colCount = row * colCount + col
            let inline mult row col k = m1.Data.[index row k m1.ColumnCount] * m2.Data.[index k col m2.ColumnCount]
            let data = [|for row = 0 to (m1.RowCount - 1) do for col = 0 to (m2.ColumnCount - 1) do yield (seq {0..(m1.ColumnCount - 1)} |> Seq.map (mult row col) |> Seq.sum)|]
            Just (Matrix(data, m1.RowCount, m2.ColumnCount))
        else Nothing
    /// Multiplies a matrix by a scalar value
    static member inline (.*) (m:Matrix<_>, a) = let inline map x = x * a in Matrix<_>(Array.map map m.Data, m.RowCount, m.ColumnCount)
    /// Multiplies a matrix by a scalar value
    static member inline (.*) (a, m:Matrix<_>) = let inline map x = x * a in Matrix<_>(Array.map map m.Data, m.RowCount, m.ColumnCount)

/// A 4x4 matrix used for all necessary transformations in the graphics pipeline.
[<Struct>]
type Matrix4<'a>(arr : 'a array) =
    member this.Data = arr
    override this.ToString() = 
        let inline toString i v = v.ToString() + (if ((i + 1) % 4) = 0 then "\n" else " ")
        arr
        |> Array.mapi toString
        |> String.concat ""
        |> (+) "\n"
    interface IMatrix<'a> with
        member this.Data = arr
        member this.ColumnCount = 4
        member this.RowCount = 4
    /// Adds two matrices component wise
    static member inline (+) (m1:Matrix4<_>, m2:Matrix4<_>) = let inline map x y = x + y in Matrix4<_>(Array.map2 map m1.Data m2.Data)
    /// Multiplies two matrices by brute force.
    static member inline (*) (m1:Matrix4<_>, m2:Matrix4<_>) = 
        let inline index row col colCount = row * colCount + col
        let inline mult row col k= m1.Data.[index row k 4] * m2.Data.[index k col 4]
        let data = [|for row = 0 to 3 do for col = 0 to 3 do yield (seq {0..3} |> Seq.map (mult row col) |> Seq.sum)|]
        Matrix4(data)
    /// Multiplies a matrix by a vector, returning another vector.
    static member inline (*) (m:Matrix4<_>, v:Vec4<_>) = ()
    /// Multiplies matrix values by a generic value
    static member inline (.*) (m:Matrix4<_>, a) = let inline map x = x * a in Matrix4<_>(Array.map map m.Data)
    /// Multiplies matrix values by a generic value
    static member inline (.*) (a, m:Matrix4<_>) = let inline map x = x * a in Matrix4<_>(Array.map map m.Data)


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
    let inline dataType<'a> =
        match typeof<'a> with
        | x when x = typeof<int> -> Just DataType.Int
        | x when x = typeof<int16> -> Just DataType.Short
        | x when x = typeof<uint32> -> Just DataType.UnsignedInt
        | x when x = typeof<float> -> Just DataType.Double
        | x when x = typeof<single> -> Just DataType.Single
        | _ -> Nothing
    /// Returns a color struct as array
    let inline toColorArray (c:Color<'a>) = [|c.R; c.G; c.B; c.A|]