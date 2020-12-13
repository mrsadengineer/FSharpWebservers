module File1

open Suave
open Suave.Operators
open Suave.Filters
open System.Threading
open System



//simple default config AND Hello World 
let runWebServerAsyncDefaultConfigAndReturnHello argv = 
    printfn "F# Web and Listening Servers! With custom config and default hello world!"
    let cts = new CancellationTokenSource()
    Console.WriteLine("Intializing Async Suave Web Server: server and listening")
    let listening, server = startWebServerAsync defaultConfig (choose [ GET >=> (Successful.OK "Hello World! startwebswerver async") ])
    Async.Start(server, cts.Token)
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> printfn "start stats: %A"

    printfn "Make web requests now"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()
