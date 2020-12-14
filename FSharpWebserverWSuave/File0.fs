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

