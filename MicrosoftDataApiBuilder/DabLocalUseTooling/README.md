### Run sql server in docker

#### Postgressql
docker pull postgres:latest
docker run --name pstgress-dab -e POSTGRES_PASSWORD=PassWord -e POSTGRES_USER=postgres -p 5432:5432 -d postgres 


#SqlServer
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Data@Api" -p 1433:1433 --name mssql_dataapi -d mcr.microsoft.com/mssql/server

[Quickstart: Use Data API builder with SQL](https://learn.microsoft.com/en-us/azure/data-api-builder/quickstart-sql)

```
DROP DATABASE IF EXISTS bookshelf;
GO

CREATE DATABASE bookshelf;
GO

USE bookshelf;
GO

DROP TABLE IF EXISTS dbo.authors;
GO

CREATE TABLE dbo.authors
(
    id int not null primary key,
    first_name nvarchar(100) not null,
    middle_name nvarchar(100) null,
    last_name nvarchar(100) not null
)
GO

INSERT INTO dbo.authors VALUES
    (01, 'Henry', null, 'Ross'),
    (02, 'Jacob', 'A.', 'Hancock'),
    (03, 'Sydney', null, 'Mattos'),
    (04, 'Jordan', null, 'Mitchell'),
    (05, 'Victoria', null, 'Burke'),
    (06, 'Vance', null, 'DeLeon'),
    (07, 'Reed', null, 'Flores'),
    (08, 'Felix', null, 'Henderson'),
    (09, 'Avery', null, 'Howard'),
    (10, 'Violet', null, 'Martinez')
GO

```
