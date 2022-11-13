module Hw4Tests.ParserTests

open System
open Hw4.Calculator
open Hw4.Parser
open Microsoft.FSharp.Core
open Xunit
        
[<Theory>]
[<InlineData("+", CalculatorOperation.Plus)>]
[<InlineData("-", CalculatorOperation.Minus)>]
[<InlineData("*", CalculatorOperation.Multiply)>]
[<InlineData("/", CalculatorOperation.Divide)>]
let ``+, -, *, / parsed correctly`` (operation, operationExpected) =
    //arrange
    let args = [|"15";operation;"5"|]
   
    //act
    let options = parseCalcArguments args
    
    //assert
    Assert.Equal(operationExpected, options.operation)
    
[<Theory>]
[<InlineData("f", "+", "3")>]
[<InlineData("3", "+", "f")>]
[<InlineData("a", "+", "f")>]
let ``Incorrect values throw ArgumentException`` (val1, operation, val2) =
    // arrange
    let args = [|val1;operation;val2|]
    
    // act/assert
    Assert.Throws<ArgumentException>(fun () -> parseCalcArguments args |> ignore)

[<Fact>]
let ``Incorrect operations throw ArgumentException``() =
    // arrange
    let args = [|"3";".";"4"|]
    
    // act/assert
    Assert.Throws<ArgumentException>(fun () -> parseCalcArguments args |> ignore)

[<Fact>]
let ``Incorrect argument count throws ArgumentException``() =
    // arrange
    let args = [|"3"; "."; "4"; "5"|]
    
    // act/assert
    Assert.Throws<ArgumentException>(fun () -> parseCalcArguments args |> ignore)
    
[<Theory>]
[<InlineData(5.5)>]
[<InlineData(7.0)>]
[<InlineData(0.02)>]
[<InlineData(-12.000001)>]
[<InlineData(0.0)>]
let ``First value parsed correctly`` firstValueExpected =
    //arrange
    let args = [|firstValueExpected.ToString(); "+"; "15"|]
    
    //act
    let options = parseCalcArguments args
    
    //assert
    Assert.Equal(firstValueExpected, options.arg1)
    
[<Theory>]
[<InlineData(10.035)>]
[<InlineData(0.0)>]
[<InlineData(-2.1)>]
[<InlineData(-12.7)>]
[<InlineData(21.2)>]
let ``Second value parsed correctly``(secondValueExpected) =
    //arrange
    let args = [|"12"; "*"; secondValueExpected.ToString()|]
    
    //act
    let options = parseCalcArguments args
    
    //assert
    Assert.Equal(secondValueExpected, options.arg2)