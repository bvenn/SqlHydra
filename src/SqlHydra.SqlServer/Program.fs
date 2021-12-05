﻿module SqlHydra.SqlServer.Program

open SqlHydra.SqlServer
open SqlHydra
open System.IO
open Domain

type private SelfRef = class end
let version = System.Reflection.Assembly.GetAssembly(typeof<SelfRef>).GetName().Version |> string

let app = 
    {
        AppInfo.Name = "SqlHydra.SqlServer"
        AppInfo.Command = "sqlhydra-mssql"
        AppInfo.DefaultReaderType = "Microsoft.Data.SqlClient.SqlDataReader"
        AppInfo.Version = version
    }

[<EntryPoint>]
let main argv =

    let cfg = Console.getConfig(app, argv)

    let formattedCode = 
        SqlServerSchemaProvider.getSchema cfg
        |> SchemaGenerator.generateModule cfg app
        |> SchemaGenerator.toFormattedCode cfg app

    File.WriteAllText(cfg.OutputFile, formattedCode)
    Fsproj.addFileToProject(cfg)
    0
