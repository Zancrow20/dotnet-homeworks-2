module Hw4.Program

open Hw4.Calculator
open Hw4.Parser

[<EntryPoint>]
let main args =
    let parsedArgs = parseCalcArguments args
    printf $"{calculate parsedArgs.arg1 parsedArgs.operation parsedArgs.arg2}"
    0