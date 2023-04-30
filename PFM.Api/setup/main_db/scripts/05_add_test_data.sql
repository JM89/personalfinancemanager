USE PFM_MAIN_DB
GO

SET IDENTITY_INSERT [dbo].[Currencies] ON
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (1, 'GBP', '£')
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (2, 'EUR', '€')
INSERT INTO [dbo].[Currencies] (Id, Name, Symbol) VALUES (3, 'USD', '$')
SET IDENTITY_INSERT [dbo].[Currencies] OFF

SET IDENTITY_INSERT [dbo].[ExpenseTypes] ON
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (1, 'Groceries', '#3399ff', 1)
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (2, 'Energy', '#33cc33', 1)
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (3, 'Insurances', '#ff0000', 1)
SET IDENTITY_INSERT [dbo].[ExpenseTypes] OFF

INSERT INTO [dbo].[Banks] ([Name],[IconPath],[CountryId],[Website],[GeneralEnquiryPhoneNumber])
VALUES ('Goodlife Bancorp','/Resources/bank_icons/default.png',1,'https://goodlife-bankcorp.co.uk',07000000000)