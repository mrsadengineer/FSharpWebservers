module SuaveAdvanceWebServers
open Suave
open Suave.Operators
open Suave.Filters
open Suave.Successful
open System
open System.IO
open System.Threading
open Suave.Logging






//Async Web Servers 
//Advance Custom Config: Logger, MimiTypes, localhome, port
//Advance Routing: - GET routes '/'
//no static file getting, but creating config entry for homeFolder
let run00WebServerWCustomConfigCustomRoutesAdvance argv = 
    let websitepath2 = "./Websites/website" //homeFolder Starting Location
    printfn "Hello Config HomeFolder and custom routes !"
    let cts = new CancellationTokenSource()
    //logger
    let logger = Targets.create Verbose [||]
    // Adds a new mime type to the default map
    let mimeTypes =
      Writers.defaultMimeTypesMap
        @@ (function | ".avi" -> Writers.createMimeType "video/avi" false | _ -> None)
    //port
    let port = 8080   
    let cfg = // create an app config with the port
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
            homeFolder            = Some (Path.GetFullPath websitepath2)
            compressedFilesFolder = None
            logger                = logger
            tcpServerFactory      = new DefaultTcpServerFactory()
            cookieSerialiser      = new BinaryFormatterSerialiser()
            tlsProvider           = new DefaultTlsProvider()
            hideHeader            = false
            maxContentLength      = 1000000 }

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
    let listening, server = startWebServerAsync cfg myApp 
    Async.Start(server, cts.Token)

    // wait for the server to start listening
    listening |> Async.RunSynchronously |> ignore
    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()




//using homeFolder; understanding browswHome
//simple custom config AND custom app route path for '/'
//testing browseHome
//shows that this app can browse from homeFolder and recieve text from url
let run01WebServerWCustomConfigStaticFilesFromBrowserHome argv = 
    printfn "Hello Config HomeFolder and custom routes !"


    let websitepath2 = "./Websites/website" 

    let cts = new CancellationTokenSource()
    let logger = Targets.create Verbose [||]
    let port = 8080  // Define the port where you want to serve. We'll hardcode this for now.
    let cfg = // create an app config with the port
    
          { defaultConfig with
              bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]
              bufferSize = 2048
              maxOps     = 10000
              logger     = logger
              homeFolder = Some (Path.GetFullPath websitepath2) }
    //shows that this app can browse from homeFolder and recieve text from url
    let app =  // We'll define a single GET route at the / endpoint that returns "Hello World"
        choose
            [ 
              //path "/" >=> request (fun _ -> OK "Hello World!")
            path "/friend" >=> request (fun _ -> OK "Hello, My friend!")
            //GET >=> path "/" >=> Files.file "index.html"
            GET >=> Files.browseHome
            RequestErrors.NOT_FOUND "Page not found."             
          ]

    //let listening, server = startWebServerAsync cfg (choose [ GET >=> browseHome ]) //might can do it with oneline. need to check/test
    let listening, server = startWebServerAsync cfg app //(choose [ GET >=> (Successful.OK "Hello World! startwebswerver async") ])
    Async.Start(server, cts.Token)

    printf("http://127.0.0.1:8080/index.html \n")
    Console.WriteLine("Home Folder is located: ")
    Console.WriteLine(cfg.homeFolder.Value)
    Console.WriteLine("Purpose: Test Static Browse at homeFolder ")
    Console.WriteLine("Purpose: test path /friend ")
    Console.WriteLine("visit <http://127.0.0.1:8080/index.html> to test this webserver")
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> ignore

    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()






//Full Website: No CORS - simple custom config AND custom and advance routes with static and url paths. AND multiple static site
//more protection in securing from addresss based attacks
//--single http operation: GET
//--checking for file types
//--no page found
//--RequestErrors.FORBIDDEN
//--RequestErrors.NOT_FOUND
//css
let run02WebServerWFullStaticSiteGet argv = 
    printfn "Hello Config HomeFolder and custom routes !"

    
    let websitepath1 = "./Websites/StaticWebsite"
    let cts = new CancellationTokenSource()
    let logger = Targets.create Verbose [||]
    let port = 8080    // Define the port where you want to serve. We'll hardcode this for now.
    let cfg = // create an app config with the port
    
          { defaultConfig with
              bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port]
              bufferSize = 2048
              maxOps     = 10000
              logger     = logger
              homeFolder = Some (Path.GetFullPath websitepath1) }
    
    let app =  // We'll define a single GET route at the / endpoint that returns "Hello World"
        choose
            [GET >=> choose
                [ 
                //path "/" >=> request (fun _ -> OK "Hello World!")
                path "/friend" >=> request (fun _ -> OK "Hello My Friend!")
                GET >=> path "/" >=> Files.file "index.html"
                // GET >=> Files.browseHome
                //GET >=> path "/" >=> Files.file "about.html"
                GET >=> Files.browseHome
                pathRegex "(.*?)\.(dll|mdb|log)$"  >=> RequestErrors.FORBIDDEN "Access denied."
                //pathRegex "(.*?)\.(html|css|js|png|jpg|ico|bmp)$"  >=> staticFilesRequest 
                path "/"  >=> Redirection.redirect "/index.html"// (//indexRequest
                //GET >=> path "/" >=> Files.file "index.html"
                //GET >=> Files.browseHome
                //GET >=> path "/" >=> Files.file "about.html"
                //GET >=> Files.browseHome

                //path "/index"  >=> indexRequest
                //path "/static" >=> staticFilesRequest
                // ...
                //path "/" >=> request (fun _ -> OK "Hello World with nothing!")
                //path "/f" >=> request (fun _ -> OK "Hello Friend!")
                         
                RequestErrors.NOT_FOUND "Page not found." 
                // POST >=> path "/" >=> request (fun _ -> OK "posting processing......!")
                ]   
            ]

         

    //// Now we start the server
    let listening, server = startWebServerAsync cfg app 
    Async.Start(server, cts.Token)
    Console.WriteLine("visit <http://127.0.0.1:8080/index.html> to test this webserver")
    // wait for the server to start listening
    listening |> Async.RunSynchronously |> ignore

    printfn "Make requests now or press key to exit webserver from terminal"
    Console.ReadKey true |> ignore
    ////kill the server
    cts.Cancel()



//Full Website: with CORS - simple custom config AND custom app route path for '/'
let run03WebServerWFullCORSCapabileStaticSite argv = 
    printfn "Hello Config HomeFolder and custom routes !"



    let websitepath3 = "./Websites/CORSWebsite"

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
              homeFolder = Some (Path.GetFullPath websitepath3) }
    
    let appa =  // We'll define a single GET route at the / endpoint that returns "Hello World"
        choose [
            GET >=> 
        
                //path "/" >=> request (fun _ -> OK "Hello World!")
                path "/friend" >=> request (fun _ -> OK "Hello, My friend!")
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


//CORS Example - advance web server option, setting CORS and http headers (4.)
let run04AdvanceWebServer argv = 
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
    
//CORS Example - advance web server option, setting CORS, http headers, pathRegex (5.)
let run05AdvanceWebServer2 argv =
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



    //look into suave.html