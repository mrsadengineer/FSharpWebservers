// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    printfn "Learning Suave in from F#!"

    SuaveWebServers.startSuaveWebserver argv
    0 // return an integer exit code
