using log4net;
using Microsoft.AspNet.Identity;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Core.Exceptions;

namespace PersonalFinanceManager.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(BaseController));

        private readonly IBankAccountService _bankAccountService;

        public BaseController(IBankAccountService bankAccountService)
        {
            this._bankAccountService = bankAccountService;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;

            if (ex.GetType() == typeof(BusinessException))
            {
                filterContext.ExceptionHandled = true;

                var errorMessages = ((BusinessException)ex).ErrorMessages;
                foreach(var errorMessagePerField in errorMessages)
                {
                    foreach(var errorMessage in errorMessagePerField.Value)
                    {
                        ModelState.AddModelError(errorMessagePerField.Key, errorMessage);
                    }
                }

                var controller = filterContext.Controller;
                filterContext.Result = new ViewResult
                {
                    ViewData = controller.ViewData,
                    TempData = controller.TempData
                };
            }
            else
            {
                _log.Error($"\n{ex.Message}");
            }
        }

        protected string CurrentUser => User.Identity.GetUserId();

        protected bool HasAccount => _bankAccountService.GetAccountsByUser(CurrentUser).Any();

        protected int GetCurrentAccount()
        {
            if (Session["CurrentAccount"] != null)
            {
                return (int)Session["CurrentAccount"];
            }
            var firstAccount = _bankAccountService.GetAccountsByUser(CurrentUser).FirstOrDefault();

            if (firstAccount != null)
            {
                return firstAccount.Id;
            }
            throw new ArgumentException("User has no account yet");
        }

        protected void AccountBasicInfo()
        {
            int accountId = GetCurrentAccount();

            var account = _bankAccountService.GetById(accountId);
            var accountName = account.Name;
            var accountCurrentAmount = account.CurrencySymbol + account.CurrentBalance;

            ViewBag.AccountId = accountId;
            ViewBag.AccountName = accountName;
            ViewBag.CurrentAmount = accountCurrentAmount;
        }
    }
}