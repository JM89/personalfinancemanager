using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public static class WebDrivertExtensions
    {
        public static IWebElement FindElementAndWaitUntilDisplayed(this IWebDriver webDriver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            return webDriver.FindElement(by);
        }
    }
}
