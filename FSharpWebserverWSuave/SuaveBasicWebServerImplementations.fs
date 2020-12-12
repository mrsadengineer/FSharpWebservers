module SuaveBasicWebServerImplementations

open System

open Suave

open Suave.Operators

open Suave.Filters

open Suave.Successful

open System.Threading 

//1. one line implementation
let startSimple =
    startWebServer defaultConfig (Successful.OK "Hello World!")


//2. simple custom config AND custom app route path for '/'
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

//3. medium async example using constellation tokens
let runAsyncWCT argv =
     let cts = new CancellationTokenSource()
     let conf = { defaultConfig with cancellationToken = cts.Token }
     let listening, server = startWebServerAsync conf (Successful.OK "Hello World")
       
     Async.Start(server, cts.Token)
     printfn "Make requests now"
     Console.ReadKey true |> ignore
       
     cts.Cancel()

//4. advance web server option, setting CORS and http headers 
let runAdvanceWebServer argv = 
    let hello name = OK ("hello " + name)
    
    let setServerHeader =
        Writers.setHeader "Access-Control-Allow-Origin" "*"  >=>
        Writers.setHeader "server" "kestrel + suave" >=>
        Writers.setHeader "Access-Control-Allow-Headers" "content-type" >=>
        Writers.setHeader "Access-Control-Allow-Methods" "POST, GET, OPTIONS, DELETE, PATCH"
    
    
    let app =
        setServerHeader
        >=> choose [
        path "/" >=> hello "world"
        path "/api" >=> NO_CONTENT
        path "/api/users" >=> OK "users"
      ]

    startWebServer defaultConfig app
    
//5. advance web server option, setting CORS, http headers, pathRegex 
let runAdvanceWebServer2 argv =
    let setCORSHeaders =
        Writers.addHeader  "Access-Control-Allow-Origin" "*" 
        >=> Writers.setHeader "Access-Control-Allow-Headers" "token" 
        >=> Writers.addHeader "Access-Control-Allow-Headers" "content-type" 
        >=> Writers.addHeader "Access-Control-Allow-Methods" "GET,POST,PUT"    
    //let hello = OK ("hello ")
    let indexRequest = OK ("hello something index request")
    let dllFilesRequest = OK ("hello " )
    let staticFilesRequest = OK ("hello staticFilesRequest")
    let runSomething = OK ("hello staticFilesRequest")
    let app =
        choose [
            GET >=>
                fun context ->
                    context |> (
                        setCORSHeaders
                        >=> choose
                            [ 
                            pathRegex "(.*?)\.(dll|mdb|log)$"  >=> dllFilesRequest
                            pathRegex "(.*?)\.(html|css|js|png|jpg|ico|bmp)$"  >=> staticFilesRequest 
                            path "/"  >=> indexRequest
                            path "/index"  >=> indexRequest
                            path "/static" >=> staticFilesRequest
                            // ...
                            ] )
    
            POST >=>
                fun context ->
                    context |> (
                        setCORSHeaders
                        >=> choose
                            [
                            path "/something" >=> runSomething
                            // ...
                            ] )
        ]
    startWebServer defaultConfig app

