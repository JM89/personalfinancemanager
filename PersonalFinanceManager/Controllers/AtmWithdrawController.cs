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
using AutoMapper;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class AtmWithdrawController : BaseController
    {
        private readonly IAtmWithdrawService _atmWithdrawService;

        public AtmWithdrawController(IAtmWithdrawService atmWithdrawService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._atmWithdrawService = atmWithdrawService;
        }

        /// <summary>
        /// Return the list of ATM withdraws.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            AccountBasicInfo();

            var model = _atmWithdrawService.GetAtmWithdrawsByAccountId(accountId).OrderByDescending(x => x.DateExpenditure).ThenByDescending(x => x.Id).ToList();

            return View(model);
        }
        
        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            AccountBasicInfo();

            var atmWithdrawModel = new AtmWithdrawEditModel();

            atmWithdrawModel.DateExpenditure = DateTime.Today;

            return View(atmWithdrawModel);
        }

        /// <summary>
        /// Create a new ATM withdraw.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;
                atmWithdrawEditModel.AccountId = accountId;

                _atmWithdrawService.CreateAtmWithdraw(atmWithdrawEditModel);

                return RedirectToAction("Index");
            }

            return View(atmWithdrawEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">ATM Withdraw id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var atmWithdrawModel = _atmWithdrawService.GetById(id.Value);
            
            if (atmWithdrawModel == null)
            {
                return HttpNotFound();
            }

            return View(atmWithdrawModel);
        }

        /// <summary>
        /// Update an existing ATM withdraw.
        /// </summary>
        /// <param name="atmWithdrawEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;
                atmWithdrawEditModel.AccountId = accountId;

                _atmWithdrawService.EditAtmWithdraw(atmWithdrawEditModel);
                
                return RedirectToAction("Index");
            }
            return View(atmWithdrawEditModel);
        }

        /// <summary>
        /// Close an ATM withdraw: set current amount to 0 and don't display it anymore.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Close(int id)
        {
            _atmWithdrawService.CloseAtmWithdraw(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Show the details of the ATM withdraw you are about to delete.
        /// </summary>
        /// <param name="id">ATM withdraw id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AtmWithdrawEditModel atmWithdrawEditModel = _atmWithdrawService.GetById(id.Value);

            if (atmWithdrawEditModel == null)
            {
                return HttpNotFound();
            }
            return View(atmWithdrawEditModel);
        }

        /// <summary>
        /// Delete the ATM withdraw after confirmation.
        /// </summary>
        /// <param name="id">ATM withdraw id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _atmWithdrawService.DeleteAtmWithdraw(id);

            return RedirectToAction("Index");
        }

        public ActionResult UndoDebit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _atmWithdrawService.ChangeDebitStatus(id.Value, false);

            var accountId = CurrentAccount;

            return RedirectToAction("Index", new { accountId });
        }

        public ActionResult Debit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _atmWithdrawService.ChangeDebitStatus(id.Value, true);

            var accountId = CurrentAccount;

            return RedirectToAction("Index", new { accountId });
        }
    }
}
