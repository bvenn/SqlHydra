﻿module Program

open System.IO
open Fake.IO
open Fake.Core
open Fake.DotNet
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

// Initialize FAKE context
Setup.context()

let path xs = Path.Combine(Array.ofList xs)
let slnRoot = Files.findParent __SOURCE_DIRECTORY__ "SqlHydra.sln";

let query = path [ slnRoot; "SqlHydra.Query" ]
let mssql = path [ slnRoot; "SqlHydra.SqlServer" ]
let npgsql = path [ slnRoot; "SqlHydra.Npgsql" ]
let sqlite = path [ slnRoot; "SqlHydra.Sqlite" ]
let tests = path [ slnRoot; "Tests" ]

let packages = [ query; mssql; npgsql; sqlite ]

Target.create "Restore" <| fun _ ->
    packages @ [ tests ]
    |> List.map (fun pkg -> Shell.Exec(Tools.dotnet, "restore", pkg), pkg)
    |> List.iter (fun (code, pkg) -> if code <> 0 then failwith $"Could not restore '{pkg}' package.")

Target.create "BuildNet5" <| fun _ ->
    packages @ [ tests ]
    |> List.map (fun pkg -> Shell.Exec(Tools.dotnet, "build --configuration Release --framework net5.0", pkg), pkg)
    |> List.iter (fun (code, pkg) -> if code <> 0 then failwith $"Could not build '{pkg}'package.'")

Target.create "BuildNet6" <| fun _ ->
    packages @ [ tests ]
    |> List.map (fun pkg -> Shell.Exec(Tools.dotnet, "build --configuration Release --framework net6.0", pkg), pkg)
    |> List.iter (fun (code, pkg) -> if code <> 0 then failwith $"Could not build '{pkg}'package.'")

Target.create "Build" <| fun _ ->
    printfn "Building all supported frameworks."

Target.create "TestNet5" <| fun _ ->
    let exitCode = Shell.Exec(Tools.dotnet, "run --configuration Release --framework net5.0", tests)
    if exitCode <> 0 then failwith "Failed while running net5.0 tests"

Target.create "TestNet6" <| fun _ ->
    let exitCode = Shell.Exec(Tools.dotnet, "run --configuration Release --framework net6.0", tests)
    if exitCode <> 0 then failwith "Failed while running net6.0 tests"

Target.create "Test" <| fun _ ->
    printfn "Testing on all supported frameworks."

Target.create "Pack" <| fun _ ->
    packages
    |> List.map (fun pkg -> Shell.Exec(Tools.dotnet, "pack --configuration Release -o nupkg/Release", pkg), pkg)
    |> List.iter (fun (code, pkg) -> if code <> 0 then failwith $"Could not build '{pkg}' package.'")

let version = "*.0.630.0-beta.2.nupkg"

Target.create "Publish" <| fun _ ->
    let nugetKey =
        match Environment.environVarOrNone "SQLHYDRA_NUGET_KEY" with
        | Some nugetKey -> nugetKey
        | None -> failwith "The Nuget API key must be set in a SQLHYDRA_NUGET_KEY environmental variable"
    
    packages
    |> List.map (fun pkg -> pkg </> "nupkg" </> "Release" </> version)
    |> List.map (fun nupkg -> Shell.Exec(Tools.dotnet, $"nuget push {nupkg} -s nuget.org -k {nugetKey}"), nupkg)
    |> List.iter (fun (code, pkg) -> if code <> 0 then failwith $"Could not publish '{pkg}' package. Error: {code}")

let dependencies = [
    "Restore" ==> "BuildNet5" ==> "BuildNet6" ==> "Build"
    "Restore" ==> "BuildNet5" ==> "BuildNet6" ==> "Build" ==> "TestNet5" ==> "TestNet6" ==> "Test"
    "Restore" ==> "BuildNet5" ==> "BuildNet6" ==> "Build" ==> "TestNet5" ==> "TestNet6" ==> "Test" ==> "Pack" ==> "Publish"
]

[<EntryPoint>]
let main (args: string[]) =
    try
        match args with
        | [| singleArg |] -> Target.runOrDefault singleArg
        | _ -> Target.runOrDefault "Publish"
        0
    with ex ->
        printfn "%A" ex
        1
