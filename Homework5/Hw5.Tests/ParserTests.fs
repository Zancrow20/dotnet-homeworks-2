module Hw5Tests.ParserTests

open Hw5
open Hw5.Parser
open Calculator
open Microsoft.FSharp.Core
open Xunit
open Xunit.Sdk
open FsCheck


let epsilon: decimal = 0.001m


let errors = Map [[|"a"; "+"; "5"|],Message.WrongArgFormat
                  [|"k"; "+"; "1"|],Message.WrongArgFormat
                  [|"f"; "+"; "9"|],Message.WrongArgFormat
                  [|"3"; "."; "4"|],Message.WrongArgFormatOperation
                  [|"5"; "_"; "3"|],Message.WrongArgFormatOperation
                  [|"3";"+";"4";"5"|], Message.WrongArgLength
                  [|"4";"+";"2";"+";"12"|], Message.WrongArgLength
                  [|"3";"/";"0"|], Message.DivideByZero
                  [|"0";"/";"0"|], Message.DivideByZero]

let argsReturnErrorsProperty =
    for KeyValue(args,message) in errors do
        
        let result = parseCalcArguments args
        
        match result with
        | Ok _ -> XunitException $"Should return {message}, but return Ok" |> raise
        | Error errorMessage -> Assert.Equal(errorMessage,message)

[<Fact>]
let ``Incorrect data return Error`` () =
    Check.Quick argsReturnErrorsProperty


[<Theory>]
[<InlineData("15", "+", "5", 20)>]
[<InlineData("15", "-", "5", 10)>]
[<InlineData("15", "*", "5", 75)>]
[<InlineData("15", "/", "5",  3)>]
[<InlineData("15.6", "+", "5.6", 21.2)>]
[<InlineData("15.6", "-", "5.6", 10)>]
[<InlineData("15.6", "*", "5.6", 87.36)>]
[<InlineData("15.6", "/", "5.6", 2.7857)>]
let ``values parsed correctly`` (value1, operation, value2, expectedValue) =
    //arrange
    let values = [|value1;operation;value2|]
    
    //act
    let result = parseCalcArguments values
    
    //assert
    match result with
    | Ok resultOk ->
        match resultOk with
        | arg1, operation, arg2 -> Assert.True((abs (expectedValue - calculate arg1 operation arg2)) |> decimal < epsilon)
    | Error err -> XunitException $"Should return Ok, but return {err}" |> raise
        
[<Theory>]
[<InlineData("f", "+", "3")>]
[<InlineData("3", "+", "f")>]
[<InlineData("a", "+", "f")>]
let ``Incorrect values return Error`` (value1, operation, value2) =
    //arrange
    let args = [|value1;operation;value2|]
    
    //act
    let result = parseCalcArguments args
    
    //assert
    match result with
    | Ok _ -> XunitException $"Should return {Message.WrongArgFormat}, but return Ok" |> raise
    | Error resultError -> Assert.Equal(resultError, Message.WrongArgFormat)
    
[<Fact>]
let ``Incorrect operations return Error`` () =
    //arrange
    let args = [|"3";".";"4"|]
    
    //act
    let result = parseCalcArguments args
    
    //assert
    match result with
    | Ok _ -> XunitException $"Should return {Message.WrongArgFormatOperation}, but return Ok" |> raise
    | Error resultError -> Assert.Equal(resultError, Message.WrongArgFormatOperation)
    
[<Fact>]
let ``Incorrect argument count throws ArgumentException`` () =
    //arrange
    let args = [|"3";"+";"4";"5"|]
    
    //act
    let result = parseCalcArguments args
    
    //assert
    match result with
    | Ok _ -> XunitException $"Should return {Message.WrongArgLength}, but return Ok" |> raise
    | Error resultError -> Assert.Equal(resultError, Message.WrongArgLength)
    
[<Fact>]
let ``any / 0 -> Error(Message.DivideByZero)`` () =
    //arrange
    let args = [|"3";"/";"0"|]
    
    //act
    let result = parseCalcArguments args
    
    //assert
    match result with
    | Ok _ -> XunitException $"Should return {Message.DivideByZero}, but return Ok" |> raise
    | Error resultError -> Assert.Equal(resultError, Message.DivideByZero)