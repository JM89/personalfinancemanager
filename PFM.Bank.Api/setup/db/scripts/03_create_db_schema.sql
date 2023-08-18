USE PFM_BANK_DB
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Accounts'))
BEGIN
	CREATE TABLE [dbo].[Accounts](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NULL,
		[BankId] [int] NOT NULL,
		[CurrencyId] [int] NOT NULL,
		[User_Id] [nvarchar](max) NULL,
		[InitialBalance] [decimal](9, 2) NOT NULL,
		[CurrentBalance] [decimal](9, 2) NOT NULL,
		[IsFavorite] [bit] NOT NULL,
		[IsSavingAccount] [bit] NOT NULL,
	CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Banks'))
BEGIN
	CREATE TABLE [dbo].[Banks](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NOT NULL,
		[IconPath] [nvarchar](max) NOT NULL,
		[CountryId] [int] NOT NULL,
		[Website] [nvarchar](max) NULL,
		[GeneralEnquiryPhoneNumber] [nvarchar](max) NULL,
		[User_Id] [nvarchar](max) NULL,
	CONSTRAINT [PK_Banks] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Countries'))
BEGIN
	CREATE TABLE [dbo].[Countries](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Currencies'))
BEGIN
	CREATE TABLE [dbo].[Currencies](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NOT NULL,
		[Symbol] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_Currencies] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Accounts_Banks_BankId'))
BEGIN
	ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Banks_BankId] FOREIGN KEY([BankId])
	REFERENCES [dbo].[Banks] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Banks_BankId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Accounts_Currencies_CurrencyId'))
BEGIN
	ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Currencies_CurrencyId] FOREIGN KEY([CurrencyId])
	REFERENCES [dbo].[Currencies] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Currencies_CurrencyId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Banks_Countries_CountryId'))
BEGIN
	ALTER TABLE [dbo].[Banks]  WITH CHECK ADD  CONSTRAINT [FK_Banks_Countries_CountryId] FOREIGN KEY([CountryId])
	REFERENCES [dbo].[Countries] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Banks] CHECK CONSTRAINT [FK_Banks_Countries_CountryId]
END
GO

