using Microsoft.EntityFrameworkCore.Migrations;

namespace PFM.DataAccessLayer.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Currencys_CurrencyId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Pensions_Currencys_CurrencyId",
                table: "Pensions");

            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_Currencys_CurrencyId",
                table: "Salaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_Currencys_CurrencyId",
                table: "Taxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencys",
                table: "Currencys");

            migrationBuilder.RenameTable(
                name: "Currencys",
                newName: "Currencies");

            migrationBuilder.RenameColumn(
                name: "DateExpenditure",
                table: "Expenses",
                newName: "DateExpense");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Currencies_CurrencyId",
                table: "Accounts",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pensions_Currencies_CurrencyId",
                table: "Pensions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_Currencies_CurrencyId",
                table: "Salaries",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_Currencies_CurrencyId",
                table: "Taxes",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Currencies_CurrencyId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Pensions_Currencies_CurrencyId",
                table: "Pensions");

            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_Currencies_CurrencyId",
                table: "Salaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_Currencies_CurrencyId",
                table: "Taxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currencys");

            migrationBuilder.RenameColumn(
                name: "DateExpense",
                table: "Expenses",
                newName: "DateExpenditure");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencys",
                table: "Currencys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Currencys_CurrencyId",
                table: "Accounts",
                column: "CurrencyId",
                principalTable: "Currencys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pensions_Currencys_CurrencyId",
                table: "Pensions",
                column: "CurrencyId",
                principalTable: "Currencys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_Currencys_CurrencyId",
                table: "Salaries",
                column: "CurrencyId",
                principalTable: "Currencys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_Currencys_CurrencyId",
                table: "Taxes",
                column: "CurrencyId",
                principalTable: "Currencys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
