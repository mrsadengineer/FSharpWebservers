module File7



open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters

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
    
    let appa =  // We'll define a single GET route at the / endpoint that returns "Hello World"
        choose [
            GET >=> 
        
                path "/" >=> request (fun _ -> OK "Hello World!")
                path "/f" >=> request (fun _ -> OK "Hello Friend!")
                GET >=> path "/" >=> Files.file "index.html"
                GET >=> path "/" >=> Files.file "about.html"
                GET >=> Files.browseHome
                RequestErrors.NOT_FOUND "Page not found." 

          //  POST >=> //maynot be possible because CORS is not configured 

               //POST >=> path "/postzone.html" >=> request (fun _ -> OK "posting processing......!")
               //POST >=> path "/index.html" >=> request (fun _ -> OK "posting processing......!")
             //  POST >=> path "/postzone.html"  >=>  OK "posting......"
              // POST >=> path "/postzone.html" >=>  OK "posting......"  
          ]



    let app = 
        let setCORSHeaders =
            Writers.addHeader  "Access-Control-Allow-Origin" "*" 
            >=> Writers.setHeader "Access-Control-Allow-Headers" "token" 
            >=> Writers.addHeader "Access-Control-Allow-Headers" "content-type" 
            >=> Writers.addHeader "Access-Control-Allow-Methods" "GET,POST,PUT"    
        //let hello = OK ("hello ")
        let indexRequest = OK ("hello something index request")
        let dllFilesRequest = OK ("hello " )
        let staticFilesRequest = OK ("hello staticFilesRequest here")
        let runSomething = OK ("hello posting")
        let postZoneRequrest = OK ("Success: posting......")
     
        choose [
            GET >=>
                fun context ->
                    context |> (
                        setCORSHeaders
                        >=> choose
                            [ 
                            pathRegex "(.*?)\.(dll|mdb|log)$"  >=> dllFilesRequest
                            //pathRegex "(.*?)\.(html|css|js|png|jpg|ico|bmp)$"  >=> staticFilesRequest 
                            //path "/"  >=> indexRequest
                            GET >=> path "/" >=> Files.file "index.html"
                            GET >=> Files.browseHome
                            GET >=> path "/" >=> Files.file "about.html"
                            GET >=> Files.browseHome

                            path "/index"  >=> indexRequest
                            //path "/static" >=> staticFilesRequest
                            // ...
                            path "/" >=> request (fun _ -> OK "Hello World with nothing!")
                            path "/f" >=> request (fun _ -> OK "Hello Friend!")
                         
                            RequestErrors.NOT_FOUND "Page not found." 
                            ] )
        
            POST >=>
                fun context ->
                    context |> (
                        setCORSHeaders
                        >=> choose
                            [
                            path "/something" >=> runSomething
                            path "/postzone.html" >=> postZoneRequrest
                            // ...
                            ] )
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



