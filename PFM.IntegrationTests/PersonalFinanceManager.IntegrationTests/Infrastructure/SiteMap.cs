using PersonalFinanceManager.IntegrationTests.Scenarios.Pages.AccountManagement;
using PersonalFinanceManager.IntegrationTests.Scenarios.Pages.AtmWithdraw;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Account;
using PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Expense;
using PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income;
using PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Saving;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public static class SiteMap
    {
        public static AccountManagementDashboardPage AccountManagementDashboardPage;
        public static AtmWithdrawListPage AtmWithdrawListPage;
        public static AtmWithdrawCreatePage AtmWithdrawCreatePage;
        public static AtmWithdrawEditPage AtmWithdrawEditPage;
        public static ExpenseListPage ExpenseListPage;
        public static ExpenseCreatePage ExpenseCreatePage;
        public static ExpenseEditPage ExpenseEditPage;
        public static IncomeListPage IncomeListPage;
        public static IncomeCreatePage IncomeCreatePage;
        public static IncomeEditPage IncomeEditPage;
        public static SavingListPage SavingListPage;
        public static SavingCreatePage SavingCreatePage;
        public static SavingEditPage SavingEditPage;
        public static LoginPage LoginPage;
        public static CountryListPage CountryListPage;
        public static CountryEditPage CountryEditPage;
        public static CountryCreatePage CountryCreatePage;

        public static void Initialize(IWebDriver webDriver, string baseUrl)
        {
            AccountManagementDashboardPage = new AccountManagementDashboardPage(webDriver, baseUrl);
            AtmWithdrawListPage = new AtmWithdrawListPage(webDriver, baseUrl);
            AtmWithdrawCreatePage = new AtmWithdrawCreatePage(webDriver, baseUrl);
            AtmWithdrawEditPage = new AtmWithdrawEditPage(webDriver, baseUrl);
            ExpenseListPage = new ExpenseListPage(webDriver, baseUrl);
            ExpenseCreatePage = new ExpenseCreatePage(webDriver, baseUrl);
            ExpenseEditPage = new ExpenseEditPage(webDriver, baseUrl);
            ExpenseListPage = new ExpenseListPage(webDriver, baseUrl);
            IncomeListPage = new IncomeListPage(webDriver, baseUrl);
            IncomeCreatePage = new IncomeCreatePage(webDriver, baseUrl);
            IncomeEditPage = new IncomeEditPage(webDriver, baseUrl);
            SavingListPage = new SavingListPage(webDriver, baseUrl);
            SavingCreatePage = new SavingCreatePage(webDriver, baseUrl);
            SavingEditPage = new SavingEditPage(webDriver, baseUrl);
            LoginPage = new LoginPage(webDriver, baseUrl);
            CountryListPage = new CountryListPage(webDriver, baseUrl);
            CountryEditPage = new CountryEditPage(webDriver, baseUrl);
            CountryCreatePage = new CountryCreatePage(webDriver, baseUrl);
        }
    }
}
