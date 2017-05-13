using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Expense
{
    public class ExpenseEditPage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Expenditure/Edit/{id}";

        private const string CostPropertyId = "Cost";
        private const string DescriptionPropertyId = "Description";
        private const string ExpenseTypePropertyId = "TypeExpenditureId";
        private const string PaymentMethodPropertyId = "paymentMethodId";
        private const string AtmWithdrawPropertyId = "AtmWithdrawId";
        private const string TargetInternalPropertyId = "TargetInternalAccountId";
        private const string ExpensePropertyId = "Id";

        public ExpenseEditPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void EnterCost(decimal cost)
        {
            var field = WebDriver.FindElement(By.Id(CostPropertyId));
            field.Clear();
            field.SendKeys(cost.ToString(CultureInfo.InvariantCulture));
        }

        public void EnterDescription(string text)
        {
            var field = WebDriver.FindElement(By.Id(DescriptionPropertyId));
            field.Clear();
            field.SendKeys(text);
        }

        public void SelectPaymentMethod(string value)
        {
            var paymentMethodDdl = new SelectElement(WebDriver.FindElement(By.Id(PaymentMethodPropertyId)));
            paymentMethodDdl.SelectByText(value);
            Thread.Sleep(2000);
        }

        public int SelectAnotherAtmWithdraw()
        {
            var atwWithdrawDdl = new SelectElement(WebDriver.FindElement(By.Id(AtmWithdrawPropertyId)));
            if (atwWithdrawDdl.Options.Count < 3)
                throw new Exception("AtmWithdrawId has no enough option. At least 2 expected.");

            var selectedValueOption = Convert.ToInt32(atwWithdrawDdl.AllSelectedOptions[0].GetAttribute("value"));

            var atmWithdraws = new List<Tuple<int, int, bool>>();
            for (var ind = 1; ind < atwWithdrawDdl.Options.Count; ind++)
            {
                var value = Convert.ToInt32(atwWithdrawDdl.Options[ind].GetAttribute("value"));
                atmWithdraws.Add(new Tuple<int, int, bool>(ind, value, value == selectedValueOption));
            }

            var firstNotSelectedIndex = atmWithdraws.First(x => !x.Item3);

            atwWithdrawDdl.SelectByIndex(firstNotSelectedIndex.Item1);

            return firstNotSelectedIndex.Item2;
        }

        public int FindTargetAccountId()
        {
            var account = WebDriver.FindElement(By.Id(TargetInternalPropertyId));
            return Convert.ToInt32(account.GetAttribute("value"));
        }

        public int FindAtmWithdrawId()
        {
            var atmWithdrawHid = WebDriver.FindElement(By.Id(AtmWithdrawPropertyId));
            return Convert.ToInt32(atmWithdrawHid.GetAttribute("value"));
        }

        public void SelectFirstExpenseType()
        {
            var expenditureTypeDdl = new SelectElement(WebDriver.FindElement(By.Id(ExpenseTypePropertyId)));
            if (expenditureTypeDdl.Options.Count < 2)
                throw new Exception("TypeExpenditureId has no option. At least 1 expected.");
            expenditureTypeDdl.SelectByIndex(1);
        }

        public int FindExpenseId()
        {
            var expenditureIdHid = WebDriver.FindElement(By.Id(ExpensePropertyId));
            return Convert.ToInt32(expenditureIdHid.GetAttribute("value"));
        }

        public int SelectFirstTargetAccount()
        {
            var accountDdl = new SelectElement(WebDriver.FindElement(By.Id(TargetInternalPropertyId)));
            if (accountDdl.Options.Count < 2)
                throw new Exception("TargetInternalAccountId has no option. At least 1 expected.");
            accountDdl.SelectByIndex(1);

            return Convert.ToInt32(accountDdl.Options[1].GetAttribute("value"));
        }

        public int SelectFirstAtmWithdraw()
        {
            var atmWithdrawDdl = new SelectElement(WebDriver.FindElement(By.Id(AtmWithdrawPropertyId)));
            if (atmWithdrawDdl.Options.Count < 2)
                throw new Exception("AtmWithdrawId has no enough option. At least 1 expected.");
            atmWithdrawDdl.SelectByIndex(1);

            return Convert.ToInt32(atmWithdrawDdl.Options[1].GetAttribute("value"));
        }

        public int SelectAnotherTargetAccount()
        {
            var accountDdl = new SelectElement(WebDriver.FindElement(By.Id(TargetInternalPropertyId)));
            if (accountDdl.Options.Count < 3)
                throw new Exception("TargetInternalAccountId has no enough option. At least 2 expected.");

            var selectedValueOption = Convert.ToInt32(accountDdl.AllSelectedOptions[0].GetAttribute("value"));

            var accounts = new List<Tuple<int, int, bool>>();
            for (var ind = 1; ind < accountDdl.Options.Count; ind++)
            {
                var value = Convert.ToInt32(accountDdl.Options[ind].GetAttribute("value"));
                accounts.Add(new Tuple<int, int, bool>(ind, value, value == selectedValueOption));
            }

            var firstNotSelectedIndex = accounts.First(x => !x.Item3);

            accountDdl.SelectByIndex(firstNotSelectedIndex.Item1);

            return firstNotSelectedIndex.Item2;
        }
    }
}
