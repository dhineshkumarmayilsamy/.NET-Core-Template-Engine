#### .NET 5.0

### Repository pattern - .NET Core API Implementation (Project Created From Template)

* ORM - EFCore - Db First
* Database - MySQL 
* Logger - Serilog

### Command
* Script - Execute script from SQL/MySQL.sql file
* MySQL - Scaffold : Execute this command inside Model Project

      dotnet ef dbcontext scaffold "server=[$SERVER]; port=[$PORT]; database=[$DATABASE]; uid=[$UID]; password=[$PASSWORD]" Pomelo.EntityFrameworkCore.MySql -o Model -c AppDbContext -f --no-onconfiguring



