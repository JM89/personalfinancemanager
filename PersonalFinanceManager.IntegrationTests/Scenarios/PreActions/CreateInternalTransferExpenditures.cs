using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PersonalFinanceManager.IntegrationTests.Infrastructure;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.PreActions
{
    public class CreateInternalTransferExpenditures
    {
        public static void Execute(IntegrationTestContext ctx)
        {
            ctx.GotToUrl("/Expenditure/Index");

            ctx.WebDriver.FindElement(By.ClassName("btn_create")).Click();

            var descriptionTxt = ctx.WebDriver.FindElement(By.Id("Description"));
            descriptionTxt.Clear();
            descriptionTxt.SendKeys("Internal Transfer");

            var amountTxt = ctx.WebDriver.FindElement(By.Id("Cost"));
            amountTxt.Clear();
            amountTxt.SendKeys("100.00");

            var expenditureTypeDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("TypeExpenditureId")));
            expenditureTypeDdl.SelectByIndex(1);

            var paymentMethodDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("paymentMethodId")));
            paymentMethodDdl.SelectByText("Internal Transfer");

            Thread.Sleep(2000);

            var accountDdl = new SelectElement(ctx.WebDriver.FindElement(By.Id("TargetInternalAccountId")));
            accountDdl.SelectByIndex(1);

            var saveBtn = ctx.WebDriver.FindElement(By.ClassName("btn_save"));
            saveBtn.Click();

            Thread.Sleep(2000);
        }
    }
}
