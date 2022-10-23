module Hw6.Parser

open System
open System.Globalization
open Hw6.Message
open Hw6.MaybeBuilder
open Hw6.Calculator
open System.Diagnostics.CodeAnalysis


let convertToOperation operation =
    match operation with
    | "Plus" -> "+"
    | "Minus" -> "-"
    | "Multiply" -> "*"
    | "Divide" -> "/"
    | _ -> operation

[<ExcludeFromCodeCoverage>]
let isArgLengthSupported (args:string[]) =
    match Array.length args with
    | 3 -> Ok(args)
    | _ -> Error Message.WrongArgLength
    
    
[<ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | Calculator.plus -> Ok(arg1, CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok(arg1, CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok(arg1, CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok(arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let (|Double|_|) (str:string) =
    match Double.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) with
    | (true, double) -> Some(double)
    | _ -> None

let parseArgs (args: string[]): Result<('a * string * 'b), Message> =
    let firstValue = (|Double|_|) args[0]
    let secondValue = (|Double|_|) args[2]
    match firstValue with
    | Some arg1 ->
        match secondValue with
        | Some arg2 -> Ok (arg1, args[1], arg2)
        | _ -> Error Message.WrongArgFormatForValue2
    | _ -> Error Message.WrongArgFormatForValue1

[<ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match arg2,operation with
    | 0.0,CalculatorOperation.Divide -> Error Message.DivideByZero
    | _ -> Ok(arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe
       {
            let! checkLength = isArgLengthSupported args
            let! parsedValues = parseArgs checkLength
            let! checkOperation = isOperationSupported parsedValues
            let! checkDividingByZero = isDividingByZero checkOperation
            return checkDividingByZero
       }