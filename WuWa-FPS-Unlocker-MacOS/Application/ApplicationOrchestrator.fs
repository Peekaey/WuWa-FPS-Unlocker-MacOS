
module ApplicationOrchestrator

    open System
    open DatabaseHelpers
    open MessageDisplay
    open Types
    open WuWa_FPS_Unlocker_MacOS.SystemHandlers
    open WuWa_FPS_Unlocker_MacOS.SystemHandlers.SystemHelpers

    let exitApplication () =
        exit 1
    
    let initialiseApplication () =
        printfn "Starting Uncapping of FPS for Wuthering Waves..."
        printfn "Please note that this utility was only designed for operating on MacOS and not windows"
        
        match checkForWuWaRunning() with
        | Running ->
            printfn "Wuthering Waves is running"
            printfn "Please close the game and rerun this application"
            exitApplication()
        | NotRunning ->
            printfn "Confirmed that Wuthering Waves is not running"
        
            match checkLocalStorageExists() with
            | NotFound ->
                printfn "LocalStorage file does not exist. Please ensure that you have run the game at least once"
                printfn ""
                printfn "Exiting Application..."
                exitApplication()
            | Found localStorage ->
                 printfn "LocalStorage.db file was found in"
                 printfn $"{localStorage}"
                 printfn "Continuing Application..."
                 localStorage

    
    
    let executeUnlockFramework (localStoragePath: string) =
        
        match executeUpdateMaxRefreshRate localStoragePath with
        | Error error ->
            printfn "Error occured when attempting to update LocalStorage database"
            printfn $"Error: {error}"
            1
        | DatabaseError error ->
            printfn $"Database error during query execution: {error}"
            1
        | DatabaseSuccess ->
            printfn "FPS Cap has been set to 120 FPS"
            0
            
    let executeApplication() =
        let localStoragePath = initialiseApplication()
        executeUnlockFramework(localStoragePath)
        
