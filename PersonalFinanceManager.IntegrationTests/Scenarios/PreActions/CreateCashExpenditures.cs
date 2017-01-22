using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.PreActions
{
    public class CreateCashExpenditures
    {
        public static void Execute(IntegrationTestContext ctx)
        {
            ctx.GotToUrl("/Expenditure/Index");

            ctx.WebDriver.FindElement(By.ClassName("btn_create")).Click();

            var descriptionTxt = ctx.WebDriver.FindElement(By.Id("Description"));
            descriptionTxt.Clear();
            descriptionTxt.SendKeys("CB");

            var amountTxt = ctx.WebDriver.FindElement(By.Id("Cost"));
            amountTxt.Clear();
            amountTxt.SendKeys("100.00");

            var expenditureTypeDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("TypeExpenditureId")));
            expenditureTypeDdl.SelectByIndex(1);

            var paymentMethodDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("Cash");

            Thread.Sleep(2000);

            var atmWithdrawDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("AtmWithdrawId")));
            atmWithdrawDdl.SelectByIndex(1);

            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
    }
}
