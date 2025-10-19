
module ApplicationOrchestrator

    open System
    open DatabaseHelpers
    open Types
    open WuWa_FPS_Unlocker_MacOS.SystemHandlers
    open WuWa_FPS_Unlocker_MacOS.SystemHandlers.SystemHelpers
    open Spectre.Console
    open MessageHelpers
    
    let initialiseApplication () : InitialiseResult =
        printStartupMessage()
        
        match checkForWuWaRunning() with
        | Running ->
            InitialiseFail $"Wuthering waves is running, Please close the game and rerun this application"
        | NotRunning ->
            AnsiConsole.MarkupLine("[green]Confirmed that Wuthering Waves is not running[/]")
        
            match checkLocalStorageExists() with
            | NotFound ->
                InitialiseFail $"LocalStorage file does not exist. Please ensure that you have run the game at least once"
            | Found localStorage ->
                 printFoundLocalStorage(localStorage)
                 InitialiseSuccess localStorage

    let executeUnlockFramework (localStoragePath: string) : UnlockResult =
        
        match executeUpdateMaxRefreshRate localStoragePath with
        | Error error ->
            UnlockFail $"{error}"
        | DatabaseError error ->
            UnlockFail $"{error}"
        | DatabaseSuccess ->
            UnlockSuccess
            
    let restoreBackup (localStoragePath: string) : OperationResult =
        restoreBackupOfLocalStorageFile(localStoragePath)
    
    let executeApplication () : int =
        
        match initialiseApplication() with
        | InitialiseFail error ->
            printErrorAndExit(error)
            1
        | InitialiseSuccess localStoragePath ->
            
            match backupLocalStorageFile(localStoragePath) with
                | BackupFailure error ->
                    printErrorAndExit(error)
                    1
                | BackupSuccess backupPath ->
                    printBackupSuccessfulMessage(backupPath)
                            
                    match executeUnlockFramework(localStoragePath) with
                        | UnlockFail error ->
                            printError(error)
                            AnsiConsole.MarkupLine("[grey37]Attempting to restore backup...[/]")
                            
                            match restoreBackup(localStoragePath) with
                            | OperationFailure error ->
                                printErrorAndExit(error)
                                1
                            | OperationSuccess ->
                                AnsiConsole.MarkupLine("[green]Restore of backup successful[/]")
                                1
                        | UnlockSuccess ->
                            AnsiConsole.MarkupLine("[green]SUCCESS: Unlock successful, FPS Cap has been set to 120 FPS[/]")
                            0
                    