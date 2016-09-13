namespace VyrCore

/// TODO: Add struct attribute in later f# version
type Color = 
    {
        R : float32
        G : float32
        B : float32
        A : float32
    }

/// TODO: Add struct attribute in later f# version
type Vec2<'a> =
    {
        X : 'a
        Y : 'a
    }

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

type PrimitiveType = 
    | Triangles = 0
