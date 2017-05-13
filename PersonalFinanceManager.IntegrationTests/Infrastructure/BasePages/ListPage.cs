using System;
using OpenQA.Selenium;
using System.Threading;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure.BasePages
{
    public abstract class ListPage : BasePage
    {
        private const string CreateButtonClassName = "btn_create";
        private const string EditButtonClassName = "btn_edit";
        private const string DeleteButtonClassName = "btn_delete";
        private const string ConfirmDeletionModalClassName = "modal-title";
        private const string ConfirmDeletionButtonClassName = "btn_delete_confirm";

        public abstract string RowClassName { get; }
        public abstract string DeletionConfirmationModalTitle { get; }

        protected ListPage(IWebDriver webDriver, string baseUrl) : base(webDriver, baseUrl)
        {
        }

        public void ClickAddButton()
        {
            var createBtn = WebDriver.FindElement(By.ClassName(CreateButtonClassName));
            createBtn.Click();
        }

        public void ClickEditButton(IWebElement firstRow)
        {
            var editBtn = firstRow.FindElement(By.ClassName(EditButtonClassName));
            editBtn.Click();
        }

        public void ClickDeleteButton(IWebElement firstRow)
        {
            var deleteConfirmBtn = firstRow.FindElement(By.ClassName(DeleteButtonClassName));
            deleteConfirmBtn.Click();
        }
        
        public void ClickConfirmDeletionButton()
        {
            var deleteBtn = WebDriver.FindElement(By.ClassName(ConfirmDeletionButtonClassName));
            deleteBtn.Click();
            Thread.Sleep(2000);
        }

        public void ClickCancelDeletionButton()
        {

        }

        public IWebElement FindFirstRow()
        {
            var rows = WebDriver.FindElements(By.ClassName(RowClassName));
            if (rows.Count < 1)
            {
                throw new Exception($"There is no rows matching this {RowClassName}");
            }
            return rows[0];
        }

        public void CheckDeletionConfirmationModalTitle()
        {
            var element = WebDriver.FindElementAndWaitUntilDisplayed(By.ClassName(ConfirmDeletionModalClassName), 10);
            if (element.Text != DeletionConfirmationModalTitle)
            {
                throw new Exception("The confirmation of deletion should be there.");
            }
        }
    }
}
