module Hw6.Message

open System.Diagnostics.CodeAnalysis

type Message =
    | SuccessfulExecution = 0
    | WrongArgFormatForValue1 = 1
    | WrongArgFormatOperation = 2
    | WrongArgFormatForValue2 = 3
    | WrongArgLength = 4
    | DivideByZero = 5
    
[<ExcludeFromCodeCoverage>]
let convertMessageToString (message: Result<float, Message>, args: string[])=
    match message with
    | Ok ok -> Ok $"{ok}"
    | Error error ->
        match int error with
            | 0 -> Ok "Success"
            | 4 -> Error "WrongArgLength"
            | 5 -> Ok "DivideByZero"
            | n when n = 1 || n = 2 || n = 3 -> Error $"Could not parse value '{args[n-1]}'"