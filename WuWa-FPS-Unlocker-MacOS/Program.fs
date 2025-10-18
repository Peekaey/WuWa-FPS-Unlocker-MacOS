// For more information see https://aka.ms/fsharp-console-apps


open System
open System.IO
open WuWa_FPS_Unlocker_MacOS.SystemHandlers


[<EntryPoint>]
let main argv =
    let exitCode = ApplicationOrchestrator.executeApplication()
    exit exitCode


        
