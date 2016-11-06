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

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class IncomeController : BaseController
    {
        private IncomeService incomeService = new IncomeService();

        /// <summary>
        /// Return the list of incomes.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            AccountBasicInfo();

            var model = incomeService.GetIncomes(accountId).OrderByDescending(x => x.DateIncome).ThenByDescending(x => x.Id).ToList();

            return View(model);
        }
        
        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            AccountBasicInfo();

            var incomeModel = new IncomeEditModel();

            incomeModel.DateIncome = DateTime.Today;

            return View(incomeModel);
        }

        /// <summary>
        /// Create a new income.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Cost,DateIncome")] IncomeEditModel incomeEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;
                incomeEditModel.AccountId = accountId;

                incomeService.CreateIncome(incomeEditModel);

                return RedirectToAction("Index");
            }

            return View(incomeEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Income id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var incomeModel = incomeService.GetById(id.Value);
            
            if (incomeModel == null)
            {
                return HttpNotFound();
            }

            return View(incomeModel);
        }

        /// <summary>
        /// Update an existing income.
        /// </summary>
        /// <param name="incomeEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Cost,DateIncome")] IncomeEditModel incomeEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;
                incomeEditModel.AccountId = accountId;

                incomeService.EditIncome(incomeEditModel);
                
                return RedirectToAction("Index");
            }
            return View(incomeEditModel);
        }

        /// <summary>
        /// Show the details of the income you are about to delete.
        /// </summary>
        /// <param name="id">Income id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IncomeEditModel incomeModel = incomeService.GetById(id.Value);

            if (incomeModel == null)
            {
                return HttpNotFound();
            }
            return View(incomeModel);
        }

        /// <summary>
        /// Delete the income after confirmation.
        /// </summary>
        /// <param name="id">Income id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            incomeService.DeleteIncome(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                incomeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
