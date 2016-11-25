using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Entities;
using Microsoft.AspNet.Identity;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class ExpenditureController : BaseController
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IAtmWithdrawService _atmWithdrawService;

        public ExpenditureController(IExpenditureService expenditureService, IExpenditureTypeService expenditureTypeService, IBankAccountService bankAccountService,
            IPaymentMethodService paymentMethodService, IAtmWithdrawService atmWithdrawService)
        {
            this._bankAccountService = bankAccountService;
            this._expenditureService = expenditureService;
            this._expenditureTypeService = expenditureTypeService;
            this._paymentMethodService = paymentMethodService;
            this._atmWithdrawService = atmWithdrawService;
        }

        // GET: ExpenditureModels
        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            AccountBasicInfo();

            var expenditures = _expenditureService.GetExpendituresByAccountId2(accountId)
                .OrderByDescending(x => x.DateExpenditure)
                .ThenByDescending(x => x.Id)
                .ToList();

            return View(expenditures);
        }

        private void PopulateDropDownLists(ExpenditureEditModel expenditureModel)
        {
            //expenditureModel.AvailableAccounts = bankAccountService.GetAccountsByUser(CurrentUser).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            expenditureModel.AvailableInternalAccounts = _bankAccountService.GetAccountsByUser(CurrentUser).Where(x => x.Id != CurrentAccount).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            expenditureModel.AvailableExpenditureTypes = _expenditureTypeService.GetExpenditureTypes().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList();
            expenditureModel.AvailablePaymentMethods = _paymentMethodService.GetPaymentMethods().ToList();
            expenditureModel.AvailableAtmWithdraws = _atmWithdrawService.GetAtmWithdrawsByAccountId(CurrentAccount).Where(x => !x.IsClosed).OrderBy(x => x.DateExpenditure).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Description }).ToList();
        }

        // GET: ExpenditureModels/Create
        public ActionResult Create()
        {
            AccountBasicInfo();

            ExpenditureEditModel expenditureModel = new ExpenditureEditModel();
            expenditureModel.DateExpenditure = DateTime.Today;
            PopulateDropDownLists(expenditureModel);

            return View(expenditureModel);
        }

        // POST: ExpenditureModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenditureEditModel expenditureModel, bool stayHere)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;

                expenditureModel.AccountId = accountId;
                _expenditureService.CreateExpenditure(expenditureModel);

                if (stayHere)
                {
                    return RedirectToAction("Create", new { accountId });
                }

                return RedirectToAction("Index", new { accountId });
            }
            else
            {
                PopulateDropDownLists(expenditureModel);
            }

            return View(expenditureModel);
        }

        // GET: BankAccount/Edit/5
        public ActionResult Edit(int? id)
        {
            var accountId = CurrentAccount;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var accountName = _bankAccountService.GetById(accountId).Name;

            AccountBasicInfo();

            var expenditureModel = _expenditureService.GetById(id.Value);

            if (expenditureModel == null)
            {
                return HttpNotFound();
            }

            PopulateDropDownLists(expenditureModel);

            return View(expenditureModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpenditureEditModel expenditureModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;

                expenditureModel.AccountId = accountId;

                _expenditureService.EditExpenditure(expenditureModel);

                return RedirectToAction("Index", new { accountId });
            }
            return View(expenditureModel);
        }

        public ActionResult UndoDebit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _expenditureService.ChangeDebitStatus(id.Value, false);

            var accountId = CurrentAccount;

            return RedirectToAction("Index", new { accountId });
        }

        public ActionResult Debit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _expenditureService.ChangeDebitStatus(id.Value, true);

            var accountId = CurrentAccount;

            return RedirectToAction("Index", new { accountId });
        }


        // GET: ExpenditureModels/Delete/5
        public ActionResult Delete(int? id)
        {
            AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExpenditureEditModel expenditureModel = _expenditureService.GetById(id.Value);

            if (expenditureModel == null)
            {
                return HttpNotFound();
            }
            return View(expenditureModel);
        }

        // POST: ExpenditureModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _expenditureService.DeleteExpenditure(id);

            var accountId = CurrentAccount;

            return RedirectToAction("Index", new { accountId });
        }
    }
}
