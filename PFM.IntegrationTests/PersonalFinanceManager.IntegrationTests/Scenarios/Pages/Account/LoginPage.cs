using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using PersonalFinanceManager.IntegrationTests.Infrastructure.BasePages;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Pages.Account
{
    public class LoginPage : BasePage
    {
        public override string PageUrl => "/Account/Login";

        private const string EmailPropertyId = "Email";
        private const string PasswordPropertyId = "Password";
        private const string LoginButtonPropertyId = "btn_login";

        public LoginPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {

        }

        public void QuickLogin(string email, string password)
        {
            GoTo();
            EnterEmail(email);
            EnterPassword(password);
            ClickLoginButton();
        }

        public void EnterEmail(string email)
        {
            var emailTxt = WebDriver.FindElement(By.Id(EmailPropertyId));
            emailTxt.SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            var pwdTxt = WebDriver.FindElement(By.Id(PasswordPropertyId));
            pwdTxt.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            var loginBtn = WebDriver.FindElement(By.Id(LoginButtonPropertyId));
            loginBtn.Click();
        }
    }
}
