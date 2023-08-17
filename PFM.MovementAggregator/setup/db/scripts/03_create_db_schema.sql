USE PFM_MVT_AGGR_DB
GO

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'MovementAggregation'))
BEGIN
	CREATE TABLE [dbo].[MovementAggregation](
		[BankAccountId] [int] NOT NULL,
        [MonthYearIdentifier] [nvarchar](8) NOT NULL,
		[Type] [nvarchar](25) NOT NULL, /* Type: Expenses, Savings, Incomes */
		[Category] [nvarchar](50) NOT NULL, /* Category: Expense Type (for expense), 'Savings' (for savings), 'Incomes' (for incomes) */
        [AggregatedAmount] [decimal](9, 2) NOT NULL,
	CONSTRAINT [PK_MovementAggregation] PRIMARY KEY CLUSTERED 
	(
		[BankAccountId] ASC, [MonthYearIdentifier] ASC, [Type] ASC, [Category] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] 
END
GO