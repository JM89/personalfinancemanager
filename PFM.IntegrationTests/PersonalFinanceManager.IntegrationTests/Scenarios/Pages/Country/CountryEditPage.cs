using System;
using System.Globalization;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income
{
    public class CountryEditPage : Infrastructure.BasePages.EditPage
    {
        public override string PageUrl => "/Country/Edit/{id}";

        public CountryEditPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }
    }
}
