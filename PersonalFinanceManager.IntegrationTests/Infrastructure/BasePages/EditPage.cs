using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure.BasePages
{
    public abstract class EditPage : BasePage
    {
        private const string SaveButtonClassName = "btn_save";

        protected EditPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        protected void GoTo(int id)
        {
            WebDriver.Url = BaseUrl + PageUrl + "/" + id;
        }

        public void ClickSave()
        {
            var saveBtn = WebDriver.FindElement(By.ClassName(SaveButtonClassName));
            saveBtn.Click();
            Thread.Sleep(2000);
        }

        public void ClickCancel()
        {

        }
    }
}
