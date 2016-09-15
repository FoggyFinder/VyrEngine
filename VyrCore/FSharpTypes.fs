namespace VyrCore

type Maybe<'a> =
    | Just of 'a
    | Nothing
    with 
    member this.Value = 
        match this with
        | Just v -> v
        | Nothing -> failwith "The value is None!"

/// A basic result type where only the error type is of interest.
type Result<'TError> = 
    | Success
    | Error of 'TError
    with 
    override this.ToString() = match this with Error t -> t.ToString() | _ -> "Success"

/// A basic result type
type Result<'TSuccess, 'TError> =
    | Success of 'TSuccess
    | Error of 'TError
    with
    member this.Value =
        match this with
        | Success t -> t
        | _ -> failwith "This result is an error type."
    member this.ErrorValue =
        match this with
        | Error t -> t
        | _ -> failwith "This result contains no error."

/// Abstract class defining a disposable. Dispose sets a flag so the finalizer doesn't call dispose again else finalization happens automatically.
[<AbstractClass>]
type Disposable() as this = 
    let mutable isDisposed = false
    let dispose() = if not isDisposed then this.OnDispose(); isDisposed <- true
    abstract OnDispose : unit -> unit
    override this.Finalize() = dispose()
    interface System.IDisposable with
        member this.Dispose() = dispose()

/// Sets up a record which automatically disposes it's contained object while being finalized
type Finalizer<'a when 'a :> System.IDisposable> =
    {
        Obj : 'a
    }
    with override this.Finalize() = this.Obj.Dispose()

/// Provides some convients functions for basic fsharp types
[<AutoOpen>]
module FSharp =
    /// Checks if an optional value is some and returns true else false
    let isSome optional = 
        match optional with
        | Just _ ->  true
        | _ -> false

[<AutoOpen>]
module Maybe =
    let internal bindOptional m f =
        match m with
        | Just a -> f a
        | _ -> Nothing
    /// Maybe monad with convenient functions using maybe types 
    type MaybeBuilder() =
        member this.Bind(m, f) = bindOptional m f
        member this.Return(m) = Just m
    /// Use this maybe instance for maybe monad expressions
    let maybe = new MaybeBuilder()

[<AutoOpen>]
module Result =
    let internal bind (m:Result<'a, 'b>) (f:'a -> Result<'c,'b>) =
        match m with
        | Result.Success a -> f a
        | Result.Error b -> Error b
    /// The folder uses a current state and accumulates each new result. 
    let internal fold (s:Result<'a seq, 'b>) r = 
        match r with  // match the current result
        | Success succ ->
            match s with // match the accumulated results
            | Success successSeq -> // if both are success, just append the new success
                let append = successSeq |> Seq.append (Seq.singleton succ)
                Success append 
            | _ -> 
                match box succ with :? System.IDisposable as d -> d.Dispose() | _ -> () // make sure to dispose the result, cause it is not used anyway
                s // if the list is an error, discard r
        | Error err ->
            match s with
            | Success successSeq -> // the first error contact will create a new error result
                // make sure to dispose all values if disposable is implemented
                successSeq |> Seq.iter (fun (v:'a) -> match box v with :? System.IDisposable as d -> d.Dispose() | _ -> ())
                Error err
            | _ -> Error err
    /// Accumulates all values by function f and returns one result value containing the success sequence or an error
    let internal forValues values (f:'t -> Result<'a, 'b>) =
        let startState = Result<'a seq, 'b>.Success Seq.empty
        values 
        |> Seq.map f
        |> Seq.fold fold startState
    /// Result monad which makes it easier to process functions returning results
    type ResultBuilder() =
        member this.Bind(m, f) = bind m f
        member this.YieldFrom(x) = x
        member this.Yield(x) = Result<'a, 'b>.Success x
        member this.Return(x) = Result<'a,'b>.Success x
        member this.For(values, f) = forValues values f

    let result = new ResultBuilder()