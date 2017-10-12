#!/bin/bash
if [ -z "$ASPNETCORE_ENVIRONMENT" ]; then
    echo "Unable to determine host environment! Please set ASPNETCORE_ENVIRONMENT"
    #exit
fi
dotnet restore
dotnet build
cd BookService
dotnet ef migrations script -i -o db_migration.sql
sqlcmd -S "(LocalDB)\MSSQLLocalDB" -d BookService -i db_migration.sql -b
if [ $? -ne 0 ]; then
    echo "Migrations failed"
    exit
fi
cd ../BookServiceTests
dotnet test --no-build
#sqlcmd -S "(LocalDB)\MSSQLLocalDB" -Q "drop database BookService_Test" -b
