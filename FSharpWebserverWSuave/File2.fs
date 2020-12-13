module File2 //-Async

open Suave
open Suave.Operators
open Suave.Filters
open Suave.Logging
open System.Threading
open System



//simple custom config AND simple hello world 
let runWebServerAsyncCustomConfigAndReturnHello argv = 
    printfn "FILE2: Custom Config and Simple Hello World Text"
    let cts = new CancellationTokenSource()
    let logger = Targets.create Verbose [||]
   // Define the port where you want to serve. We'll hardcode this for now.
    let port = 8080   
    let cfg = // create an app config with the port
    
          { defaultConfig with
              bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]
              bufferSize = 2048
              maxOps     = 10000
              logger     = logger}


    let listening, server = startWebServerAsync cfg (choose [ GET >=> (Successful.OK "Hello World! File2 async") ])
    Async.Start(server, cts.Token)
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> printfn "start stats: %A"

    printfn "Make requests now"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()
