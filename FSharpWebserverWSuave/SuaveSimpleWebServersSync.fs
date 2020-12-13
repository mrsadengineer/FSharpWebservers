module SuaveSimpleWebServersSync
open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open System.Threading
open System
open Suave.Logging

//one line implementation syncronies 
let startSimple =
    startWebServer defaultConfig (Successful.OK "Hello World!")


//simple custom config AND custom app route path for '/'
let runWebServer argv = 
    printfn "Hello World from F#!"
   // Define the port where you want to serve. We'll hardcode this for now.
    let port = 8080   
    let cfg = // create an app config with the port
    
          { defaultConfig with
              bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]}
    
    let app =  // We'll define a single GET route at the / endpoint that returns "Hello World"
          choose
            [ GET >=> choose
                [ path "/" >=> request (fun _ -> OK "Hello World!")] // printfn "Hello World from F#!" 
            ]
    
    // Now we start the server
    startWebServer cfg app

//simple custom config AND custom app route path for '/' From FILE2 Synce
let runWebServerCustomConfigSimpleHelloWOrldReturnSync argv = 
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

//simple custom config AND custom app route path for '/' from FILE3
let runWebServerWCustomConfigCustomRoutesSync argv = 
    printfn "Hello World with Web and Listening Servers!"
    //let cts = new CancellationTokenSource()
    let logger = Targets.create Verbose [||]
   // Define the port where you want to serve. We'll hardcode this for now.
    let port = 8080   
    let cfg = // create an app config with the port
    
          { defaultConfig with
              bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]
              bufferSize = 2048
              maxOps     = 10000
              logger     = logger}
    
    let app = //choose [] GET is set up restricts it from other operations.  // We'll define a single GET route at the / endpoint that returns "Hello World"
          choose
            [ GET >=> choose
                [ 
                path "/" >=> request (fun _ -> OK "Hello World!")
                path "/f" >=> request (fun _ -> OK "Hello Friend!")
                ] // printfn "Hello World from F#!" 
            ]



    //let listening, server = 
    startWebServer cfg app // (choose [ GET >=> (Successful.OK "Hello World! startwebswerver async") ])
    //Async.Start(server, cts.Token)
    // wait for the server to start listening
    //listening |> Async.RunSynchronously |> printfn "start stats: %A"

    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    //cts.Cancel()


