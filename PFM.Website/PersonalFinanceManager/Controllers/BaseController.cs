using Microsoft.AspNet.Identity;
using PersonalFinanceManager.Services.Exceptions;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class BaseController : Controller
    {
        private readonly Serilog.ILogger _log;

        private readonly IBankAccountService _bankAccountService;

        public BaseController(IBankAccountService bankAccountService, Serilog.ILogger logger)
        {
            this._bankAccountService = bankAccountService;
            this._log = logger;
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