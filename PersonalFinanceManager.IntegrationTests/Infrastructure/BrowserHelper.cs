using System;
using System.Configuration;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public static class BrowserHelper
    {
        private static IWebDriver _webDriver;

        public static void Initialize()
        {
            var browserType = (BrowserType)Enum.Parse(typeof(BrowserType), ConfigurationManager.AppSettings["BrowserType"]);
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            switch (browserType)
            {
                case BrowserType.Chrome:
                    _webDriver = StartChrome();
                    SiteMap.Initialize(_webDriver, baseUrl);
                    DatabaseChecker.Initialize();
                    SiteMap.LoginPage.QuickLogin("test@test.com", "Helloworld1!");
                    break;
                default:
                    throw new NotImplementedException();
            }         
        }

        private static ChromeDriver StartChrome()
        {
            var assemblyPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(BrowserHelper).Assembly.Location)));
            var hideBrowser = bool.Parse(ConfigurationManager.AppSettings["HideBrowser"]);

            ChromeDriver webDriver;
            var driverPath = $"{assemblyPath}\\Drivers";
            if (hideBrowser)
            {
                var service = ChromeDriverService.CreateDefaultService(driverPath);
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-32000,-32000");

                webDriver = new ChromeDriver(service, options);
            }
            else
            {
                webDriver = new ChromeDriver(driverPath);
            }
            return webDriver;
        }

        public static void StopTest()
        {
            _webDriver.Quit();
        }
    }
}
