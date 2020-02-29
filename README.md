# cslabs-backend

### Setup

* [.NET Core](https://dotnet.microsoft.com/download) Download .net core 2.2 SDK
* Install entity framework tools `dotnet tool install --global dotnet-ef`
* The Official IDE is Rider. Visual Studio 2019 Can be used as a fallback but you have to set the launch profile to CSLabsBackend
* Copy `appsetting.Example.json` to `appsettings.json`
* Replace Email section of `appsettings.json` with the testing mailtrap credentials in [Email Credentials](https://trello.com/c/ytg2ndaX) card in Trello
* [Install MariaDB](#MariaDB-Setup) Choose no password when it asks you
* cd into `CSLabs.Api` and run `dotnet ef database update`

* Note: Every time changes are pulled from the repository, it is a good idea to perform this
step again: cd into `CSLabs.Api` and run `dotnet ef database update`


#### Setup steps for connecting to a proxmox server

These steps are only required if you plan on starting a lab.

* Build the project
* Copy `CSLabs.Api/appsettings.json` to `CSLabs.Console/bin/Debug/netcoreapp2.2`
* Change directory into Console build directory `cd CSLabs.Console/bin/Debug/netcoreapp2.2` 
* `dotnet CSLabs.Console.dll change-hypervisor-password --id 1 --password <proxmox root password>` You can get the password from 
[this Trello card](https://trello.com/c/WFFm6iwa)
* Connect to the VPN whenever you need to test with the proxmox server.


### Managing Hypervisors

To connect to a proxmox host, you will need to add them to the DB. The passwords used to
access the proxmox host is encrypted so a command is developed to ease the process of adding them.

1. Build the solution
2. `cd CSLabs.Console/bin/Debug/netcoreapp2.2`
3. `dotnet CSLabs.Console.dll` 

This will show you all the commands available:

```
 add-hypervisor                Adds a hypervisor to the database

  add-hypervisor-node           Adds a hypervisor node to the database

  change-hypervisor-password    Changes the password for a hypervisor

  list-hypervisors              Lists hypervisors in the database

  encrypt                       Encrypts a string

  decrypt                       Decrypts a string
```

You will need to change the password of the default hypervisor. The password is found in trello in the notes column

```
dotnet CSLabs.Console.dll change-hypervisor-password --id 1 --password <password given>
```

Documentation in trello 


Note: On production and staging the novnc url has to be added manually to the nginx config if a new host is added.



### Tutorial

Follow these tutorials to get started.

[.Net Core Web Api Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio)

[EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)


### Ef Core

Before running any `dotnet ef` commands, cd into `<project-dir>/CSLabs.Api`.
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