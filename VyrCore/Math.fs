namespace VyrCore

module Math =
    let inline isPowerOf2 x = (x &&& (x - LanguagePrimitives.GenericOne)) = LanguagePrimitives.GenericZero