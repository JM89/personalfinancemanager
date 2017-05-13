using System.Configuration;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure.BasePages
{
    public abstract class BasePage
    {
        protected IWebDriver WebDriver;
        protected string BaseUrl;

        public abstract string PageUrl { get; }

        protected BasePage(IWebDriver webDriver, string baseUrl)
        {
            this.WebDriver = webDriver;
            this.BaseUrl = baseUrl;
        }

        public void GoTo()
        {
            WebDriver.Url = BaseUrl + PageUrl;
        }
    }
}
