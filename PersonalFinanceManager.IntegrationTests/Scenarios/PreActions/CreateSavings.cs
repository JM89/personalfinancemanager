using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.PreActions
{
    public class CreateSavings
    {
        public static void Execute(IntegrationTestContext ctx)
        {
            ctx.GotToUrl("/Saving/Index");

            var createBtn = ctx.WebDriver.FindElement(By.ClassName("btn_create"));
            createBtn.Click();

            var amountTxt = ctx.WebDriver.FindElement(By.Id("Amount"));
            amountTxt.Clear();
            amountTxt.SendKeys("100.00");

            var accountDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            accountDdl.SelectByIndex(1);

            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
    }
}
