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
using PersonalFinanceManager.Models.PeriodicOutcome;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PeriodicOutcomeController : BaseController
    {
        private PeriodicOutcomeService periodicOutcomeService = new PeriodicOutcomeService();
        private BankAccountService bankAccountService = new BankAccountService();
        private FrequencyService frequencyService = new FrequencyService();
        private ExpenditureTypeService expenditureTypeService = new ExpenditureTypeService();

        /// <summary>
        /// Return the list of pPeriodic outcomes.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            var model = periodicOutcomeService.GetPeriodicOutcomes(accountId);

            return View(model);
        }

        /// <summary>
        /// Populate the list of accounts, expenditure types and frequencies for the Create / Edit form. 
        /// </summary>
        /// <param name="periodicOutcomeModel"></param>
        private void PopulateDropDownLists(PeriodicOutcomeEditModel periodicOutcomeModel)
        {
            periodicOutcomeModel.AvailableAccounts = bankAccountService.GetAccountsByUser(User.Identity.GetUserId()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            periodicOutcomeModel.AvailableFrequencies = frequencyService.GetFrequencies().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            periodicOutcomeModel.AvailableExpenditureTypes = expenditureTypeService.GetExpenditureTypes().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var PeriodicOutcomeModel = new PeriodicOutcomeEditModel();

            PeriodicOutcomeModel.StartDate = DateTime.Today;

            PopulateDropDownLists(PeriodicOutcomeModel);

            return View(PeriodicOutcomeModel);
        }

        /// <summary>
        /// Create a new periodic outcome.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,AccountId,FrequencyId,Cost,StartDate,TypeExpenditureId")] PeriodicOutcomeEditModel PeriodicOutcomeEditModel)
        {
            if (ModelState.IsValid)
            {
                periodicOutcomeService.CreatePeriodicOutcome(PeriodicOutcomeEditModel);

                return RedirectToAction("Index");
            }

            PopulateDropDownLists(PeriodicOutcomeEditModel);

            return View(PeriodicOutcomeEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Periodic outcome id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var PeriodicOutcomeModel = periodicOutcomeService.GetById(id.Value);
            
            if (PeriodicOutcomeModel == null)
            {
                return HttpNotFound();
            }

            PopulateDropDownLists(PeriodicOutcomeModel);

            return View(PeriodicOutcomeModel);
        }

        public ActionResult Disable(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            periodicOutcomeService.ChangeStatus(id.Value, false);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Update an existing periodic outcome.
        /// </summary>
        /// <param name="PeriodicOutcomeEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,AccountId,FrequencyId,Cost,StartDate,TypeExpenditureId")] PeriodicOutcomeEditModel PeriodicOutcomeEditModel)
        {
            if (ModelState.IsValid)
            {
                periodicOutcomeService.EditPeriodicOutcome(PeriodicOutcomeEditModel);
                
                return RedirectToAction("Index");
            }
            return View(PeriodicOutcomeEditModel);
        }

        /// <summary>
        /// Show the details of the periodic outcome you are about to delete.
        /// </summary>
        /// <param name="id">PeriodicOutcome id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PeriodicOutcomeEditModel periodicOutcomeModel = periodicOutcomeService.GetById(id.Value);

            if (periodicOutcomeModel == null)
            {
                return HttpNotFound();
            }
            return View(periodicOutcomeModel);
        }

        /// <summary>
        /// Delete the periodic outcome after confirmation.
        /// </summary>
        /// <param name="id">PeriodicOutcome id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            periodicOutcomeService.DeletePeriodicOutcome(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                periodicOutcomeService.Dispose();
                bankAccountService.Dispose();
                frequencyService.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
