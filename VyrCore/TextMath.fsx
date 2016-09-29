#load "FSharpTypes.fs"
#load "Basic.fs"
#load "Math.fs"

open VyrCore

let v1 = Vec2(0., 1.)
let v2 = Vec2(1., 0.)
let dot1 = Vector2.dot v1 v2
let orthogonal1 = Vector2.isOrthogonal v1 v2 System.Double.Epsilon

let v3 = Vec2(0.5, -1.)
let v4 = Vec2(1., 0.5)
let dot2 = Vector2.dot v3 v4
let orthogonal2 = Vector2.isOrthogonal v3 v4 System.Double.Epsilon

/// Decimal version very accurate; Floating version of check equality would not work cause of floating point inprecision.
let v5 = Vec3(0.3M, 0.4M, 0.6M)
let v6 = Vec3(0.5M, 0.7M, 0.9M)
let cross = Vector3.cross v5 v6
let dot3 = Vector3.dot cross v5
let dot4 = Vector3.dot cross v6
let checkEquality = dot3 = dot4
//let checkEquality = Math.approximately System.Decimal. dot3 dot4
//let checkZero = Math.approximately System.Double.Epsilon dot3 0.0