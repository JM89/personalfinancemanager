USE PFM_MAIN_DB
GO

SET IDENTITY_INSERT [dbo].[Countries] ON
INSERT INTO [dbo].[Countries] (Id, Name) VALUES (1, 'United Kingdom')
SET IDENTITY_INSERT [dbo].[Countries] OFF

SET IDENTITY_INSERT [dbo].[Currencies] ON
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (1, 'GBP', '£')
SET IDENTITY_INSERT [dbo].[Currencies] OFF

INSERT INTO [dbo].[Banks] ([Name],[IconPath],[CountryId],[Website],[GeneralEnquiryPhoneNumber])
VALUES ('Goodlife Bancorp','/Resources/bank_icons/default.png',1,'https://goodlife-bankcorp.co.uk',07000000000)