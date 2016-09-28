namespace VyrCore

open MathNet.Numerics

module Math =
    /// Checks if a generic numerical is a power of 2
    let inline isPowerOf2 x = (x &&& (x - LanguagePrimitives.GenericOne)) = LanguagePrimitives.GenericZero

module MathVec2 =
    /// Calculates the squared length of a Vec2
    let inline squaredMagnitude (v:Vec2<_>) = let genTwo = genericTwo() in (v.X ** genTwo) + (v.Y ** genTwo)
    /// Calculates the length of a Vec2
    let inline magnitude (v:Vec2<_>) = v |> squaredMagnitude |> sqrt

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
   // let inline triangleInequality (v:Vec)