USE PFM_BANK_DB
GO

SET IDENTITY_INSERT [dbo].[Countries] ON
INSERT INTO [dbo].[Countries] (Id, Name) VALUES (1, 'United Kingdom')
INSERT INTO [dbo].[Countries] (Id, Name) VALUES (2, 'France')
INSERT INTO [dbo].[Countries] (Id, Name) VALUES (3, 'Germany')
INSERT INTO [dbo].[Countries] (Id, Name) VALUES (4, 'Spain')
INSERT INTO [dbo].[Countries] (Id, Name) VALUES (5, 'Italy')
SET IDENTITY_INSERT [dbo].[Countries] OFF

SET IDENTITY_INSERT [dbo].[Currencies] ON
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (1, 'GBP', '£')
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (2, 'EUR', '€')
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (3, 'USD', '$')
SET IDENTITY_INSERT [dbo].[Currencies] OFF