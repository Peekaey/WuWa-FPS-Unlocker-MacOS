

module MessageHelpers
    open Spectre.Console
    
    let printErrorAndExit (error: string) =
        AnsiConsole.MarkupLine($"[red] Error occured when running the unlock tool: {error} [/]")
        AnsiConsole.MarkupLine($"[yellow] Exiting the application tool...[/]")
    
    let printError (error: string) =
        AnsiConsole.MarkupLine($"[red] Error occured when running the unlock tool: {error} [/]")

    let printFoundLocalStorage(localStorage: string) =
        AnsiConsole.MarkupLine("[green]LocalStorage.db file was found in[/]")
        AnsiConsole.MarkupLine($"[steelblue3]{localStorage}[/]")
        AnsiConsole.MarkupLine("[grey37]Continuing Application...[/]")
        
    let printStartupMessage() =
        AnsiConsole.MarkupLine("[grey37]Starting Uncapping of FPS for Wuthering Waves...[/]")
        AnsiConsole.MarkupLine("[grey37]Please note that this utility was only designed for operating on MacOS[/]")
    
    let printBackupSuccessfulMessage() =       
        AnsiConsole.MarkupLine("[green]Backup successfully taken of LocalStorage.db file[/]")
        AnsiConsole.MarkupLine("[grey37]Continuing Application...[/]")
        


