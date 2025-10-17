// For more information see https://aka.ms/fsharp-console-apps


open System
open System.IO

[<EntryPoint>]
printfn "Starting Uncapping of FPS for Wuthering Waves..."
printfn "Please note that this utility was only designed for operating on MacOS and not windows"

// First we identifiy if the LocalStorage.db exists or not in the users device
let homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
let localStoragePath = $"{homeDir}/Library/Containers/com.kurogame.wutheringwaves.global/Data/Library/Client/Saved/LocalStorage/LocalStorage.db"

if not (File.Exists(localStoragePath)) then
    printfn("LocalStorage file not found")
    exit 1
else
    printfn("LocalStorage file was found")
    
    
    
        
