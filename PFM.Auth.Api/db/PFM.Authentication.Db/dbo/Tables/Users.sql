CREATE TABLE [dbo].[Users] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR(256) NOT NULL,
    [LastName] NVARCHAR(256) NOT NULL,
    [Username] NVARCHAR(256) NOT NULL,
    [Password] varbinary(1024) NOT NULL
);