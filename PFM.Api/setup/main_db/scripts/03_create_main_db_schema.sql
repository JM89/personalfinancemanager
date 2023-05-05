USE PFM_MAIN_DB
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 14/04/2023 18:02:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'AtmWithdraws'))
BEGIN
	CREATE TABLE [dbo].[AtmWithdraws](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[AccountId] [int] NOT NULL,
		[DateExpense] [datetime2](7) NOT NULL,
		[InitialAmount] [decimal](9, 2) NOT NULL,
		[CurrentAmount] [decimal](9, 2) NOT NULL,
		[IsClosed] [bit] NOT NULL,
		[HasBeenAlreadyDebited] [bit] NOT NULL,
	CONSTRAINT [PK_AtmWithdraws] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[BudgetByExpenseTypes]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'BudgetByExpenseTypes'))
BEGIN
	CREATE TABLE [dbo].[BudgetByExpenseTypes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ExpenseTypeId] [int] NOT NULL,
		[Budget] [decimal](9, 2) NOT NULL,
		[Actual] [decimal](9, 2) NULL,
		[AccountId] [int] NOT NULL,
		[BudgetPlanId] [int] NOT NULL,
	CONSTRAINT [PK_BudgetByExpenseTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[BudgetPlans]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'BudgetPlans'))
BEGIN
	CREATE TABLE [dbo].[BudgetPlans](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NOT NULL,
		[StartDate] [datetime2](7) NULL,
		[EndDate] [datetime2](7) NULL,
		[ExpectedIncomes] [decimal](9, 2) NOT NULL,
		[ExpectedSavings] [decimal](9, 2) NOT NULL,
	CONSTRAINT [PK_BudgetPlans] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Expenses]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Expenses'))
BEGIN
	CREATE TABLE [dbo].[Expenses](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[AccountId] [int] NOT NULL,
		[DateExpense] [datetime2](7) NOT NULL,
		[Cost] [decimal](9, 2) NOT NULL,
		[ExpenseTypeId] [int] NOT NULL,
		[PaymentMethodId] [int] NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[HasBeenAlreadyDebited] [bit] NOT NULL,
		[AtmWithdrawId] [int] NULL,
		[TargetInternalAccountId] [int] NULL,
		[GeneratedIncomeId] [int] NULL,
	CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ExpenseTypes]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ExpenseTypes'))
BEGIN
	CREATE TABLE [dbo].[ExpenseTypes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NOT NULL,
		[GraphColor] [nvarchar](max) NOT NULL,
		[ShowOnDashboard] [bit] NOT NULL,
	CONSTRAINT [PK_ExpenseTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[FrequenceOptions]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'FrequenceOptions'))
BEGIN
	CREATE TABLE [dbo].[FrequenceOptions](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](max) NULL,
	CONSTRAINT [PK_FrequenceOptions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Incomes]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Incomes'))
BEGIN
	CREATE TABLE [dbo].[Incomes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[AccountId] [int] NOT NULL,
		[Cost] [decimal](9, 2) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[DateIncome] [datetime2](7) NOT NULL,
	CONSTRAINT [PK_Incomes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PaymentMethods'))
BEGIN
	CREATE TABLE [dbo].[PaymentMethods](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](max) NULL,
		[CssClass] [nvarchar](max) NULL,
		[IconClass] [nvarchar](max) NULL,
		[HasBeenAlreadyDebitedOption] [bit] NOT NULL,
	CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Pensions]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Pensions'))
BEGIN
	CREATE TABLE [dbo].[Pensions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[Website] [nvarchar](max) NOT NULL,
		[StartDate] [datetime2](7) NOT NULL,
		[EndDate] [datetime2](7) NULL,
		[ContributionPercentage] [decimal](9, 2) NOT NULL,
		[CompanyContributionPercentage] [decimal](9, 2) NOT NULL,
		[CurrentPot] [decimal](9, 2) NOT NULL,
		[Interest] [decimal](9, 2) NOT NULL,
		[UserId] [nvarchar](max) NOT NULL,
		[CurrencyId] [int] NOT NULL,
		[CountryId] [int] NOT NULL,
	CONSTRAINT [PK_Pensions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Salaries]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Salaries'))
BEGIN
	CREATE TABLE [dbo].[Salaries](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[StartDate] [datetime2](7) NOT NULL,
		[EndDate] [datetime2](7) NULL,
		[YearlySalary] [decimal](9, 2) NOT NULL,
		[MonthlyGrossPay] [decimal](9, 2) NOT NULL,
		[MonthlyNetPay] [decimal](9, 2) NOT NULL,
		[TaxId] [int] NOT NULL,
		[UserId] [nvarchar](max) NOT NULL,
		[CurrencyId] [int] NOT NULL,
		[CountryId] [int] NOT NULL,
	CONSTRAINT [PK_Salaries] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SalaryDeductions]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'SalaryDeductions'))
BEGIN
	CREATE TABLE [dbo].[SalaryDeductions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[Amount] [decimal](9, 2) NOT NULL,
		[SalaryId] [int] NOT NULL,
	CONSTRAINT [PK_SalaryDeductions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Savings]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Savings'))
BEGIN
	CREATE TABLE [dbo].[Savings](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[AccountId] [int] NOT NULL,
		[DateSaving] [datetime2](7) NOT NULL,
		[Amount] [decimal](9, 2) NOT NULL,
		[TargetInternalAccountId] [int] NOT NULL,
		[GeneratedIncomeId] [int] NULL,
	CONSTRAINT [PK_Savings] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Taxes]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Taxes'))
BEGIN
	CREATE TABLE [dbo].[Taxes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[Code] [nvarchar](max) NULL,
		[StartDate] [datetime2](7) NOT NULL,
		[EndDate] [datetime2](7) NULL,
		[UserId] [nvarchar](max) NOT NULL,
		[TaxTypeId] [int] NOT NULL,
		[FrequenceOptionId] [int] NOT NULL,
		[Amount] [decimal](9, 2) NOT NULL,
		[Frequence] [int] NULL,
		[CurrencyId] [int] NOT NULL,
		[CountryId] [int] NOT NULL,
	CONSTRAINT [PK_Taxes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TaxTypes]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TaxTypes'))
BEGIN
	CREATE TABLE [dbo].[TaxTypes](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](max) NOT NULL,
		[Description] [nvarchar](max) NULL,
	CONSTRAINT [PK_TaxTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 14/04/2023 18:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'UserProfiles'))
BEGIN
	CREATE TABLE [dbo].[UserProfiles](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[User_Id] [nvarchar](max) NOT NULL,
		[FirstName] [nvarchar](max) NOT NULL,
		[LastName] [nvarchar](max) NOT NULL,
		[YearlyWages] [decimal](9, 2) NOT NULL,
		[SourceWages] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_BudgetByExpenseTypes_BudgetPlans_BudgetPlanId'))
BEGIN
	ALTER TABLE [dbo].[BudgetByExpenseTypes]  WITH CHECK ADD  CONSTRAINT [FK_BudgetByExpenseTypes_BudgetPlans_BudgetPlanId] FOREIGN KEY([BudgetPlanId])
	REFERENCES [dbo].[BudgetPlans] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[BudgetByExpenseTypes] CHECK CONSTRAINT [FK_BudgetByExpenseTypes_BudgetPlans_BudgetPlanId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_BudgetByExpenseTypes_ExpenseTypes_ExpenseTypeId'))
BEGIN
	ALTER TABLE [dbo].[BudgetByExpenseTypes]  WITH CHECK ADD  CONSTRAINT [FK_BudgetByExpenseTypes_ExpenseTypes_ExpenseTypeId] FOREIGN KEY([ExpenseTypeId])
	REFERENCES [dbo].[ExpenseTypes] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[BudgetByExpenseTypes] CHECK CONSTRAINT [FK_BudgetByExpenseTypes_ExpenseTypes_ExpenseTypeId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Expenses_AtmWithdraws_AtmWithdrawId'))
BEGIN
	ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_AtmWithdraws_AtmWithdrawId] FOREIGN KEY([AtmWithdrawId])
	REFERENCES [dbo].[AtmWithdraws] ([Id])
	ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_AtmWithdraws_AtmWithdrawId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Expenses_ExpenseTypes_ExpenseTypeId'))
BEGIN
	ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_ExpenseTypes_ExpenseTypeId] FOREIGN KEY([ExpenseTypeId])
	REFERENCES [dbo].[ExpenseTypes] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_ExpenseTypes_ExpenseTypeId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Expenses_PaymentMethods_PaymentMethodId'))
BEGIN
	ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_PaymentMethods_PaymentMethodId] FOREIGN KEY([PaymentMethodId])
	REFERENCES [dbo].[PaymentMethods] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_PaymentMethods_PaymentMethodId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Salaries_Taxes_TaxId'))
BEGIN
	ALTER TABLE [dbo].[Salaries]  WITH CHECK ADD  CONSTRAINT [FK_Salaries_Taxes_TaxId] FOREIGN KEY([TaxId])
	REFERENCES [dbo].[Taxes] ([Id])
	ALTER TABLE [dbo].[Salaries] CHECK CONSTRAINT [FK_Salaries_Taxes_TaxId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_SalaryDeductions_Salaries_SalaryId'))
BEGIN
	ALTER TABLE [dbo].[SalaryDeductions]  WITH CHECK ADD  CONSTRAINT [FK_SalaryDeductions_Salaries_SalaryId] FOREIGN KEY([SalaryId])
	REFERENCES [dbo].[Salaries] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[SalaryDeductions] CHECK CONSTRAINT [FK_SalaryDeductions_Salaries_SalaryId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Savings_Incomes_GeneratedIncomeId'))
BEGIN
	ALTER TABLE [dbo].[Savings]  WITH CHECK ADD  CONSTRAINT [FK_Savings_Incomes_GeneratedIncomeId] FOREIGN KEY([GeneratedIncomeId])
	REFERENCES [dbo].[Incomes] ([Id])
	ALTER TABLE [dbo].[Savings] CHECK CONSTRAINT [FK_Savings_Incomes_GeneratedIncomeId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Taxes_FrequenceOptions_FrequenceOptionId'))
BEGIN
	ALTER TABLE [dbo].[Taxes]  WITH CHECK ADD  CONSTRAINT [FK_Taxes_FrequenceOptions_FrequenceOptionId] FOREIGN KEY([FrequenceOptionId])
	REFERENCES [dbo].[FrequenceOptions] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Taxes] CHECK CONSTRAINT [FK_Taxes_FrequenceOptions_FrequenceOptionId]
END
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' AND CONSTRAINT_NAME = 'FK_Taxes_TaxTypes_TaxTypeId'))
BEGIN
	ALTER TABLE [dbo].[Taxes]  WITH CHECK ADD  CONSTRAINT [FK_Taxes_TaxTypes_TaxTypeId] FOREIGN KEY([TaxTypeId])
	REFERENCES [dbo].[TaxTypes] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[Taxes] CHECK CONSTRAINT [FK_Taxes_TaxTypes_TaxTypeId]
END
GO
