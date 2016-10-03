#load "FSharpTypes.fs"
#load "Basic.fs"
#load "Math.fs"

open VyrCore

let v1 = Vec2(0., 1.)
let v2 = Vec2(1., 0.)
let dot1 = Vec2.dot v1 v2
let orthogonal1 = Vec2.isOrthogonal v1 v2 System.Double.Epsilon

let v3 = Vec2(0.5, -1.)
let v4 = Vec2(1., 0.5)
let dot2 = Vec2.dot v3 v4
let orthogonal2 = Vec2.isOrthogonal v3 v4 System.Double.Epsilon

/// Decimal version very accurate; Floating version of check equality would not work cause of floating point inprecision.
let v5 = Vec3(0.3M, 0.4M, 0.6M)
let v6 = Vec3(0.5M, 0.7M, 0.9M)
let cross = Vec3.cross v5 v6
let dot3 = Vec3.dot cross v5
let dot4 = Vec3.dot cross v6
let checkEquality = dot3 = dot4
//let checkEquality = Math.approximately System.Decimal. dot3 dot4
//let checkZero = Math.approximately System.Double.Epsilon dot3 0.0

// Test Matrix<_> calculations
let mIdentity : Matrix<float32> = Matrix.identity(3)
let m31 = mIdentity * 2.0f
let m32 = Matrix.init 4 3 (fun row col -> float32 (row + col))
let m33 = Matrix.init 3 5 (fun row col -> float32 (row + col))
let m34 = m32 .* m33
printfn "%A" (Matrix.initDiagonal 4 3 (fun i -> float32 i))

// Test Matrix4<_> calculations
let matIdentity = Matrix4.identity()
let mi1 = matIdentity * 3.0f
let mi2 = 2.0f * matIdentity
let mi3 = mi1 + mi2
printfn "%A" (Matrix4.initDiagonal (fun i -> float32 i))

let matFull = Matrix4.init (fun row col -> float (row + col))
let mf1 = matFull * 2.0
let mf2 = matFull * 1.5
let mf3 = mf1 + mf2
let mf4 : Matrix4<float> = mf1 .* mf2
 