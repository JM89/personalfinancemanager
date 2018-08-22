using System;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Income
{
    public class CountryListPage : Infrastructure.BasePages.ListPage
    {
        public override string PageUrl => "/Country/Index";

        public override string DeletionConfirmationModalTitle => "Delete a country";
        public override string RowClassName => "trCountry";

        public CountryListPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }
    }
}
