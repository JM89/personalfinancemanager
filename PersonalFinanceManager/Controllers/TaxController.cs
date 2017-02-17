using System;
using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;
using System.Linq;
using System.Net;
using PersonalFinanceManager.Models.Tax;

namespace PersonalFinanceManager.Controllers
{
    /// <summary>
    /// Tax MVC Controller.
    /// </summary>
    [Authorize]
    public class TaxController : BaseController
    {
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;
        private readonly ITaxTypeService _taxTypeService;
        private readonly IFrequenceOptionService _frequenceOptionService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taxService"></param>
        /// <param name="currencyService"></param>
        /// <param name="countryService"></param>
        /// <param name="taxTypeService"></param>
        /// <param name="frequenceOptionService"></param>
        /// <param name="bankAccountService"></param>
        public TaxController(ITaxService taxService, ICurrencyService currencyService, ICountryService countryService, ITaxTypeService taxTypeService, IFrequenceOptionService frequenceOptionService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._countryService = countryService;
            this._taxTypeService = taxTypeService;
            this._frequenceOptionService = frequenceOptionService;
        }

        /// <summary>
        /// Return the list of Taxs.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _taxService.GetTaxes(CurrentUser).OrderByDescending(x => x.StartDate).ToList();
            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var taxModel = new TaxEditModel { StartDate = DateTime.Now };
            PopulateDropDownLists(taxModel);
            return View(taxModel);
        }

        /// <summary>
        /// Create a new Tax.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaxEditModel taxEditModel)
        {
            PopulateDropDownLists(taxEditModel);

            if (ModelState.IsValid)
            {
                taxEditModel.UserId = CurrentUser;

                _taxService.CreateTax(taxEditModel);

                return RedirectToAction("Index");
            }

            return View(taxEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Tax id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var taxModel = _taxService.GetById(id.Value);
            PopulateDropDownLists(taxModel);

            if (taxModel == null)
            {
                return HttpNotFound();
            }

            return View(taxModel);
        }

        /// <summary>
        /// Update an existing Tax.
        /// </summary>
        /// <param name="taxEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaxEditModel taxEditModel)
        {
            PopulateDropDownLists(taxEditModel);

            if (ModelState.IsValid)
            {
                taxEditModel.UserId = CurrentUser;

                _taxService.EditTax(taxEditModel);

                return RedirectToAction("Index");
            }
            return View(taxEditModel);
        }

        /// <summary>
        /// Show the details of the Tax you are about to delete.
        /// </summary>
        /// <param name="id">Tax id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var taxModel = _taxService.GetById(id.Value);

            if (taxModel == null)
            {
                return HttpNotFound();
            }
            return View(taxModel);
        }

        /// <summary>
        /// Delete the Tax after confirmation.
        /// </summary>
        /// <param name="id">Tax id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _taxService.DeleteTax(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populate the list of currencies for the Create / Edit form. 
        /// </summary>
        /// <param name="taxModel"></param>
        private void PopulateDropDownLists(TaxEditModel taxModel)
        {
            taxModel.AvailableCurrencies = _currencyService.GetCurrencies().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            taxModel.AvailableCountries = _countryService.GetCountries().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            taxModel.AvailableFrequenceOptions = _frequenceOptionService.GetFrequencyOptions().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            taxModel.AvailableTaxTypes = _taxTypeService.GetTaxTypes().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }
    }
}
