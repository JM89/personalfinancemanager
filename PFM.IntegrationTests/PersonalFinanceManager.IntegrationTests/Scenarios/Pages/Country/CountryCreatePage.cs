using System;
using System.Globalization;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income
{
    public class CountryCreatePage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Country/Create";

        private const string NamePropertyId = "Name";

        internal void QuickCreate()
        {
            throw new NotImplementedException();
        }

        public CountryCreatePage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void EnterName(string text)
        {
            var field = WebDriver.FindElement(By.Id(NamePropertyId));
            field.Clear();
            field.SendKeys(text);
        }
    }
}
