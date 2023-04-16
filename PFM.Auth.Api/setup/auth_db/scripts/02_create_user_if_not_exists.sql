USE master
GO

IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'PFM_AUTH_API_SVC')
BEGIN
    CREATE LOGIN [PFM_AUTH_API_SVC] WITH PASSWORD = N'Helloworld123!'
END
GO

USE PFM_AUTH_DB
GO

IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = N'PFM_AUTH_API_SVC')
BEGIN
    CREATE USER [PFM_AUTH_API_SVC] FOR LOGIN [PFM_AUTH_API_SVC] WITH DEFAULT_SCHEMA=[dbo]
END
GO

EXEC sp_addrolemember N'db_datawriter', N'PFM_AUTH_API_SVC'
GO
EXEC sp_addrolemember N'db_datareader', N'PFM_AUTH_API_SVC'
GO