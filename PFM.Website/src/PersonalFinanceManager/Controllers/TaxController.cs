using PersonalFinanceManager.Models.Tax;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public TaxController(ITaxService taxService, ICurrencyService currencyService, ICountryService countryService, ITaxTypeService taxTypeService, IFrequenceOptionService frequenceOptionService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
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
        public async Task<ActionResult> Index()
        {
            var model = (await _taxService.GetTaxes(CurrentUser)).OrderByDescending(x => x.StartDate).ToList();
            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var taxModel = new TaxEditModel { StartDate = DateTime.Now };
            await PopulateDropDownLists(taxModel);
            return View(taxModel);
        }

        /// <summary>
        /// Create a new Tax.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaxEditModel taxEditModel)
        {
            await PopulateDropDownLists(taxEditModel);

            if (ModelState.IsValid)
            {
                taxEditModel.UserId = CurrentUser;

                await _taxService.CreateTax(taxEditModel);

                return RedirectToAction("Index");
            }

            return View(taxEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Tax id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var taxModel = await _taxService.GetById(id.Value);
            await PopulateDropDownLists(taxModel);

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
        public async Task<ActionResult> Edit(TaxEditModel taxEditModel)
        {
            await PopulateDropDownLists(taxEditModel);

            if (ModelState.IsValid)
            {
                taxEditModel.UserId = CurrentUser;

                await _taxService.EditTax(taxEditModel);

                return RedirectToAction("Index");
            }
            return View(taxEditModel);
        }

        /// <summary>
        /// Delete the Tax after confirmation.
        /// </summary>
        /// <param name="id">Tax id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _taxService.DeleteTax(id);

            return Content(Url.Action("Index"));
        }

        /// <summary>
        /// Populate the list of currencies for the Create / Edit form. 
        /// </summary>
        /// <param name="taxModel"></param>
        private async Task PopulateDropDownLists(TaxEditModel taxModel)
        {
            taxModel.AvailableCurrencies = (await _currencyService.GetCurrencies()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            taxModel.AvailableCountries = (await _countryService.GetCountries()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            taxModel.AvailableFrequenceOptions = (await _frequenceOptionService.GetFrequencyOptions()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            taxModel.AvailableTaxTypes = (await _taxTypeService.GetTaxTypes()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }
    }
}
