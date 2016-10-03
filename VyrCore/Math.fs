namespace VyrCore

/// Math module for plain numbers
[<RequireQualifiedAccess>]
module Math =
    /// Returns true when 2 numbers are nearly equal.
    let inline approximately epsilon x y  = abs (x - y) < epsilon
    /// Checks if a generic numerical is a power of 2
    let inline isPowerOf2 x = (x &&& (x - LanguagePrimitives.GenericOne)) = LanguagePrimitives.GenericZero

/// Math module for Vector2 types
[<RequireQualifiedAccess>]
module Vec2 =
    /// Calculates the squared length of a vector
    let inline squaredMagnitude (v:Vec2<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo)
    /// Calculates the length of a vector
    let inline magnitude (v:Vec2<_>) = v |> squaredMagnitude |> sqrt
    /// Normalizes a vector by dividing by its magnitude
    let inline normalize (v:Vec2<'a>) : Vec2<'a> = v / (magnitude v)
    /// Calculates the perpendicular vector to v
    let inline perp (v:Vec2<_>) = Vec2<_>(-v.Y, v.X)
    /// Calculates the dot product of two vectors
    let inline dot (v1:Vec2<_>) (v2:Vec2<_>) = v1.X * v2.X + v1.Y * v2.Y
    /// Calculates the perpendicular dot product of v1 and v2. 
    let inline perpDot (v1:Vec2<_>) (v2:Vec2<_>) = v1 |> perp |> dot v2
    /// Calculates the angle of two vectors by using the acos of the dot product divided by their magnitudes
    let inline angle (v1:Vec2<_>) (v2:Vec2<_>) = ((dot v1 v2) / ((magnitude v1) * (magnitude v2))) |> acos
    /// Checks if 2 vectors are orthogonal by calculating their dot product and testing against zero
    let inline isOrthogonal (v1:Vec2<_>) (v2:Vec2<_>) epsilon = (dot v1 v2) |> Math.approximately epsilon LanguagePrimitives.GenericZero
    /// Checks if 2 vectors lie on the same side of a plane
    let inline isSameSided (v1:Vec2<_>) (v2:Vec2<_>) = ((dot v1 v2) |> sign) > LanguagePrimitives.GenericZero

/// Math module for Vector3 types
[<RequireQualifiedAccess>]
module Vec3 =
    /// Calculates the squared length of a Vec2
    let inline squaredMagnitude (v:Vec3<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo) + (v.Z ** genTwo)
    /// Calculates the length of a Vec2
    let inline magnitude (v:Vec3<_>) = v |> squaredMagnitude |> sqrt
    /// Normalizes a vector by dividing by its magnitude
    let inline normalize (v:Vec3<'a>) : Vec3<'a> = v / (magnitude v)
    /// Calculates the cross product of two vectors
    let inline cross (v1:Vec3<_>) (v2:Vec3<_>) = Vec3<_>(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X)
    /// Calculates the dot product of two vectors
    let inline dot (v1:Vec3<_>) (v2:Vec3<_>) = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z
    /// Calculates the angle of two vectors by using the acos of the dot product divided by their magnitudes
    let inline angle (v1:Vec3<_>) (v2:Vec3<_>) = ((dot v1 v2) / ((magnitude v1) * (magnitude v2))) |> acos
    /// Checks if 2 vectors are orthogonal by calculating their dot product and testing against zero
    let inline isOrthogonal (v1:Vec3<_>) (v2:Vec3<_>) epsilon = (dot v1 v2) |> Math.approximately epsilon LanguagePrimitives.GenericZero
    /// Checks if 2 vectors lie on the same side of a plane
    let inline isSameSided (v1:Vec3<_>) (v2:Vec3<_>) = ((dot v1 v2) |> sign) > LanguagePrimitives.GenericZero

/// Math module for Vector4 types
[<RequireQualifiedAccess>]
module Vec4 =
     /// Calculates the squared length of a Vec2
    let inline squaredMagnitude (v:Vec4<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo) + (v.Z ** genTwo) + (v.W ** genTwo)
    /// Calculates the length of a Vec2
    let inline magnitude (v:Vec4<_>) = v |> squaredMagnitude |> sqrt
    /// Normalizes a vector by dividing by its magnitude
    let inline normalize (v:Vec4<'a>) : Vec4<'a> = v / (magnitude v)
    /// Returns a sequence containing the values of the vector in order X,Y,Z,W
    let inline toSeq (v:Vec4<_>) = seq { yield v.X; yield v.Y; yield v.Z; yield v.W }

/// Math module for matrix types in row major order
[<RequireQualifiedAccess>]
module Matrix =
    /// Calculates the index from row and column
    let inline internal index row col colCount = row * colCount + col
    /// Calculates the row from index and column count
    let inline internal row index colCount = index / colCount
    /// Calculates the column from index and column count
    let inline internal col index colCount = index % colCount
    /// Initializes a matrix array by providing the rows columns and a function
    let inline internal initData rows columns  f = 
        let inline initFunc i = f (row i columns) (col i columns)
        Array.init (rows * columns) initFunc
    /// Initializes a matrix array by providing rows columns and a function processing the diagonal indices.
    let inline internal initDiagonalData rows columns f =
        let inline initFunc row column = if row = column then f row else LanguagePrimitives.GenericZero
        initData rows columns initFunc
    /// Calls the constructor of the Matrix class
    let inline internal create rows columns data = Matrix(data, rows, columns)
    /// Creates a new matrix from a number of columns and rows and a function. The function takes the row and column as index.
    let inline init rows columns f = initData rows columns f |> create rows columns
    /// Creates a new matrix from a number of columns and rows and a function. The function iterates the values along the diagonal. For a 3x3 matrix the diagonal indices are 0, 1, 2.
    let inline initDiagonal rows columns f = initDiagonalData rows columns f |> create rows columns
    /// Creates a symmetric identity matrix from a number rows/columns
    let inline identity n =
        let inline initFunc (row:int) (col:int) = if row = col then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero
        init n n initFunc

[<RequireQualifiedAccess>]
module Matrix4 =
    /// Calls the constructor for a 4x4 matrix (array of length 16)
    let inline create data = Matrix4(data)
    /// Creates a 4x4 matrix and initializes it by using a function taking the row and column.
    let inline init f = Matrix.initData 4 4 f |> create 
    /// Creates a 4x4 matrix and initializes it by using a function taking the indices of the diagonal
    let inline initDiagonal f = Matrix.initDiagonalData 4 4 f |> create
    /// Returns the 4x4 identity
    let inline identity() = let m = Matrix.identity 4 in create m.Data
    /// Multiplies 2 Matrix4 types
    let inline mult m1 m2 = m1 .* m2

//module Trigonometry =
    /// Checks if two vectors can form a triangle with a 
    //let inline isTriangle v1 v2 v3