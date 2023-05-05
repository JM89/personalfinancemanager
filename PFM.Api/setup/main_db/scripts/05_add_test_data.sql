USE PFM_MAIN_DB
GO

SET IDENTITY_INSERT [dbo].[ExpenseTypes] ON
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (1, 'Groceries', '3399FF', 1)
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (2, 'Energy', '33CC33', 1)
INSERT INTO [dbo].[ExpenseTypes] (Id, Name, [GraphColor], [ShowOnDashboard]) VALUES (3, 'Insurances', 'FF0000', 1)
SET IDENTITY_INSERT [dbo].[ExpenseTypes] OFF
