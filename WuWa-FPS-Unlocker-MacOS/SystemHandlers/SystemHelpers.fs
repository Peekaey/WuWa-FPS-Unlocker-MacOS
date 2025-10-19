

module WuWa_FPS_Unlocker_MacOS.SystemHandlers.SystemHelpers

    open System
    open System.Diagnostics
    open System.IO
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

    
    let deleteLocalStorageFile (localStoragePath : string ) : OperationResult =
        try
            if File.Exists(localStoragePath) then
                File.Delete(localStoragePath)
                OperationSuccess
            else
                OperationSuccess
        with
        | ex ->
            OperationFailure $"Failed to delete LocalStorage.db file: {ex.Message}"
            
    // No need to return the backup FilePath as we will assume that the backup file is always called
    // LocalStorage-backup        
    let backupLocalStorageFile (localStoragePath: string) : BackupResult =
        try
            let directory = Path.GetDirectoryName localStoragePath
            let backupFileName = directory + "/" + "LocalStorage-backup.db"
            File.Copy(localStoragePath, backupFileName, true)
            BackupSuccess backupFileName
        with
        | ex ->
            BackupFailure $"Failed to backup the LocalStorage.db file {ex.Message}"
    
    let copyLocalStorageFile (localStoragePath: string) : OperationResult =
        try
            let directory = Path.GetDirectoryName(localStoragePath)
            let backup = directory + "LocalStorage-backup.db"
            File.Copy(backup, localStoragePath, true)
            OperationSuccess
        with
        | ex ->
            OperationFailure $"Failed to copy paste the backup of the LocalStorage.db file: {ex.Message}"
            
    let restoreBackupOfLocalStorageFile (localStoragePath: string) : OperationResult =
        try
            // Attempt to just copy paste the file directly
            match copyLocalStorageFile(localStoragePath) with
            | OperationSuccess ->
                OperationSuccess
            | OperationFailure error ->
                // However if unable to do so, delete the file instead
                match deleteLocalStorageFile(localStoragePath) with
                | OperationFailure error ->
                    OperationFailure $"Failed to copy paste and delete LocalStorage.db file, Please check that the file is not currently being used"
                | OperationSuccess ->
                    // If successful, attempt to copy paste the backup again
                    match copyLocalStorageFile(localStoragePath) with
                    | OperationFailure error ->
                        OperationFailure $"Delete successfull, however failed to restore LocalStorage.db file : {error}"
                    | OperationSuccess ->
                        OperationSuccess
        with
        | ex ->
            OperationFailure $"Failed to restore the backup of the LocalStorage.db file: {ex.Message}"
            