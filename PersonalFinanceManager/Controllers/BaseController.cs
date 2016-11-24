﻿using Microsoft.AspNet.Identity;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class BaseController : Controller
    {
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
        }

        protected string CurrentUser
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        protected int CurrentAccount
        {
            get
            {
                if (Session["CurrentAccount"] != null)
                {
                    return (int)Session["CurrentAccount"];
                }
                else
                {
                    var bankAccountService = new BankAccountService();

                    var firstAccount = bankAccountService.GetAccountsByUser(CurrentUser).FirstOrDefault();

                    if (firstAccount != null)
                    {
                        return firstAccount.Id;
                    }
                    else
                    {
                        throw new Exception("User has no account yet");
                    }
                }
            }
        }

        protected void AccountBasicInfo()
        {
            int accountId = CurrentAccount;

            var bankAccountService = new BankAccountService();

            var account = bankAccountService.GetById(accountId);
            var accountName = account.Name;
            var accountCurrentAmount = account.CurrencySymbol + account.CurrentBalance;

            ViewBag.AccountId = accountId;
            ViewBag.AccountName = accountName;
            ViewBag.CurrentAmount = accountCurrentAmount;
        }
    }
}