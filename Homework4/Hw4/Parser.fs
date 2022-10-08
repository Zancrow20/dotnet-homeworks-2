module Hw4.Parser

open System
open Hw4.Calculator
open Microsoft.FSharp.Core


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    match Array.length args with
    | 3 -> true
    | _ -> false

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> CalculatorOperation.Undefined
    
let tryParseValues (arg1 : string, arg2 : string) =
    match Double.TryParse arg1, Double.TryParse arg2 with
    | (true,_),(true,_) -> true
    | _ -> false
    
let parseCalcArguments(args : string[]) =
    match isArgLengthSupported args with
    | false -> ArgumentException() |> raise
    | true ->
        match parseOperation args[1] with
        | CalculatorOperation.Undefined -> ArgumentException() |> raise
        | CalculatorOperation.Plus | CalculatorOperation.Minus | CalculatorOperation.Multiply | CalculatorOperation.Divide ->
            match tryParseValues (args[0], args[2]) with
            | false -> ArgumentException() |> raise
            | true -> {arg1 = Double.Parse args[0]; arg2 = Double.Parse args[2]; operation = parseOperation args[1]}
        | _ -> ArgumentOutOfRangeException() |> raise