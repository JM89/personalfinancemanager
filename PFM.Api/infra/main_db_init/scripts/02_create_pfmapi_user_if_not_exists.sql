USE master
GO

IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'PFM_API_SVC')
BEGIN
    CREATE LOGIN [PFM_API_SVC] WITH PASSWORD = N'Helloworld123!'
END
GO

USE PFM_MAIN_DB
GO

IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = N'PFM_API_SVC')
BEGIN
    CREATE USER [PFM_API_SVC] FOR LOGIN [PFM_API_SVC] WITH DEFAULT_SCHEMA=[dbo]
END
GO

EXEC sp_addrolemember N'db_datawriter', N'PFM_API_SVC'
GO
EXEC sp_addrolemember N'db_datareader', N'PFM_API_SVC'
GO