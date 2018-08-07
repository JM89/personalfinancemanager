using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PFM.DataAccessLayer.Migrations
{
    public partial class allentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Banks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IconPath",
                table: "Banks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BankBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    AddressLine1 = table.Column<string>(nullable: false),
                    AddressLine2 = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    BankId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankBranches_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ExpectedIncomes = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    ExpectedSavings = table.Column<decimal>(type: "decimal(9,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Symbol = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    GraphColor = table.Column<string>(nullable: false),
                    ShowOnDashboard = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrequenceOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequenceOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoricMovements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SourceId = table.Column<int>(nullable: false),
                    SourceType = table.Column<int>(nullable: false),
                    SourceOldAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    SourceNewAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    DestinationId = table.Column<int>(nullable: true),
                    DestinationType = table.Column<int>(nullable: false),
                    DestinationOldAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    DestinationNewAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(9,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricMovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CssClass = table.Column<string>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    HasBeenAlreadyDebitedOption = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_Id = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    YearlyWages = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    SourceWages = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    BankId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    User_Id = table.Column<string>(nullable: true),
                    InitialBalance = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    IsFavorite = table.Column<bool>(nullable: false),
                    IsSavingAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Currencys_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pensions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Website = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ContributionPercentage = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    CompanyContributionPercentage = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    CurrentPot = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Interest = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pensions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pensions_Currencys_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    TaxTypeId = table.Column<int>(nullable: false),
                    FrequenceOptionId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Frequence = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxes_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Taxes_Currencys_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Taxes_FrequenceOptions_FrequenceOptionId",
                        column: x => x.FrequenceOptionId,
                        principalTable: "FrequenceOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Taxes_TaxTypes_TaxTypeId",
                        column: x => x.TaxTypeId,
                        principalTable: "TaxTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtmWithdraws",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    DateExpense = table.Column<DateTime>(nullable: false),
                    InitialAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    HasBeenAlreadyDebited = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtmWithdraws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtmWithdraws_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetByExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpenseTypeId = table.Column<int>(nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Actual = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    BudgetPlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetByExpenseTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetByExpenseTypes_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetByExpenseTypes_BudgetPlans_BudgetPlanId",
                        column: x => x.BudgetPlanId,
                        principalTable: "BudgetPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetByExpenseTypes_ExpenseTypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(nullable: false),
                    DateIncome = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    YearlySalary = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    MonthlyGrossPay = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    MonthlyNetPay = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    TaxId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salaries_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salaries_Currencys_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salaries_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    DateExpenditure = table.Column<DateTime>(nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    ExpenseTypeId = table.Column<int>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    HasBeenAlreadyDebited = table.Column<bool>(nullable: false),
                    AtmWithdrawId = table.Column<int>(nullable: true),
                    TargetInternalAccountId = table.Column<int>(nullable: true),
                    GeneratedIncomeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_AtmWithdraws_AtmWithdrawId",
                        column: x => x.AtmWithdrawId,
                        principalTable: "AtmWithdraws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseTypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_TargetInternalAccountId",
                        column: x => x.TargetInternalAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Savings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    DateSaving = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    TargetInternalAccountId = table.Column<int>(nullable: false),
                    GeneratedIncomeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Savings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Savings_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Savings_Incomes_GeneratedIncomeId",
                        column: x => x.GeneratedIncomeId,
                        principalTable: "Incomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Savings_Accounts_TargetInternalAccountId",
                        column: x => x.TargetInternalAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalaryDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    SalaryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryDeductions_Salaries_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "Salaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BankId",
                table: "Accounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CurrencyId",
                table: "Accounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AtmWithdraws_AccountId",
                table: "AtmWithdraws",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranches_BankId",
                table: "BankBranches",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetByExpenseTypes_AccountId",
                table: "BudgetByExpenseTypes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetByExpenseTypes_BudgetPlanId",
                table: "BudgetByExpenseTypes",
                column: "BudgetPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetByExpenseTypes_ExpenseTypeId",
                table: "BudgetByExpenseTypes",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AtmWithdrawId",
                table: "Expenses",
                column: "AtmWithdrawId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseTypeId",
                table: "Expenses",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PaymentMethodId",
                table: "Expenses",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TargetInternalAccountId",
                table: "Expenses",
                column: "TargetInternalAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_AccountId",
                table: "Incomes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Pensions_CountryId",
                table: "Pensions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pensions_CurrencyId",
                table: "Pensions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_CountryId",
                table: "Salaries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_CurrencyId",
                table: "Salaries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_TaxId",
                table: "Salaries",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDeductions_SalaryId",
                table: "SalaryDeductions",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_AccountId",
                table: "Savings",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_GeneratedIncomeId",
                table: "Savings",
                column: "GeneratedIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_TargetInternalAccountId",
                table: "Savings",
                column: "TargetInternalAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_CountryId",
                table: "Taxes",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_CurrencyId",
                table: "Taxes",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_FrequenceOptionId",
                table: "Taxes",
                column: "FrequenceOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_TaxTypeId",
                table: "Taxes",
                column: "TaxTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankBranches");

            migrationBuilder.DropTable(
                name: "BudgetByExpenseTypes");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "HistoricMovements");

            migrationBuilder.DropTable(
                name: "Pensions");

            migrationBuilder.DropTable(
                name: "SalaryDeductions");

            migrationBuilder.DropTable(
                name: "Savings");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "BudgetPlans");

            migrationBuilder.DropTable(
                name: "AtmWithdraws");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Salaries");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "FrequenceOptions");

            migrationBuilder.DropTable(
                name: "TaxTypes");

            migrationBuilder.DropTable(
                name: "Currencys");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Banks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IconPath",
                table: "Banks",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
