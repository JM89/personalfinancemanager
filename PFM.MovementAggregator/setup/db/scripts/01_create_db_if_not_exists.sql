USE master
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PFM_MVT_AGGR_DB')
BEGIN
    CREATE DATABASE [PFM_MVT_AGGR_DB]
END
GO