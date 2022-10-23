module Hw6.App

open System
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
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

let webApp =
    choose [
        GET >=> choose [ 
             route "/" >=> text "Use //calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0