using System;
using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;
using System.Linq;
using System.Net;
using PersonalFinanceManager.Models.Pension;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PensionController : BaseController
    {
        private readonly IPensionService _pensionService;
        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;

        public PensionController(IPensionService pensionService, ICurrencyService currencyService, ICountryService countryService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._pensionService = pensionService;
            this._currencyService = currencyService;
            this._countryService = countryService;
        }

        /// <summary>
        /// Return the list of pensions.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = (await _pensionService.GetPensions(CurrentUser)).OrderByDescending(x => x.StartDate).ToList();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var pensionModel = new PensionEditModel { StartDate = DateTime.Now };
            await PopulateDropDownLists(pensionModel);
            return View(pensionModel);
        }

        /// <summary>
        /// Create a new pension.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PensionEditModel pensionEditModel)
        {
            await PopulateDropDownLists(pensionEditModel);

            if (ModelState.IsValid)
            {
                pensionEditModel.UserId = CurrentUser;

                await _pensionService.CreatePension(pensionEditModel);

                return RedirectToAction("Index");
            }

            return View(pensionEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Pension id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pensionModel = await _pensionService.GetById(id.Value);
            await PopulateDropDownLists(pensionModel);

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
        public async Task<ActionResult> Edit(PensionEditModel pensionEditModel)
        {
            await PopulateDropDownLists(pensionEditModel);

            if (ModelState.IsValid)
            {
                pensionEditModel.UserId = CurrentUser;

                await _pensionService.EditPension(pensionEditModel);

                return RedirectToAction("Index");
            }
            return View(pensionEditModel);
        }

        /// <summary>
        /// Delete the pension after confirmation.
        /// </summary>
        /// <param name="id">Pension id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _pensionService.DeletePension(id);

            return Content(Url.Action("Index"));
        }

        /// <summary>
        /// Populate the list of currencies for the Create / Edit form. 
        /// </summary>
        /// <param name="pensionModel"></param>
        private async Task PopulateDropDownLists(PensionEditModel pensionModel)
        {
            pensionModel.AvailableCurrencies = (await _currencyService.GetCurrencies()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            pensionModel.AvailableCountries = (await _countryService.GetCountries()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }
    }
}
