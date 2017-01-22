using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.PreActions
{
    public class CreateIncomes
    {
        public static void Execute(IntegrationTestContext ctx)
        {
            ctx.GotToUrl("/Income/Index");

            ctx.WebDriver.FindElement(By.ClassName("btn_create")).Click();

            var descriptionTxt = ctx.WebDriver.FindElement(By.Id("Description"));
            descriptionTxt.Clear();
            descriptionTxt.SendKeys("Income Description");

            var costTxt = ctx.WebDriver.FindElement(By.Id("Cost"));
            costTxt.Clear();
            costTxt.SendKeys("100.00");

            ctx.WebDriver.FindElement(By.ClassName("btn_save")).Click();

            Thread.Sleep(2000);
        }
    }
}
