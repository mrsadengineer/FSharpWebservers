module SuaveSimpleWebServers

open Suave
open Suave.Operators
open Suave.Filters
open Suave.Successful
open System.Threading
open System
open Suave.Logging



//async example using constellation tokens
let runAsyncWCT argv =
     let cts = new CancellationTokenSource()
     let conf = { defaultConfig with cancellationToken = cts.Token }
     let listening, server = startWebServerAsync conf (Successful.OK "Hello World")
       
     Async.Start(server, cts.Token)
     printfn "Make requests now"
     Console.ReadKey true |> ignore
       
     cts.Cancel()

//two server listening and server     
//simple default config AND Hello World //FILE1
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



//logging (in cfg)     
//simple custom config AND simple hello world //FILE2
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


//simple async custom config AND simple custom app route path for '/'
let runWebServerAsyncCustomConfigAndCustomUrlRoutes argv = 
    printfn "Hello World with Web and Listening Servers!"
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
    
    let app =  // We'll define a single GET route at the / endpoint that returns "Hello World"
          choose
            [ GET >=> choose
                [ 
                path "/" >=> request (fun _ -> OK "Hello, World!")
                path "/friend" >=> request (fun _ -> OK "Hello, My friend!")
                ] 
            ]



    let listening, server = startWebServerAsync cfg app // (choose [ GET >=> (Successful.OK "Hello World! startwebswerver async") ])
    Async.Start(server, cts.Token)
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> printfn "start stats: %A"

    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()


