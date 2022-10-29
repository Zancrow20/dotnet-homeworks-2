module Hw5.Program

open Hw5
open Parser
open Calculator

[<EntryPoint>]
let main (args: string[]) =
    match parseCalcArguments args with
    | Ok res ->
        match res with
        | firstArg, operation, secondArg -> printf $"{calculate firstArg operation secondArg}"
    | Error error -> printf $"Return {error} for {args}"
    0
