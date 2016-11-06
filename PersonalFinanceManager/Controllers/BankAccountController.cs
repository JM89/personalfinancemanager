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

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class BankAccountController : BaseController
    {
        private BankAccountService bankAccountService = new BankAccountService();
        private CurrencyService currencyService = new CurrencyService();
        private BankService bankService = new BankService();

        /// <summary>
        /// Return the list of accounts of an authenticated user as a Json object, for the menu. 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAccounts()
        {
            var generatedAccountMenu = bankAccountService.GetAccountsByUserForMenu(User.Identity.GetUserId()).OrderBy(x => x.Name);

            return Json(generatedAccountMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return the list of accounts of an authenticated user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = bankAccountService.GetAccountsByUser(User.Identity.GetUserId()).OrderBy(x => x.Name);

            return View(model);
        }

        /// <summary>
        /// Populate the list of currencies and banks for the Create / Edit form. 
        /// </summary>
        /// <param name="accountModel"></param>
        private void PopulateDropDownLists(AccountEditModel accountModel)
        {
            accountModel.AvailableCurrencies = currencyService.GetCurrencies().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            accountModel.AvailableBanks = bankService.GetBanks().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
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
        public ActionResult Create([Bind(Include = "Id,Name,CurrencyId,BankId,InitialBalance")] AccountEditModel accountEditModel)
        {
            if (ModelState.IsValid)
            {
                bankAccountService.CreateBankAccount(accountEditModel, User.Identity.GetUserId());

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

            var accountModel = bankAccountService.GetById(id.Value);
            
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
        public ActionResult Edit([Bind(Include = "Id,Name,CurrencyId,BankId")] AccountEditModel accountEditModel)
        {
            if (ModelState.IsValid)
            {
                bankAccountService.EditBankAccount(accountEditModel, User.Identity.GetUserId());
                
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

            AccountEditModel accountModel = bankAccountService.GetById(id.Value);

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
            bankAccountService.DeleteBankAccount(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bankAccountService.Dispose();
                currencyService.Dispose();
                bankService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
