#load "FSharpTypes.fs"
#load "Basic.fs"
#load "Math.fs"

open VyrCore

let v1 = Vec2(0., 1.)
let v2 = Vec2(1., 0.)
let dot = MathVec2.dot v1 v2
let orthogonal = MathVec2.isOrthogonal v1 v2

let v3 = Vec2(0.5, -1.)
let v4 = Vec2(1., 0.5)
let dot2 = MathVec2.dot v3 v4
let orthogonal2 = MathVec2.isOrthogonal v3 v4
