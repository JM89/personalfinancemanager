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
using PersonalFinanceManager.Models.Country;
using AutoMapper;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class CountryController : BaseController
    {
        private CountryService countryService = new CountryService();

        /// <summary>
        /// Return the list of countries.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = countryService.GetCountries();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var countryEditModel = new CountryEditModel();

            return View(countryEditModel);
        }

        /// <summary>
        /// Create a new country.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] CountryEditModel countryEditModel)
        {
            if (ModelState.IsValid)
            {
                countryService.CreateCountry(countryEditModel);

                return RedirectToAction("Index");
            }

            return View(countryEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Country id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var countryModel = countryService.GetById(id.Value);
            
            if (countryModel == null)
            {
                return HttpNotFound();
            }

            return View(countryModel);
        }

        /// <summary>
        /// Update an existing country.
        /// </summary>
        /// <param name="countryEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] CountryEditModel countryEditModel)
        {
            if (ModelState.IsValid)
            {
                countryService.EditCountry(countryEditModel);
                
                return RedirectToAction("Index");
            }
            return View(countryEditModel);
        }

        /// <summary>
        /// Show the details of the country you are about to delete.
        /// </summary>
        /// <param name="id">Country id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CountryEditModel countryModel = countryService.GetById(id.Value);

            if (countryModel == null)
            {
                return HttpNotFound();
            }
            return View(countryModel);
        }

        /// <summary>
        /// Delete the country after confirmation.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            countryService.DeleteCountry(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                countryService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
