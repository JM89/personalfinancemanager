using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Models.TaxType;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class SalaryController : BaseController
    {
        private readonly ISalaryService _salaryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;
        private readonly ITaxService _taxService;

        public SalaryController(ISalaryService salaryService, ICurrencyService currencyService, ICountryService countryService, ITaxService taxService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._salaryService = salaryService;
            this._currencyService = currencyService;
            this._countryService = countryService;
            this._taxService = taxService;
        }

        /// <summary>
        /// Return the list of salaries.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = (await _salaryService.GetSalaries(CurrentUser)).OrderByDescending(x => x.StartDate).ToList();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var salaryModel = new SalaryEditModel { StartDate = DateTime.Now, SalaryDeductions = new List<SalaryDeductionEditModel>() };
            await PopulateDropDownLists(salaryModel);
            return View(salaryModel);
        }

        /// <summary>
        /// Create a new salary.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SalaryEditModel salaryEditModel)
        {
            await PopulateDropDownLists(salaryEditModel);

            if (ModelState.IsValid)
            {
                salaryEditModel.UserId = CurrentUser;

                await _salaryService.CreateSalary(salaryEditModel);

                return RedirectToAction("Index");
            }

            return View(salaryEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Salary id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var salaryModel = await _salaryService.GetById(id.Value);
            await PopulateDropDownLists(salaryModel);

            if (salaryModel == null)
            {
                return HttpNotFound();
            }

            return View(salaryModel);
        }

        /// <summary>
        /// Update an existing salary.
        /// </summary>
        /// <param name="salaryEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SalaryEditModel salaryEditModel)
        {
            await PopulateDropDownLists(salaryEditModel);

            if (ModelState.IsValid)
            {
                salaryEditModel.UserId = CurrentUser;

                await _salaryService.EditSalary(salaryEditModel);

                return RedirectToAction("Index");
            }
            return View(salaryEditModel);
        }

        /// <summary>
        /// Delete the salary after confirmation.
        /// </summary>
        /// <param name="id">Salary id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _salaryService.DeleteSalary(id);

            return Content(Url.Action("Index"));
        }

        private async Task PopulateDropDownLists(SalaryEditModel salaryModel)
        {
            salaryModel.AvailableCurrencies = (await _currencyService.GetCurrencies()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            salaryModel.AvailableCountries = (await _countryService.GetCountries()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            salaryModel.AvailableTaxes = (await _taxService.GetTaxesByType(CurrentUser, TaxType.IncomeTax)).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Code }).ToList();
        }

        /// <summary>
        /// Duplicate the salary.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public async Task<ActionResult> Copy(int sourceId)
        {
            await _salaryService.CopySalary(sourceId);

            return RedirectToAction("Index");
        }
    }
}
