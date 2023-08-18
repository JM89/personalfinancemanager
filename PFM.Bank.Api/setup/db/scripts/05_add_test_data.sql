USE PFM_BANK_DB
GO

SET IDENTITY_INSERT [dbo].[Banks] ON

INSERT INTO [dbo].[Banks] (Id, [Name],[IconPath],[CountryId],[Website],[GeneralEnquiryPhoneNumber], User_Id)
VALUES (1, 'Goodlife Bancorp','/Resources/bank_icons/default.png',1,'https://goodlife-bankcorp.co.uk',07000000000, 'jess')

SET IDENTITY_INSERT [dbo].[Banks] OFF

SET IDENTITY_INSERT [dbo].[Accounts] ON

insert into dbo.Accounts (Id, Name, BankId, CurrencyId, User_Id, InitialBalance, CurrentBalance, IsFavorite, IsSavingAccount)
values (1, 'Account A', 1, 1, 'jess', 0, 0, 1, 0)

insert into dbo.Accounts (Id, Name, BankId, CurrencyId, User_Id, InitialBalance, CurrentBalance, IsFavorite, IsSavingAccount)
values (2, 'Account B', 1, 1, 'jess', 0, 0, 0, 0)

insert into dbo.Accounts (Id, Name, BankId, CurrencyId, User_Id, InitialBalance, CurrentBalance, IsFavorite, IsSavingAccount)
values (3, 'Account C', 1, 1, 'jess', 0, 0, 0, 1)

SET IDENTITY_INSERT [dbo].[Accounts] OFF