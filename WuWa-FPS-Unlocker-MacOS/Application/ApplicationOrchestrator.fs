
module ApplicationOrchestrator

    open System
    open DatabaseHelpers
    open Types
    open WuWa_FPS_Unlocker_MacOS.SystemHandlers
    open WuWa_FPS_Unlocker_MacOS.SystemHandlers.SystemHelpers

    
    let printErrorAndExit (error: string) =
        printfn $"Error occured when running the unlock tool: {error}"
        printfn $"Exiting the application tool..."
    
    let printError (error: string) =
        printfn $"Error occured when running the unlock tool: {error}"

    let initialiseApplication () : InitialiseResult =
        printfn "Starting Uncapping of FPS for Wuthering Waves..."
        printfn "Please note that this utility was only designed for operating on MacOS and not windows"
        
        match checkForWuWaRunning() with
        | Running ->
            InitialiseFail $"Wuthering waves is running, Please close the game and rerun this application"
        | NotRunning ->
            printfn "Confirmed that Wuthering Waves is not running"
        
            match checkLocalStorageExists() with
            | NotFound ->
                InitialiseFail $"LocalStorage file does not exist. Please ensure that you have run the game at least once"
            | Found localStorage ->
                 printfn "LocalStorage.db file was found in"
                 printfn $"{localStorage}"
                 printfn "Continuing Application..."
                 InitialiseSuccess localStorage

    let executeUnlockFramework (localStoragePath: string) : UnlockResult =
        
        match executeUpdateMaxRefreshRate localStoragePath with
        | Error error ->
            UnlockFail $"{error}"
        | DatabaseError error ->
            UnlockFail $"{error}"
        | DatabaseSuccess ->
            UnlockSuccess
            
    let restoreBackup (localStoragePath: string) : OperationSuccess =
        restoreBackupOfLocalStorageFile(localStoragePath)
    
    let executeApplication () : int =
        
        match initialiseApplication() with
        | InitialiseFail error ->
            printErrorAndExit(error)
            1
        | InitialiseSuccess localStoragePath ->
            
            match backupLocalStorageFile(localStoragePath) with
                | Failure error ->
                    printErrorAndExit(error)
                    1
                | Success ->
                    printfn "Backup successfully taken of LocalStorage.db file"
                    printfn "Continuing application execution"
                            
                    match executeUnlockFramework(localStoragePath) with
                        | UnlockFail error ->
                            printError(error)
                            printfn("Attempting to restore backup...")
                            
                            match restoreBackup(localStoragePath) with
                            | Failure error ->
                                printErrorAndExit(error)
                                1
                            | Success ->
                                printfn "Restore of backup successfull"
                                1
                        | UnlockSuccess ->
                            printfn $"Unlock successful, FPS Cap has been set to 120 FPS"
                            0
                    