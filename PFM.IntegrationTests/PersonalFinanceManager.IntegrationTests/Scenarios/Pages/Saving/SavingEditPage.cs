using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Saving
{
    public class SavingEditPage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Saving/Edit/{id}";

        private const string SavingPropertyId = "Id";
        private const string AmountPropertyId = "Amount";
        private const string TargetInternalAccountPropertyId = "TargetInternalAccountId";

        public SavingEditPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void EnterAmount(decimal amount)
        {
            var field = WebDriver.FindElement(By.Id(AmountPropertyId));
            field.Clear();
            field.SendKeys(amount.ToString(CultureInfo.InvariantCulture));
        }

        public int SelectAnotherSavingAccount()
        {
            var accountDdl = new SelectElement(WebDriver.FindElement(By.Id(TargetInternalAccountPropertyId)));
            if (accountDdl.Options.Count < 3)
                throw new Exception("TargetInternalAccountId has no enough option. At least 2 expected.");

            var selectedValueOption = Convert.ToInt32(accountDdl.AllSelectedOptions[0].GetAttribute("value"));

            var savingAccounts = new List<Tuple<int, int, bool>>();
            for (var ind = 1; ind < accountDdl.Options.Count; ind++)
            {
                var value = Convert.ToInt32(accountDdl.Options[ind].GetAttribute("value"));
                savingAccounts.Add(new Tuple<int, int, bool>(ind, value, value == selectedValueOption));
            }

            var firstNotSelectedIndex = savingAccounts.First(x => !x.Item3);

            accountDdl.SelectByIndex(firstNotSelectedIndex.Item1);

            return firstNotSelectedIndex.Item2;
        }

        public int FindSavingId()
        {
            var id = WebDriver.FindElement(By.Id(SavingPropertyId));
            return Convert.ToInt32(id.GetAttribute("value"));
        }
    }
}
