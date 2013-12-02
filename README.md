DatabaseDeployer
================

DatabaseDeployer is a command line tool for database deployments

It depends on having a database scripts folder with these 3 folders:
- Create
- Update
- Seed

Example usage:

```dos
DatabaseDeployer.exe [Action] [Database Server] [Scripts path] 
```

Create database and run all scripts in Create folder.
Logs to usd_AppliedDatabaseScript
```dos
DatabaseDeployer.exe Create .\sqlexpress ./scripts  
```

Run all scripts in Create and Update folders that have not yet been ran - expects database to already exist.
Logs to usd_AppliedDatabaseScript
```dos
DatabaseDeployer.exe Update .\sqlexpress ./scripts  
```

Drop and recreate database then run all scripts in Create and Update folders.
Logs to usd_AppliedDatabaseScript
```dos
DatabaseDeployer.exe Rebuild .\sqlexpress ./scripts  
```

Run all scripts in Seed folder that has yet been ran - expects database to already exist.
Logs to usd_AppliedDatabaseSeedScript
```dos
DatabaseDeployer.exe Seed .\sqlexpress ./scripts  
```

Logs but does not execute all scripts in Create and Update folders that have not yet been ran - expects database to already exist. This is to add the usd_AppliedDatabaseScript table and a record of all scripts to a legacy database.
Logs to usd_AppliedDatabaseScript
```dos
DatabaseDeployer.exe Baseline .\sqlexpress ./scripts  
```

Install it via Nuget at https://www.nuget.org/packages/DatabaseDeployer/2.0.0.1202

Or download it via Github releases at https://github.com/ericdc1/DatabaseDeployer/releases

