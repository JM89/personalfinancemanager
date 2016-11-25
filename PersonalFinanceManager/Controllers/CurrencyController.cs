using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class CurrencyController : BaseController
    {
        private ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            this._currencyService = currencyService;
        }

        /// <summary>
        /// Return the list of currencies.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _currencyService.GetCurrencies();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a new currency.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurrencyEditModel currencyModel)
        {
            if (ModelState.IsValid)
            {
                _currencyService.CreateCurrency(currencyModel);

                return RedirectToAction("Index");
            }

            return View(currencyModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Currency id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currencyModel = _currencyService.GetById(id.Value);

            if (currencyModel == null)
            {
                return HttpNotFound();
            }

            return View(currencyModel);
        }

        /// <summary>
        /// Update an existing currency.
        /// </summary>
        /// <param name="currencyEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurrencyEditModel currencyEditModel)
        {
            if (ModelState.IsValid)
            {
                _currencyService.EditCurrency(currencyEditModel);

                return RedirectToAction("Index");
            }
            return View(currencyEditModel);
        }

        /// <summary>
        /// Show the details of the currency you are about to delete.
        /// </summary>
        /// <param name="id">Currency id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CurrencyEditModel currencyModel = _currencyService.GetById(id.Value);

            if (currencyModel == null)
            {
                return HttpNotFound();
            }
            return View(currencyModel);
        }

        /// <summary>
        /// Delete the currency after confirmation.
        /// </summary>
        /// <param name="id">Currency id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _currencyService.DeleteCurrency(id);

            return RedirectToAction("Index");
        }
    }
}
