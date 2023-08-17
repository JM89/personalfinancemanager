USE PFM_MAIN_DB
GO

SET IDENTITY_INSERT [dbo].[ExpenseTypes] ON
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (1, 'Groceries', '3399FF', 1)
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (2, 'Energy', '33CC33', 1)
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (3, 'Insurances', 'FF0000', 1)
SET IDENTITY_INSERT [dbo].[ExpenseTypes] OFF

SET IDENTITY_INSERT [dbo].[AtmWithdraws] ON

insert into dbo.AtmWithdraws (Id, AccountId, DateExpense, InitialAmount, CurrentAmount, IsClosed, HasBeenAlreadyDebited)
values (1, 1, '2022-12-01', 100, 100, 0, 1)

insert into dbo.AtmWithdraws (Id, AccountId, DateExpense, InitialAmount, CurrentAmount, IsClosed, HasBeenAlreadyDebited)
values (2, 1, '2022-12-15', 50, 50, 0, 1)

insert into dbo.AtmWithdraws (Id, AccountId, DateExpense, InitialAmount, CurrentAmount, IsClosed, HasBeenAlreadyDebited)
values (3, 2, '2022-12-28', 80, 80, 0, 1)

SET IDENTITY_INSERT [dbo].[AtmWithdraws] OFF