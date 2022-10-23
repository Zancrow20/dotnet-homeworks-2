
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
module Hw6.Client.Program

open System
open System.Net.Http
open System.Diagnostics.CodeAnalysis
open System.Threading.Tasks
open Microsoft.FSharp.Control

[<ExcludeFromCodeCoverage>]
let httpClient = new HttpClient()

[<ExcludeFromCodeCoverage>]
let setUri localhostPort firstValue operation secondValue =
    $"http://localhost:{localhostPort}/calculate?value1={firstValue}&operation={operation}&value2={secondValue}"

[<ExcludeFromCodeCoverage>]
let checkForPlusOperation operation =
    match operation with
    | "+" -> "%2b"
    | _ -> operation
    
[<ExcludeFromCodeCoverage>]
let getResponseAsync(uri : string) =
    async
        {
         let! response = httpClient.GetAsync(uri) |> Async.AwaitTask
         let result = response.Content.ReadAsStringAsync()
                      |> Async.AwaitTask
                      |> Async.RunSynchronously   
         do! Task.Delay(600) |> Async.AwaitTask
           
         match response.IsSuccessStatusCode with
         | true -> $"Ok: {result}"
         | false ->  $"Bad request: {result}"
         return result
        }
    
[<ExcludeFromCodeCoverage>]
[<EntryPoint>]    
let main _ =
    printfn "Please enter localhost port"
    let localhostPort = Console.ReadLine()
    use httpClient = new HttpClient()
    while(Console.ReadLine() <> "exit") do
        printfn "<firstValue> <operation> <secondValue>"
        let values = Console.ReadLine().Split(' ')
        let args = 
            match Array.length values with
            | 3 -> [|$"{values[0]}"; $"{checkForPlusOperation values[1]}";$"{values[2]}"|]
            | _ -> [|"";"";""|]
        let responseAsync = setUri localhostPort args[0] args[1] args[2] |> getResponseAsync 
        printfn $"{responseAsync |> Async.RunSynchronously}"
    0