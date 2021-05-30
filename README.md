# SqlHydra
SqlHydra is a collection of dotnet tools that generate F# records for a given database provider.

Currently supported databases:
- [SQL Server](https://github.com/JordanMarr/SqlHydra#sqlhydrasqlserver-)
- [SQLite](https://github.com/JordanMarr/SqlHydra#sqlhydrasqlite-)

## SqlHydra.SqlServer [![NuGet version (SqlHydra.SqlServer)](https://img.shields.io/nuget/v/SqlHydra.SqlServer.svg?style=flat-square)](https://www.nuget.org/packages/SqlHydra.SqlServer/)

### Local Install (recommended)
Run the following commands from your project directory:
1) `dotnet new tool-manifest`
2) `dotnet tool install SqlHydra.SqlServer`

### Configure

Create a batch file or shell script (`gen.bat` or `gen.sh`) in your project directory with the following contents:

```bat
dotnet sqlhydra-mssql {connection string} {namespace} {filename.fs}
```

_Example:_
```bat
dotnet sqlhydra-mssql "Data Source=localhost\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=SSPI" "SampleApp.AdventureWorks" "AdventureWorks.fs"
```

### Generate Records
1) Run your `gen.bat` (or `gen.sh`) file to generate the output .fs file.
2) Manually add the .fs file to your project.

### Regenerate Records
1) Run your `gen.bat` (or `gen.sh`) file to refresh the output .fs file.

## SqlHydra.Sqlite [![NuGet version (SqlHydra.Sqlite)](https://img.shields.io/nuget/v/SqlHydra.SqlServer.svg?style=flat-square)](https://www.nuget.org/packages/SqlHydra.Sqlite/)

### Local Install (recommended)
Run the following commands from your project directory:
1) `dotnet new tool-manifest`
2) `dotnet tool install SqlHydra.Sqlite`

### Configure

Create a batch file or shell script (`gen.bat` or `gen.sh`) in your project directory with the following contents:

```bat
dotnet sqlhydra-sqlite {connection string} {namespace} {filename.fs}
```

_Example:_
```bat
dotnet sqlhydra-sqlite "Data Source=C:\MyProject\AdventureWorksLT.db" "SampleApp.AdventureWorks" "AdventureWorks.fs"
```

### Generate Records
1) Run your `gen.bat` (or `gen.sh`) file to generate the output .fs file.
2) Manually add the .fs file to your project.

### Regenerate Records
1) Run your `gen.bat` (or `gen.sh`) file to refresh the output .fs file.


### Example Output for AdventureWorks
```F#
// This code was generated by SqlHydra.SqlServer.
namespace SampleApp.AdventureWorks

module dbo =
    type ErrorLog =
        { ErrorLogID: int
          ErrorTime: System.DateTime
          UserName: string
          ErrorNumber: int
          ErrorMessage: string
          ErrorSeverity: Option<int>
          ErrorState: Option<int>
          ErrorProcedure: Option<string>
          ErrorLine: Option<int> }

    type BuildVersion =
        { SystemInformationID: byte
          ``Database Version``: string
          VersionDate: System.DateTime
          ModifiedDate: System.DateTime }

module SalesLT =
    type Address =
        { City: string
          StateProvince: string
          CountryRegion: string
          PostalCode: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          AddressID: int
          AddressLine1: string
          AddressLine2: Option<string> }

    type Customer =
        { LastName: string
          PasswordHash: string
          PasswordSalt: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          CustomerID: int
          NameStyle: bool
          FirstName: string
          MiddleName: Option<string>
          Title: Option<string>
          Suffix: Option<string>
          CompanyName: Option<string>
          SalesPerson: Option<string>
          EmailAddress: Option<string>
          Phone: Option<string> }

    type CustomerAddress =
        { CustomerID: int
          AddressID: int
          AddressType: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime }

    type Product =
        { ProductID: int
          Name: string
          ProductNumber: string
          StandardCost: decimal
          ListPrice: decimal
          SellStartDate: System.DateTime
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          SellEndDate: Option<System.DateTime>
          DiscontinuedDate: Option<System.DateTime>
          ThumbNailPhoto: Option<byte []>
          ThumbnailPhotoFileName: Option<string>
          Size: Option<string>
          Weight: Option<decimal>
          ProductCategoryID: Option<int>
          ProductModelID: Option<int>
          Color: Option<string> }

    type ProductCategory =
        { Name: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          ProductCategoryID: int
          ParentProductCategoryID: Option<int> }

    type ProductDescription =
        { ProductDescriptionID: int
          Description: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime }

    type ProductModel =
        { ProductModelID: int
          Name: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          CatalogDescription: Option<System.Xml.Linq.XElement> }

    type ProductModelProductDescription =
        { ProductModelID: int
          ProductDescriptionID: int
          Culture: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime }

    type SalesOrderDetail =
        { SalesOrderID: int
          SalesOrderDetailID: int
          OrderQty: System.Int16
          ProductID: int
          UnitPrice: decimal
          UnitPriceDiscount: decimal
          LineTotal: decimal
          rowguid: System.Guid
          ModifiedDate: System.DateTime }

    type SalesOrderHeader =
        { SalesOrderID: int
          RevisionNumber: byte
          OrderDate: System.DateTime
          DueDate: System.DateTime
          Status: byte
          OnlineOrderFlag: bool
          SalesOrderNumber: string
          CustomerID: int
          SubTotal: decimal
          TaxAmt: decimal
          Freight: decimal
          TotalDue: decimal
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          ShipMethod: string
          CreditCardApprovalCode: Option<string>
          Comment: Option<string>
          ShipToAddressID: Option<int>
          BillToAddressID: Option<int>
          PurchaseOrderNumber: Option<string>
          AccountNumber: Option<string>
          ShipDate: Option<System.DateTime> }

    type vProductAndDescription =
        { ProductID: int
          Name: string
          ProductModel: string
          Culture: string
          Description: string }

    type vProductModelCatalogDescription =
        { ProductModelID: int
          Name: string
          rowguid: System.Guid
          ModifiedDate: System.DateTime
          Summary: Option<string>
          Manufacturer: Option<string>
          Copyright: Option<string>
          ProductURL: Option<string>
          WarrantyPeriod: Option<string>
          WarrantyDescription: Option<string>
          NoOfYears: Option<string>
          MaintenanceDescription: Option<string>
          Wheel: Option<string>
          Saddle: Option<string>
          Pedal: Option<string>
          BikeFrame: Option<string>
          Crankset: Option<string>
          PictureAngle: Option<string>
          PictureSize: Option<string>
          ProductPhotoID: Option<string>
          Material: Option<string>
          Color: Option<string>
          ProductLine: Option<string>
          Style: Option<string>
          RiderExperience: Option<string> }

    type vGetAllCategories =
        { ParentProductCategoryName: string
          ProductCategoryName: Option<string>
          ProductCategoryID: Option<int> }

```



## Officially Recommended ORM: Dapper.FSharp.Linq

After creating SqlHydra, I was trying to find the perfect ORM to complement SqlHyda's generated records.
Ideally, I wanted to find a library with 
- First-class support for F# records, option types, etc.
- LINQ queries (to take advantage of strongly typed SqlHydra generated records)

[FSharp.Dapper](https://github.com/Dzoukr/Dapper.FSharp) met the first critera with flying colors. 
As the name suggests, Dapper.FSharp was written specifically for F# with simplicity and ease-of-use as the driving design priorities.
FSharp.Dapper features custom F# Computation Expressions for selecting, inserting, updating and deleting, and support for F# Option types and records (no need for `[<CLIMutable>]` attributes!).

If only it had Linq queries, it would be the _perfect_ complement to SqlHydra...

So I submitted a [PR](https://github.com/Dzoukr/Dapper.FSharp/pull/26) to Dapper.FSharp that adds Linq query expressions (now in v2.0+)!

[Dapper.FSharp.Linq](https://github.com/JordanMarr/Dapper.FSharp.Linq) is a more specialized version which takes the new Linq provider in Dapper.FSharp and adds some extra features that facilitate the generated types workflow.

Between the two, you can have strongly typed access to your database:

```fsharp
module SampleApp.DapperFSharpExample
open System.Data
open Microsoft.Data.SqlClient
open Dapper.FSharp.LinqBuilders
open Dapper.FSharp.MSSQL
open SampleApp.AdventureWorks // Generated Types

Dapper.FSharp.OptionTypes.register()
    
// Tables
let customerTable =         table<Customer>         |> inSchema (nameof SalesLT)
let customerAddressTable =  table<CustomerAddress>  |> inSchema (nameof SalesLT)
let addressTable =          table<SalesLT.Address>  |> inSchema (nameof SalesLT)

let getAddressesForCity(conn: IDbConnection) (city: string) = 
    select {
        for a in addressTable do
        where (a.City = city)
    } |> conn.SelectAsync<SalesLT.Address>
    
let getCustomersWithAddresses(conn: IDbConnection) =
    select {
        for c in customerTable do
        leftJoin ca in customerAddressTable on (c.CustomerID = ca.CustomerID)
        leftJoin a  in addressTable on (ca.AddressID = a.AddressID)
        where (isIn c.CustomerID [30018;29545;29954;29897;29503;29559])
        orderBy c.CustomerID
    } |> conn.SelectAsyncOption<Customer, CustomerAddress, Address>

```

