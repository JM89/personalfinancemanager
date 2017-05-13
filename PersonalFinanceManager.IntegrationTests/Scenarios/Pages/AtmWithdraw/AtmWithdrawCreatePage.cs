using System;
using System.Globalization;
using OpenQA.Selenium;
using System.Threading;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.AtmWithdraw
{
    public class AtmWithdrawCreatePage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/AtmWithdraw/Create";

        private const string CostPropertyId = "InitialAmount";

        public AtmWithdrawCreatePage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
            
        }

        public void EnterCost(double cost)
        {
            var initialAmountTxt = WebDriver.FindElement(By.Id(CostPropertyId));
            initialAmountTxt.Clear();
            initialAmountTxt.SendKeys(cost.ToString(CultureInfo.InvariantCulture));
        }

        public void QuickCreate()
        {
            GoTo();
            EnterCost(100);
            ClickSave();
            Thread.Sleep(2000);
        }
    }
}
