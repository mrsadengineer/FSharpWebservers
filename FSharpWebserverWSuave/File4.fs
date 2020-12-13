module File4
open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Files
open Suave.Logging
open System.Threading
open System
open System.Diagnostics
open System.IO

//Advance Configuration; Async Web Server; Advance GET routes '/'
let runWebServerWCustomConfigCustomRoutes argv = 
    printfn "Hello Config HomeFolder and custom routes !"
    let cts = new CancellationTokenSource()
    let logger = Targets.create Verbose [||]
    // Adds a new mime type to the default map
    let mimeTypes =
      Writers.defaultMimeTypesMap
        @@ (function | ".avi" -> Writers.createMimeType "video/avi" false | _ -> None)
   // Define the port where you want to serve. We'll hardcode this for now.
    let port = 8080   
    let cfg = // create an app config with the port
    
          //{ defaultConfig with
          //    bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]
          //    bufferSize = 2048
          //    maxOps     = 10000
          //    logger     = logger
          //    homeFolder = Some (Path.GetFullPath "./public") }
          { bindings              = [ HttpBinding.createSimple HTTP "127.0.0.1" port
                                    ]
            serverKey             = Utils.Crypto.generateKey HttpRuntime.ServerKeyLength
            errorHandler          = defaultErrorHandler
            listenTimeout         = TimeSpan.FromMilliseconds 2000.
            cancellationToken     = Async.DefaultCancellationToken
            bufferSize            = 2048
            maxOps                = 100
            autoGrow              = true
            mimeTypesMap          = mimeTypes
            homeFolder            = None
            compressedFilesFolder = None
            logger                = logger
            tcpServerFactory      = new DefaultTcpServerFactory()
            cookieSerialiser      = new BinaryFormatterSerialiser()
            tlsProvider           = new DefaultTlsProvider()
            hideHeader            = false
            maxContentLength      = 1000000 }
    
    //let app =  // We'll define a single GET route at the / endpoint that returns "Hello World"
    //    choose
    //        [ GET >=> choose
    //        [ 
    //          path "/" >=> request (fun _ -> OK "Hello World!")
    //          path "/friend" >=> request (fun _ -> OK "Hello Friend!")
    //        ]  
    //    ]

    let myApp =
        choose [
          GET >=> choose
            [ 
            path "/hello" >=> OK "Hello GET" ; 
            path "/goodbye" >=> OK "Good bye GET";
            path "/" >=> request (fun _ -> OK "Hello World!"); 
            path "/friend" >=> request (fun _ -> OK "Hello Friend!")
            ];
          POST >=> choose
            [ path "/hello" >=> OK "Hello POST" ; path "/goodbye" >=> OK "Good bye POST" ];
          DELETE >=> choose
            [ path "/hello" >=> OK "Hello DELETE" ; path "/goodbye" >=> OK "Good bye DELETE" ];
          PUT >=> choose
            [ path "/hello" >=> OK "Hello PUT" ; path "/goodbye" >=> OK "Good bye PUT" ];
        ]

    //// Now we start the server
    //startWebServer cfg app
    //let listening, server = startWebServerAsync cfg (choose [ GET >=> browseHome ])
    let listening, server = startWebServerAsync cfg myApp //(choose [ GET >=> (Successful.OK "Hello World! startwebswerver async") ])
    Async.Start(server, cts.Token)
    //Console.WriteLine(cfg.homeFolder.Value)
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> ignore
    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()



