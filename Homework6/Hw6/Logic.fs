module Hw6.Logic
open Microsoft.AspNetCore.Http
open Giraffe
open Hw6.Calculator
open Hw6.Parser
open Hw6.Message

[<CLIMutable>]
type Values =
    {
        value1: string
        operation: string
        value2:string
    }

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let calculatorHandler: HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        let values = ctx.BindQueryString<Values>()
        let args = [|values.value1; convertToOperation values.operation;values.value2|]
        let parsedValues = parseCalcArguments args
        let result =
            match parsedValues with
            | Ok ok -> Ok (ok |||> calculate)
            | Error error -> Error error
        let message = convertMessageToString (result , args)
        match message with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx