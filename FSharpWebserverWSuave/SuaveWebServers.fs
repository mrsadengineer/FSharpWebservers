module SuaveWebServers

// Learn more about F# at http://fsharp.org

open System

// We'll use argv later :)

//[<EntryPoint>]
let startSuaveWebserver argv =
    //level 1 of 3 Simple
    SuaveSimpleWebServers.runAsyncWCT argv




    //Advance configuration
    //FILE7: Custom Config; GET for static files and URLs with CORS 
    //FILE6: Custom Config; GET static files and URLs no CORS (http operations beyound GET) 
    //FILE5: Working GET Static Files from Routes
    //FILE4: Advance Configuration: Config and Routing Apps (nohttp operations beyound GET) 
    
    //simple configuration
    //"FILE3: Custom Config, Multiple URL paths, no CORS (nohttp operations beyound GET) 
    //"FILE2-Sync: Custom Config and Simple Hello World Text" //-sync
    //"FILE2: Custom Config and Simple Hello World Text" //-async
    //FILE1: One line deploy 
    







//###########################################
    //working
    //File1.runWebAndListeningServer argv
    
    //"FILE2: Custom Config and Simple Hello World Text" //-async
    //working
    //-simple custom config
    //listening server? -since async, why not?
    //-prints out start stats 
    //File2.runWebServerCustomConfigSimpleHelloWOrldReturn argv //index on website/app returns "Hello World! startwebswerver async"
    
    //"FILE2-Sync: Custom Config and Simple Hello World Text" //-sync
    //in progress
    //-simple custom config
    //enclosed choose GET


//"FILE3: Custom Config, Multiple URL paths, no CORS (http operations beyound GET) 
    //working
    //-simple custom config
    //-multiple paths for app/website
    ////single http operation - get http operation
    //-async
    //-listenting server? -since async, why not?
    //-prints out start stats
    //-cancels after key pressed
    //File3.runWebServerWCustomConfigCustomRoutes argv
    
    //"FILE3Sync: Custom Config, Multiple URL paths, no CORS (http operations beyound GET) 
      //working
      //-simple custom config
      //-multiple paths for app/website
      ////single http operation - get http operation
      //-async
      //-listenting server? -since async, why not?
      //-prints out start stats
      //-cancels after key pressed
      //File3.runWebServerWCustomConfigCustomRoutes argv

    

    //FILE4: Advance Configuration: 
    //--------------location of homeFolder and other
    //working
    //-config 
    //--homeFolder for static files
    //--logger
     ////single http operation - get http operation
    //-async
    //-listenting server?
    //File4.runWebServerWCustomConfigCustomRoutesBrowsweHome argv


    //FILE5: Working GET Static Files from Routes
    //working 
    //-config 
    //--homeFolder for static files
    //--logger
    //-retrieve static file
    //-async
    ////single http operation - get http operation
    //File5.runWebServerWCustomConfigStaticFilesFromBrowserHome argv

    //FILE6: Custom Config; GET static files and URLs
    //working 
    //-config homeFolder for static files
    //-retrieve static file //if files embedded don't forget to change propteries on the file to copy if newer.
    //-use both url and uri interchangable
    //-static pages are linking toeachother
    //-async
    //-explore a more complex app choosing structure. imbedded. 
    //-comunications between static pages
    //File6.runWebServerWCustomConfigStaticFilesFromBrowserHome argv

    //FILE7: Custom Config; GET for static files and URLs with CORS 
    //working 
    //multipe http operation
    //-testing: post capabilities
    //-config homeFolder for static files
    //-retrieve static file
    //-use both url and uri interchangable
    //-static pages are linking toeachother
    //-async - benefit is that you can run two app async
    //-explore a more complex app choosing structure. imbedded. 
    //-comunications between static pages
    //-get
    //-post
    //CORS configuration is required for post and other routing/http operations
    //File7.runWebServerWCustomConfigStaticFilesFromBrowserHome argv
