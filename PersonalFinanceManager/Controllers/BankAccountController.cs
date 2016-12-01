using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Models.Account;
using AutoMapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class BankAccountController : BaseController
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly ICurrencyService _currencyService;
        private readonly IBankService _bankService;

        public BankAccountController(IBankAccountService bankAccountService, ICurrencyService currencyService, IBankService bankService)
        {
            this._bankAccountService = bankAccountService;
            this._currencyService = currencyService;
            this._bankService = bankService;
        }

        /// <summary>
        /// Return the list of accounts of an authenticated user as a Json object, for the menu. 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAccounts()
        {
            var accountsMenu = _bankAccountService.GetAccountsByUser(User.Identity.GetUserId());

            if (!accountsMenu.Any())
            {
                return Json(new List<AccountListModel>(), JsonRequestBehavior.AllowGet);
            }
            
            var first = accountsMenu.First(x => x.IsFavorite);
            var otherAccounts = accountsMenu.Where(x => !x.IsFavorite).OrderBy(x => x.Name);

            var generatedAccountMenu = new List<AccountListModel>();
            generatedAccountMenu.Add(first);
            generatedAccountMenu.AddRange(otherAccounts);

            return Json(generatedAccountMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return the list of accounts of an authenticated user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _bankAccountService.GetAccountsByUser(User.Identity.GetUserId()).OrderBy(x => x.Name);

            return View(model);
        }

        /// <summary>
        /// Populate the list of currencies and banks for the Create / Edit form. 
        /// </summary>
        /// <param name="accountModel"></param>
        private void PopulateDropDownLists(AccountEditModel accountModel)
        {
            accountModel.AvailableCurrencies = _currencyService.GetCurrencies().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            accountModel.AvailableBanks = _bankService.GetBanks().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var accountModel = new AccountEditModel();

            PopulateDropDownLists(accountModel);

            return View(accountModel);
        }

        /// <summary>
        /// Create a new account.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountEditModel accountEditModel)
        {
            if (ModelState.IsValid)
            {
                _bankAccountService.CreateBankAccount(accountEditModel, User.Identity.GetUserId());

                return RedirectToAction("Index");
            }

            PopulateDropDownLists(accountEditModel);

            return View(accountEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var accountModel = _bankAccountService.GetById(id.Value);
            
            if (accountModel == null)
            {
                return HttpNotFound();
            }

            PopulateDropDownLists(accountModel);

            return View(accountModel);
        }

        /// <summary>
        /// Update an existing account.
        /// </summary>
        /// <param name="accountEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountEditModel accountEditModel)
        {
            if (ModelState.IsValid)
            {
                _bankAccountService.EditBankAccount(accountEditModel, User.Identity.GetUserId());
                
                return RedirectToAction("Index");
            }
            return View(accountEditModel);
        }

        /// <summary>
        /// Show the details of the account you are about to delete.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AccountEditModel accountModel = _bankAccountService.GetById(id.Value);

            if (accountModel == null)
            {
                return HttpNotFound();
            }
            return View(accountModel);
        }

        /// <summary>
        /// Delete the account after confirmation.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _bankAccountService.DeleteBankAccount(id);

            return RedirectToAction("Index");
        }

        public ActionResult SetAsFavorite(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _bankAccountService.SetAsFavorite(id.Value);

            var accountId = CurrentAccount;

            return RedirectToAction("Index");
        }
    }
}
