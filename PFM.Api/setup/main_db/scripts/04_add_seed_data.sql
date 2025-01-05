USE PFM_MAIN_DB
GO

-- Enumeration: PaymentMethod

IF NOT EXISTS (SELECT * FROM [dbo].[PaymentMethods] WHERE [Id] = 1)
BEGIN
    INSERT INTO [dbo].[PaymentMethods]([Id], [Name], [CssClass], [IconClass], [HasBeenAlreadyDebitedOption]) VALUES(1, 'CB', 'primary', 'fa fa-credit-card', 1)
END

IF NOT EXISTS (SELECT * FROM [dbo].[PaymentMethods] WHERE [Id] = 2)
BEGIN
    INSERT INTO [dbo].[PaymentMethods]([Id], [Name], [CssClass], [IconClass], [HasBeenAlreadyDebitedOption]) VALUES(2, 'Cash', 'info', 'fa fa-money', 0)
END

IF NOT EXISTS (SELECT * FROM [dbo].[PaymentMethods] WHERE [Id] = 3)
BEGIN
    INSERT INTO [dbo].[PaymentMethods]([Id], [Name], [CssClass], [IconClass], [HasBeenAlreadyDebitedOption]) VALUES(3, 'Direct Debit', 'success', 'fa fa-credit-card-alt', 0)
END

IF NOT EXISTS (SELECT * FROM [dbo].[PaymentMethods] WHERE [Id] = 4)
BEGIN
    INSERT INTO [dbo].[PaymentMethods]([Id], [Name], [CssClass], [IconClass], [HasBeenAlreadyDebitedOption]) VALUES(4, 'Transfer', 'warning', 'fa fa-external-link', 0)
END

IF NOT EXISTS (SELECT * FROM [dbo].[PaymentMethods] WHERE [Id] = 5)
BEGIN
    INSERT INTO [dbo].[PaymentMethods]([Id], [Name], [CssClass], [IconClass], [HasBeenAlreadyDebitedOption]) VALUES(5, 'Internal Transfer', 'danger', 'fa fa-refresh', 0)
END
