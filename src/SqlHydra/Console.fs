﻿module Console

open Spectre.Console
open SqlHydra.Schema
open System
open Newtonsoft.Json

type AppInfo = {
    Name: string
    Command: string
    DefaultReaderType: string
    Version: string
}

let yesNo(title: string) = 
    let selection = SelectionPrompt<string>()
    selection.Title <- title
    selection.AddChoices(["Yes"; "No"]) |> ignore
    AnsiConsole.Prompt(selection) = "Yes"

let newConfigWizard(appInfo: AppInfo) = 
    let connStr = AnsiConsole.Ask<string>("Enter a database [green]Connection String[/]:")
    let outputFile = AnsiConsole.Ask<string>("Enter an [green]Output Filename[/] (Ex: [yellow]AdventureWorks.fs[/]):")
    let ns = AnsiConsole.Ask<string>("Enter a [green]Namespace[/] (Ex: [yellow]MyApp.AdventureWorks[/]):")
    let isCLIMutable = yesNo "Add CLIMutable attribute to generated records?"
    let enableReaders = yesNo "Generate HydraReader?"
    let useDefaultReaderType = 
        if enableReaders 
        then yesNo $"Use the default Data Reader Type? (Default = {appInfo.DefaultReaderType}):"
        else false
    let readerType = 
        if not useDefaultReaderType
        then AnsiConsole.Ask<string>($"Enter [green]Data Reader Type[/]:")
        else appInfo.DefaultReaderType

    { 
        Config.ConnectionString = connStr
        Config.OutputFile = outputFile
        Config.Namespace = ns
        Config.IsCLIMutable = isCLIMutable
        Config.Readers = 
            {
                ReadersConfig.IsEnabled = enableReaders
                ReadersConfig.ReaderType = readerType
            }
    }

let fileName = "hydra.json"

let saveConfig (cfg: Config) = 
    let json = JsonConvert.SerializeObject(cfg, Formatting.Indented)
    IO.File.WriteAllText(fileName, json)

let tryLoadConfig() = 
    if IO.File.Exists(fileName) then
        try
            let json = IO.File.ReadAllText(fileName)
            JsonConvert.DeserializeObject<SqlHydra.Schema.Config>(json) |> Some
        with ex -> None
    else 
        None

/// Creates hydra.json if necessary and then runs.
let getConfig(appInfo: AppInfo, argv: string array) = 

    AnsiConsole.MarkupLine("[red]SqlHydra[/]")

    match argv with 
    | [| |] ->
        match tryLoadConfig() with
        | Some cfg -> cfg
        | None ->
            AnsiConsole.MarkupLine("[yellow]\"hydra.json\" not detected. Starting configuration wizard...[/]")
            let cfg = newConfigWizard(appInfo)
            saveConfig cfg
            cfg

    | [| "--create" |] -> 
        AnsiConsole.MarkupLine("[yellow]Creating a new configuration...[/]")
        let cfg = newConfigWizard(appInfo)
        saveConfig cfg
        cfg

    | _ ->
        AnsiConsole.MarkupLine($"Invalid args: '{argv}'. Expected no args, or \"--edit\".")
        failwith "Invalid args."
