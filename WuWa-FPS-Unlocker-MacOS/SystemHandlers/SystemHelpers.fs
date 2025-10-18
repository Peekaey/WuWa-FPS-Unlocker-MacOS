

module WuWa_FPS_Unlocker_MacOS.SystemHandlers.SystemHelpers

    open System
    open System.Diagnostics
    open System.IO
    open MessageDisplay
    open Types

    // We follow the convention that the user installs the native Wuthering Waves application from the App Store,LocalStorage is then stored in users file path
    // Example path:
    // /Users/UserName/Library/Containers/com.kurogame.wutheringwaves.global/Data/Library/Client/Saved/LocalStorage/LocalStorage.db
    let getLocalStorageDBPath () =
        let homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        $"{homeDir}/Library/Containers/com.kurogame.wutheringwaves.global/Data/Library/Client/Saved/LocalStorage/LocalStorage.db"
    
    let checkLocalStorageExists () : LocalStorageResult =
        let path = getLocalStorageDBPath()
        if (File.Exists(path)) then
            Found path
        else
            NotFound
        
    let checkForWuWaRunning () : WuwaRunningResult =
        let wuwaProccessName = "Client-Mac-Shipping"
        let wuwaExecutablePath = "/Applications/WutheringWaves.app/Contents/MacOS/Client-Mac-Shipping"
        let processes = Process.GetProcessesByName(wuwaProccessName)
        
        match processes with
        //  | [||] -> Matches an empty array
        | [||] ->

            NotRunning
        // | _ -> is wildcard pattern which matches anything else
        | _ ->
            let wuwaFound =
                // |>  -- Pipe Operator
                // Passes the result from the left side as the last arguement to the function on the right
                // fun proc  -- Lambda Function
                // anonymous function - proc becomes the object
                processes |> Array.exists (fun proc ->
                    try
                        proc.MainModule.FileName.Equals(wuwaExecutablePath)
                        with
                        // | _ -> Wildcard in Pattern Matching
                        // matches anything we don't care about (equivalent of just else)
                        | _ -> false)
                
            if wuwaFound then
                Running
            else
                NotRunning

    
    let deleteLocalStorageFile (localStoragePath : string ) : OperationSuccess =
        try
            if File.Exists(localStoragePath) then
                File.Delete(localStoragePath)
                if File.Exists(localStoragePath) then
                    Failure
                else
                    Success
            else
                Success
        with
        | ex ->
            printDeleteLocalStorageError(ex.Message)
            Failure
            
    // let backupLocalStorageFile (localStoragePath: string) : OperationSuccess =
    //     try
    //         File.Copy()
    //     