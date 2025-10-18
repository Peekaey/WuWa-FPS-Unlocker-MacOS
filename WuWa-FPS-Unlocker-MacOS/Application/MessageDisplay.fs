
module MessageDisplay


    let printFileStatus (exists: bool) =
       if exists then
            printfn "LocalStorage file was found at"
       else
            printfn "LocalStorage file was not found"
            printfn "Exiting Application..."
       
    let printFilePathLocation (path: string) =
        printfn $"LocalStorage.db found at: {path}"

    let printDeleteLocalStorageError (message: string) =
            printfn $"Error deleting LocalStorage File with error {message}"
            printfn "Exiting Program..."