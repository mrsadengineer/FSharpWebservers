module SuaveAdvanceWebServers
open Suave

open Suave.Operators

open Suave.Filters

open Suave.Successful

///addvance configuation implementation
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

