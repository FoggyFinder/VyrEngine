namespace VyrCore

type Option<'a> =
    | Some of 'a
    | None
    with 
    member this.Value = 
        match this with
        | Some v -> v
        | None -> failwith "The value is None!"

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

[<AutoOpen>]
module FSharp =
    let isSome optional = 
        match optional with
        | Some _ ->  true
        | _ -> false