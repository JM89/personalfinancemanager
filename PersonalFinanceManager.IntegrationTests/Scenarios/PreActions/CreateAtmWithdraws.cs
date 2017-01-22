using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.PreActions
{
    public class CreateAtmWithdraws
    {
        public static void Execute(IntegrationTestContext ctx)
        {
            ctx.GotToUrl("/AtmWithdraw/Index");

            ctx.WebDriver.FindElement(By.ClassName("btn_create")).Click();

            var initialAmountTxt = ctx.WebDriver.FindElement(By.Id("InitialAmount"));
            initialAmountTxt.Clear();
            initialAmountTxt.SendKeys("100.00");

            ctx.WebDriver.FindElement(By.ClassName("btn_save")).Click();

            Thread.Sleep(2000);
        }
    }
}
