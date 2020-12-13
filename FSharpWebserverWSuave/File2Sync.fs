module File2Sync

open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Files
open Suave.Logging
open System.Threading
open System
open System.Diagnostics


//simple custom config AND custom app route path for '/'
let runWebServerCustomConfigSimpleHelloWOrldReturn argv = 
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


    //let server = 
    startWebServer cfg (choose [ GET >=> (Successful.OK "Hello World! File2 async") ])
   // Async.Start(server, cts.Token)
    // wait for the server to start listening
   
    
    printfn "Make requests now"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()
