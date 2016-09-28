namespace VyrCore

module Math =
    let inline approximately x y = abs (x - y) < System.Double.Epsilon
    /// Checks if a generic numerical is a power of 2
    let inline isPowerOf2 x = (x &&& (x - LanguagePrimitives.GenericOne)) = LanguagePrimitives.GenericZero

module MathVec2 =
    /// Calculates the squared length of a vector
    let inline squaredMagnitude (v:Vec2<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo)
    /// Calculates the length of a vector
    let inline magnitude (v:Vec2<_>) = v |> squaredMagnitude |> sqrt
    /// Calculates the dot product of two vectors
    let inline dot (v1:Vec2<_>) (v2:Vec2<_>) = v1.X * v2.X + v1.Y * v2.Y
    /// Calculates the angle of two vectors by using the acos of the dot product divided by their magnitudes
    let inline angle (v1:Vec2<_>) (v2:Vec2<_>) = (dot v1 v2) / ((magnitude v1) * (magnitude v2)) |> acos
    /// Checks if 2 vectors are orthogonal by calculating their dot product and testing against zero
    let inline isOrthogonal (v1:Vec2<_>) (v2:Vec2<_>) = (dot v1 v2) |> Math.approximately LanguagePrimitives.GenericZero
    /// Checks if 2 vectors lie on the same side of a plane
    let inline isSameSided (v1:Vec2<_>) (v2:Vec2<_>) = ((dot v1 v2) |> sign) > LanguagePrimitives.GenericZero

module MathVec3 =
    /// Calculates the squared length of a Vec2
    let inline squaredMagnitude (v:Vec3<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo) + (v.Z ** genTwo)
    /// Calculates the length of a Vec2
    let inline magnitude (v:Vec3<_>) = v |> squaredMagnitude |> sqrt

module MathVec4 =
     /// Calculates the squared length of a Vec2
    let inline squaredMagnitude (v:Vec4<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo) + (v.Z ** genTwo) + (v.W ** genTwo)
    /// Calculates the length of a Vec2
    let inline magnitude (v:Vec4<_>) = v |> squaredMagnitude |> sqrt   

//module Trigonometry =
    /// Checks if two vectors can form a triangle with a 
    //let inline isTriangle v1 v2 v3