# cslabs-backend

### Requirements

* [.NET Core](https://dotnet.microsoft.com/download) Download .net core 2.2 SDK
* Visual Studio 2019 or the Rider IDE from Jetbrains

### Tutorial

Follow these tutorials to get started.

[.Net Core Web Api Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio)

[EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)


### Ef Core

To generate a migration based on your latest changes, type:

```
dotnet ef migrations add <MigrationName>
``` 

Modify the migration if it doesn't suite your needs exactly.

To Update the database with the migration using this command:

```
dotnet ef database update
```

Sometimes you add a migration and realize you need to make additional changes to your EF Core model before applying it. To remove the last migration, use this command.

```
dotnet ef migrations remove
```
Read more about migrations [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations)

### MariaDB Setup

[Setup MariaDB 10.2.13](https://downloads.mariadb.org/interstitial/mariadb-10.2.13/winx64-packages/mariadb-10.2.13-winx64.msi/from/http%3A//ftp.hosteurope.de/mirror/archive.mariadb.org/)

### Database Diagram

Download MYSQL Workbench from [here](https://dev.mysql.com/get/Downloads/MySQLGUITools/mysql-workbench-community-8.0.17-winx64.msi)

Open [cslabs-db-diagram.mwb](./cslabs-db-diagram.mwb) in the root of this project using mysql workbench.