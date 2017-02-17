using System;
using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;
using System.Linq;
using System.Net;
using PersonalFinanceManager.Models.Pension;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PensionController : BaseController
    {
        private readonly IPensionService _pensionService;
        private readonly ICurrencyService _currencyService;

        public PensionController(IPensionService pensionService, ICurrencyService currencyService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._pensionService = pensionService;
            this._currencyService = currencyService;
        }

        /// <summary>
        /// Return the list of pensions.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _pensionService.GetPensions(CurrentUser).OrderByDescending(x => x.StartDate).ToList();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var pensionModel = new PensionEditModel { StartDate = DateTime.Now };
            PopulateDropDownLists(pensionModel);
            return View(pensionModel);
        }

        /// <summary>
        /// Create a new pension.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PensionEditModel pensionEditModel)
        {
            PopulateDropDownLists(pensionEditModel);

            if (ModelState.IsValid)
            {
                pensionEditModel.UserId = CurrentUser;

                _pensionService.CreatePension(pensionEditModel);

                return RedirectToAction("Index");
            }

            return View(pensionEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Pension id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pensionModel = _pensionService.GetById(id.Value);
            PopulateDropDownLists(pensionModel);

            if (pensionModel == null)
            {
                return HttpNotFound();
            }

            return View(pensionModel);
        }

        /// <summary>
        /// Update an existing pension.
        /// </summary>
        /// <param name="pensionEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PensionEditModel pensionEditModel)
        {
            PopulateDropDownLists(pensionEditModel);

            if (ModelState.IsValid)
            {
                pensionEditModel.UserId = CurrentUser;

                _pensionService.EditPension(pensionEditModel);

                return RedirectToAction("Index");
            }
            return View(pensionEditModel);
        }

        /// <summary>
        /// Show the details of the pension you are about to delete.
        /// </summary>
        /// <param name="id">Pension id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pensionModel = _pensionService.GetById(id.Value);

            if (pensionModel == null)
            {
                return HttpNotFound();
            }
            return View(pensionModel);
        }

        /// <summary>
        /// Delete the pension after confirmation.
        /// </summary>
        /// <param name="id">Pension id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _pensionService.DeletePension(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populate the list of currencies for the Create / Edit form. 
        /// </summary>
        /// <param name="pensionModel"></param>
        private void PopulateDropDownLists(PensionEditModel pensionModel)
        {
            pensionModel.AvailableCurrencies = _currencyService.GetCurrencies().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }
    }
}
