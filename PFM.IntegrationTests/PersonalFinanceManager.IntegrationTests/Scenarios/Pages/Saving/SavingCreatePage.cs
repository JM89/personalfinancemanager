using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Saving
{
    public class SavingCreatePage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Saving/Create";

        private const string AmountPropertyId = "Amount";
        private const string TargetInternalAccountPropertyId = "TargetInternalAccountId";

        public SavingCreatePage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void EnterAmount(decimal amount)
        {
            var field = WebDriver.FindElement(By.Id(AmountPropertyId));
            field.Clear();
            field.SendKeys(amount.ToString(CultureInfo.InvariantCulture));
        }

        public int SelectFirstSavingAccount()
        {
            var accountDdl = new SelectElement(WebDriver.FindElement(By.Id(TargetInternalAccountPropertyId)));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            return Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
        }

        public void QuickCreate()
        {
            GoTo();
            EnterAmount(100);
            SelectFirstSavingAccount();
            ClickSave();
        }
    }
}
