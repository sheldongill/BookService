# BookService - An example Web API project for DotNet Core 2.0

This is an example project for a web service which can serve as a useful starting template.
It's a straight-forward CRUD style web api, as that's the most widely understood form and easiest to come to. API design, HATEOS and such aren't the subject here.
Even a "simple" API has a few complexities:
* returning useful errors as well as data objects
* properly using return codes (200, 201, 401, 404)

When actually spinning up a web service for real use, you require logging, authentication and a backing database. This example has all those things.
- EF Core to utilize a SQL Server database
- Serilog for structured logging. Shows output to console and log file.
- Authentication using JWT bearer token

- Simple controller for status (for health indication behind a load balancer or application monitoring)
- Simple controller for version information



## SQL Server setup

You need to have a version of SQL Server installed and running. For Visual Studio users, LocalDB is the easiest solution.

### SQL Server create datbase and user

For OSX users, use docker to run MS SQL Server Linux. You need to alter the connection string accordingly and create the database:
'''sql
 create database BookServiceDB
 go
 create login booksvc with password='Passw0rd1'
 go
 use BookServiceDB;
 go
 create user booksvc from login booksvc;
 go
 exec sp_addrolemember N'db_owner', N'booksvc'
 go
'''

### Apply migrations

dotnet ef database update

## Authentication

Hit the Authentication button in Swagger. Enter
  Bearer <jwt_token.txt>


1> create login nomadd with password='N0maddpass'
2> go
Changed database context to 'BookServiceDB'.
1> select * from sysobjects where xtype = 'U';
2> go

