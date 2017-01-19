using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public class IntegrationTestContext
    {
        public IWebDriver WebDriver { get; set; }

        private string baseUrl;

        public IntegrationTestContext()
        {
            string assemblyPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(IntegrationTestContext).Assembly.Location)));

            this.baseUrl = "http://localhost:54401";
            this.WebDriver = new ChromeDriver($"{assemblyPath}\\Drivers");
            Login(); 
        }

        public void GotToUrl(string url)
        {
            this.WebDriver.Url = this.baseUrl + url;
        }

        public void Login()
        {
            GotToUrl("/Account/Login");
            var emailTxt = this.WebDriver.FindElement(By.Id("Email"));
            emailTxt.SendKeys("test@test.com");
            var pwdTxt = this.WebDriver.FindElement(By.Id("Password"));
            pwdTxt.SendKeys("Helloworld1!");
            var loginBtn = this.WebDriver.FindElement(By.Id("btn_login"));
            loginBtn.Click();
        }

        public int SelectedSourceAccountId()
        {
            var mainAccountDd = WebDriver.FindElements(By.ClassName("dd-selected-value"));
            var firstAccount = mainAccountDd.FirstOrDefault();
            if (firstAccount == null)
                throw new Exception("Account has no option. At least 1 expected.");
            return Convert.ToInt32(firstAccount.GetAttribute("value"));
        }
    }
}
