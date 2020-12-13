module File0

open System.Diagnostics
open System.Threading
open Suave
open Suave.Logging

//if executing on linux distribution
let execute cmd args =
    use proc = new Process()
    
    proc.StartInfo.FileName         <- cmd
    proc.StartInfo.CreateNoWindow   <- true
    proc.StartInfo.RedirectStandardOutput <- true
    proc.StartInfo.UseShellExecute  <- false
    proc.StartInfo.Arguments        <- args
    proc.StartInfo.CreateNoWindow   <- true
    
    let _ = proc.Start()
    proc.WaitForExit()
    proc.StandardOutput.ReadToEnd()

printfn "Sample Token and Configuration for Ancy!"
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