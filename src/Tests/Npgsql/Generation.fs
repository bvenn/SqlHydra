module Npgsql.Generation

open Expecto
open SqlHydra.Npgsql
open SqlHydra
open SqlHydra.Domain
open VerifyTests
open VerifyExpecto

let cfg = 
    {
        ConnectionString = DB.connectionString
        OutputFile = ""
        Namespace = "TestNS"
        IsCLIMutable = true
        Readers = Some { ReadersConfig.ReaderType = Program.app.DefaultReaderType } 
        Filters = FilterPatterns.Empty
    }

[<Tests>]
let tests = 
    categoryList "Npgsql" "Generation Integration Tests" [

        ptest "Print Schema" {
            let schema = NpgsqlSchemaProvider.getSchema cfg
            printfn "Schema: %A" schema
        }

        let lazySchema = lazy NpgsqlSchemaProvider.getSchema cfg

        let getCode cfg =
            lazySchema.Value
            |> SchemaGenerator.generateModule cfg SqlHydra.Npgsql.Program.app
            |> SchemaGenerator.toFormattedCode cfg SqlHydra.Npgsql.Program.app

        let inCode (str: string) cfg = 
            let code = getCode cfg
            Expect.isTrue (code.Contains str) ""

        let notInCode (str: string) cfg = 
            let code = getCode cfg
            Expect.isFalse (code.Contains str) ""

        ptest "Print Code" {
            getCode cfg |> printfn "%s"
        }

        testTask "Verify Generated Code"  {
            let code = getCode cfg
            
            let settings = VerifySettings()
            settings.UseDirectory("./Verify")
            settings.ScrubLines(fun line -> line.StartsWith("// This code was generated by `SqlHydra.Npgsql`"))
#if NET5_0
            do! Verifier.Verify("Verify Generated Code NET5", code, settings)
#endif
#if NET6_0
            do! Verifier.Verify("Verify Generated Code NET6", code, settings)
#endif
        }
    
        test "Code Should Have Reader" {
            cfg |> inCode "type HydraReader"
        }
    
        test "Code Should Not Have Reader" {
            { cfg with Readers = None } |> notInCode "type HydraReader"
        }

        test "Code Should Have CLIMutable" {
            { cfg with IsCLIMutable = true } |> inCode "[<CLIMutable>]"
        }

        test "Code Should Not Have CLIMutable" {
            { cfg with IsCLIMutable = false } |> notInCode "[<CLIMutable>]"
        }

        test "Code Should Have Namespace" {
            cfg |> inCode "namespace TestNS"
        }

        test "Should have Tables and PKs" {
            let schema = lazySchema.Value
            
            let allColumns = 
                schema.Tables 
                |> List.collect (fun t -> t.Columns)

            let pks = allColumns |> List.filter (fun c -> c.IsPK)
            
            Expect.equal schema.Tables.Length 156 ""

            let numberOfTables =
                schema.Tables
                |> Seq.filter (fun t -> t.Type = TableType.Table)
                |> Seq.length

            Expect.isTrue (pks.Length > numberOfTables) "Expected at least one pk per table"
            Expect.isTrue (pks.Length < allColumns.Length) "Every column should not be a PK"
        }
        
        test "Code Should Have ProviderDbTypeAttribute With Json" {
            cfg |> inCode "[<SqlHydra.ProviderDbType(\"Json\")>]"
        }
        
        test "Code Should Have ProviderDbTypeAttribute With Jsonb" {
            cfg |> inCode "[<SqlHydra.ProviderDbType(\"Jsonb\")>]"
        }
    ]