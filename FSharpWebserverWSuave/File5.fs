module File5


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

//simple custom config AND custom app route path for '/'
let runWebServerWCustomConfigStaticFilesFromBrowserHome argv = 
    printfn "Hello Config HomeFolder and custom routes !"
    let cts = new CancellationTokenSource()
    let logger = Targets.create Verbose [||]
   // Define the port where you want to serve. We'll hardcode this for now.
    let port = 8080   
    let cfg = // create an app config with the port
    
          { defaultConfig with
              bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]
              bufferSize = 2048
              maxOps     = 10000
              logger     = logger
              homeFolder = Some (Path.GetFullPath "./public") }
    
    let app =  // We'll define a single GET route at the / endpoint that returns "Hello World"
        choose
            [ 
              //path "/" >=> request (fun _ -> OK "Hello World!")
              //path "/f" >=> request (fun _ -> OK "Hello Friend!")
            GET >=> path "/" >=> Files.file "index.html"
            GET >=> Files.browseHome
            RequestErrors.NOT_FOUND "Page not found."             
          ]



    //// Now we start the server
    //startWebServer cfg app
    //let listening, server = startWebServerAsync cfg (choose [ GET >=> browseHome ])
    let listening, server = startWebServerAsync cfg app //(choose [ GET >=> (Successful.OK "Hello World! startwebswerver async") ])
    Async.Start(server, cts.Token)

    printf("http://127.0.0.1:8080/index.html \n")
 
    Console.WriteLine(cfg.homeFolder.Value)
    Console.WriteLine(" ")
    Console.WriteLine("visit <http://127.0.0.1:8080/index.html> to test this webserver")
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> ignore

    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()



