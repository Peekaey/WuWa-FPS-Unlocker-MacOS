
module DatabaseHelpers
    open Microsoft.Data.Sqlite
    open Queries
    open Types

    let queriesList () =
        [
            getDeleteCustomFramerateTriggerQuery().Trim()
            getInsertCustomFramerateTriggerQuery().Trim()
            getUpdateCustomFramerateQuery().Trim()
            getDeleteDependencyKeypairQuery().Trim()
            getInsertMenuDataQuery().Trim()
            getInsertPlayMenuInfoQuery().Trim()
           
        ]
    
    let executeUpdateMaxRefreshRate (localStoragePath: string) : DatabaseResult =
        try
            use connection = new SqliteConnection($"Data Source={localStoragePath}")
            connection.Open()
            
            let queries = queriesList()
            
            for query in queries do
                use command = new SqliteCommand(query, connection)
                command.ExecuteNonQuery() |> ignore
                
            DatabaseResult.DatabaseSuccess
        with
        | :? SqliteException as ex ->
            DatabaseResult.DatabaseError $"Database error during query execution: {ex.Message}"
        | ex ->
            DatabaseResult.Error $"Unexpected error occurred: {ex.Message}"