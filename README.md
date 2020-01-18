# cslabs-backend

### Requirements

* [.NET Core](https://dotnet.microsoft.com/download) Download .net core 2.2 SDK
* Visual Studio 2019 or the Rider IDE from Jetbrains
* Copy `appsetting.Example.json` to `appsettings.json`


### Managing Hypervisors

To connect to a proxmox host, you will need to add them to the DB. The passwords used to
access the proxmox host is encrypted so a command is developed to ease the process of adding them.

1. Build the solution
2. `cd CSLabsConsole/bin/Debug/netcoreapp2.2`
3. `dotnet CSLabsConsole.dll` 

This will show you all the commands available:

```
 add-hypervisor                Adds a hypervisor to the database

  add-hypervisor-node           Adds a hypervisor node to the database

  change-hypervisor-password    Changes the password for a hypervisor

  list-hypervisors              Lists hypervisors in the database

  encrypt                       Encrypts a string

  decrypt                       Decrypts a string
```

Command to add a hypervisor:

```
dotnet CSLabsConsole.dll add-hypervisor --host <hostname> --username root --password <password given> --novncurl
```

Command to add a node:

```
dotnet CSLabsConsole.dll add-hypervisor-node --name <node-name> --hypervisorid <the id of the hypervisor added>
```

Documentation in trello 


Note: On production and staging the novnc url has to be added manually to the nginx config if a new host is added.



### Tutorial

Follow these tutorials to get started.

[.Net Core Web Api Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio)

[EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)


### Ef Core

Before running any `dotnet ef` commands, cd into `<project-dir>/cslabs-backend`.
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

To revert a migration:

```
dotnet ef database update LastGoodMigration
```


### MariaDB Setup

[Setup MariaDB 10.2.13](https://downloads.mariadb.org/interstitial/mariadb-10.2.13/winx64-packages/mariadb-10.2.13-winx64.msi/from/http%3A//ftp.hosteurope.de/mirror/archive.mariadb.org/)

### Database Diagram

Download MYSQL Workbench from [here](https://dev.mysql.com/get/Downloads/MySQLGUITools/mysql-workbench-community-8.0.17-winx64.msi)

Open [cslabs-db-diagram.mwb](./cslabs-db-diagram.mwb) in the root of this project using mysql workbench.