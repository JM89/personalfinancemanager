using System;
using OpenQA.Selenium;
using System.Linq;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using System.Threading;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.AccountManagement
{
    public class AccountManagementDashboardPage : Infrastructure.BasePages.BasePage
    {
        public override string PageUrl => "/AccountManagement/Index";

        private const string AccountDropDownListClassName = "dd-selected-value";

        public AccountManagementDashboardPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {

        }

        public int SelectAccount()
        {
            Thread.Sleep(1000);
            var mainAccountDd = WebDriver.FindElements(By.ClassName(AccountDropDownListClassName));
            var firstAccount = mainAccountDd.FirstOrDefault();
            if (firstAccount == null)
                throw new Exception("Account has no option. At least 1 expected.");
            return Convert.ToInt32(firstAccount.GetAttribute("value"));
        }
    }
}
