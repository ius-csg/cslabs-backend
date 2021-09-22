# cslabs-backend

### Setup

* [.NET Core](https://dotnet.microsoft.com/download) Download the latest .net core 3.1 SDK
* Install entity framework tools `dotnet tool install --global dotnet-ef`
* The Official IDE is Rider. Visual Studio 2019 Can be used as a fallback (Rider is preferred) but you have to set the launch profile to CSLabsBackend
* Copy `appsetting.Example.json` to `appsettings.json`
* Replace Email section of `appsettings.json` with the testing MailTrap credentials in [Email Credentials](https://trello.com/c/ytg2ndaX) card in Trello
* [Install MariaDB](#MariaDB-Setup) Choose no password when it asks you
* cd into `CSLabs.Api` and run `dotnet ef database update`

* Note: Every time changes are pulled from the repository, it is a good idea to perform this
  step again: cd into `CSLabs.Api` and run `dotnet ef database update`


#### Setup steps for connecting to a proxmox server

These steps are only required if you plan on starting a lab.

* Open terminal to the solution folder
* `cslabs change-hypervisor-password --id 1 --password <proxmox root password>` You can get the password from
  [this Trello card](https://trello.com/c/WFFm6iwa). Note: If on linux you have to write `./cslabs` instead.
* Connect to the VPN whenever you need to test with the proxmox server.


### Managing Hypervisors

To connect to a proxmox host, you will need to add them to the DB. The passwords used to
access the proxmox host is encrypted so a command is developed to ease the process of adding them.

1. Open the terminal to the solution folder.
2. Run `cslabs help`

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
cslabs change-hypervisor-password --id 1 --password <password given>
```

Documentation in trello


Note: On production and staging the novnc url has to be added manually to the nginx config if a new host is added.



### Tutorial

Follow these tutorials to get started.

[.Net Core Web Api Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio)

[EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)


### Ef Core

Before running any `dotnet ef` commands, cd into `<solution-dir>/CSLabs.Api`.
Also before generating migration, go to Models/UserModels/DefaultContext.cs and
add the line below.
```
DbSet<ModuleName> ModuleName {get; set;}
```
You will then need to go to OnModelCreating method within that file
and add the line below.
```
ModuleName.onModelCreating(builder); 
```

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


### Development

#### Git Workflow

The main branch is master, which will hold the version currently running in production.
When some work needs to be done, you will branch off from master using the
naming convention `<initials>/<feature-name>` for your branch.
After the work has completed, the developer should create a PR in Github to merge the branch back into dev and notify the project lead
to review. Please make sure to submit a PR to merge into **dev** branch and not master.