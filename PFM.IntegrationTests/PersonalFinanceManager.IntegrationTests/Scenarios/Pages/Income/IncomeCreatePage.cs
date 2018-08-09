using System.Globalization;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income
{
    public class IncomeCreatePage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Income/Create";

        private const string DescriptionPropertyId = "Description";
        private const string CostPropertyId = "Cost";

        public IncomeCreatePage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
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

        public void QuickCreate()
        {
            GoTo();
            EnterDescription("Income Description");
            EnterCost(100);
            ClickSave();
        }
    }
}
