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
using PersonalFinanceManager.Models.ExpenditureType;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class ExpenditureTypeController : BaseController
    {
        private ExpenditureTypeService expenditureTypeService = new ExpenditureTypeService();

        /// <summary>
        /// Return the list of expenditure types.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = expenditureTypeService.GetExpenditureTypes();

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
        /// Create a new expenditure type.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,GraphColor,ShowOnDashboard")] ExpenditureTypeEditModel expenditureTypeModel)
        {
            if (ModelState.IsValid)
            {
                expenditureTypeService.CreateExpenditureType(expenditureTypeModel);

                return RedirectToAction("Index");
            }

            return View(expenditureTypeModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Expenditure Type id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var expenditureTypeModel = expenditureTypeService.GetById(id.Value);

            if (expenditureTypeModel == null)
            {
                return HttpNotFound();
            }
            return View(expenditureTypeModel);
        }

        /// <summary>
        /// Update an existing expenditure type.
        /// </summary>
        /// <param name="expenditureTypeEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,GraphColor,ShowOnDashboard")] ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            if (ModelState.IsValid)
            {
                expenditureTypeService.EditExpenditureType(expenditureTypeEditModel);

                return RedirectToAction("Index");
            }
            return View(expenditureTypeEditModel);
        }

        /// <summary>
        /// Show the details of the expenditure type you are about to delete.
        /// </summary>
        /// <param name="id">Expenditure type id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExpenditureTypeEditModel expenditureTypeModel = expenditureTypeService.GetById(id.Value);

            if (expenditureTypeModel == null)
            {
                return HttpNotFound();
            }
            return View(expenditureTypeModel);
        }

        /// <summary>
        /// Delete the expenditure type after confirmation.
        /// </summary>
        /// <param name="id">Expenditure type id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            expenditureTypeService.DeleteExpenditureType(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                expenditureTypeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
