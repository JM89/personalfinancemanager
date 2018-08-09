using System;
using System.Globalization;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income
{
    public class IncomeEditPage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Income/Edit/{id}";

        private const string IncomePropertyId = "Id";
        private const string DescriptionPropertyId = "Description";
        private const string CostPropertyId = "Cost";

        public IncomeEditPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void EnterDescription(string text)
        {
            var field = WebDriver.FindElement(By.Id(DescriptionPropertyId));
            field.Clear();
            field.SendKeys(text);
        }

        public void EnterCost(decimal cost)
        {
            var field = WebDriver.FindElement(By.Id(CostPropertyId));
            field.Clear();
            field.SendKeys(cost.ToString(CultureInfo.InvariantCulture));
        }

        public int FindIncomeId()
        {
            var id = WebDriver.FindElement(By.Id(IncomePropertyId));
            return Convert.ToInt32(id.GetAttribute("value"));
        }
    }
}
