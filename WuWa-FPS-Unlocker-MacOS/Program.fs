// For more information see https://aka.ms/fsharp-console-apps


[<EntryPoint>]
let main argv =
    let exitCode = ApplicationOrchestrator.executeApplication()
    exit exitCode


        
