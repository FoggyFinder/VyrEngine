namespace VyrCore

/// Math module for plain numbers
module Math =
    /// Returns true when 2 numbers are nearly equal.
    let inline approximately epsilon x y  = abs (x - y) < epsilon
    /// Checks if a generic numerical is a power of 2
    let inline isPowerOf2 x = (x &&& (x - LanguagePrimitives.GenericOne)) = LanguagePrimitives.GenericZero

/// Math module for Vector2 types
module Vector2 =
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
module Vector3 =
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
module Vector4 =
     /// Calculates the squared length of a Vec2
    let inline squaredMagnitude (v:Vec4<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo) + (v.Z ** genTwo) + (v.W ** genTwo)
    /// Calculates the length of a Vec2
    let inline magnitude (v:Vec4<_>) = v |> squaredMagnitude |> sqrt
    /// Normalizes a vector by dividing by its magnitude
    let inline normalize (v:Vec4<'a>) : Vec4<'a> = v / (magnitude v)

module Matrix =
    let init row col f = ()

//module Trigonometry =
    /// Checks if two vectors can form a triangle with a 
    //let inline isTriangle v1 v2 v3