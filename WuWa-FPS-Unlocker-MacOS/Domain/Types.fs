
module Types

type LocalStorageResult =
    | Found of path: string
    | NotFound
    
    
type WuwaRunningResult =
            | Running
            | NotRunning
            
type DatabaseResult = 
    | DatabaseSuccess
    | DatabaseError of string
    | Error of string

type OperationSuccess =
    | Success
    | Failure