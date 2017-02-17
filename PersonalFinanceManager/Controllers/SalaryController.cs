using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class SalaryController : BaseController
    {
        private readonly ISalaryService _salaryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICountryService _countryService;

        public SalaryController(ISalaryService salaryService, ICurrencyService currencyService, ICountryService countryService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._salaryService = salaryService;
            this._currencyService = currencyService;
            this._countryService = countryService;
        }

        /// <summary>
        /// Return the list of salaries.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _salaryService.GetSalaries(CurrentUser).OrderByDescending(x => x.StartDate).ToList();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var salaryModel = new SalaryEditModel { StartDate = DateTime.Now, SalaryDeductions = new List<SalaryDeductionEditModel>() };
            PopulateDropDownLists(salaryModel);
            return View(salaryModel);
        }

        /// <summary>
        /// Create a new salary.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaryEditModel salaryEditModel)
        {
            PopulateDropDownLists(salaryEditModel);

            if (ModelState.IsValid)
            {
                salaryEditModel.UserId = CurrentUser;

                _salaryService.CreateSalary(salaryEditModel);

                return RedirectToAction("Index");
            }

            return View(salaryEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Salary id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var salaryModel = _salaryService.GetById(id.Value);
            PopulateDropDownLists(salaryModel);

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
        public ActionResult Edit(SalaryEditModel salaryEditModel)
        {
            PopulateDropDownLists(salaryEditModel);

            if (ModelState.IsValid)
            {
                salaryEditModel.UserId = CurrentUser;

                _salaryService.EditSalary(salaryEditModel);

                return RedirectToAction("Index");
            }
            return View(salaryEditModel);
        }

        /// <summary>
        /// Show the details of the salary you are about to delete.
        /// </summary>
        /// <param name="id">Salary id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var salaryModel = _salaryService.GetById(id.Value);

            if (salaryModel == null)
            {
                return HttpNotFound();
            }
            return View(salaryModel);
        }

        /// <summary>
        /// Delete the salary after confirmation.
        /// </summary>
        /// <param name="id">Salary id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _salaryService.DeleteSalary(id);

            return RedirectToAction("Index");
        }

        private void PopulateDropDownLists(SalaryEditModel salaryModel)
        {
            salaryModel.AvailableCurrencies = _currencyService.GetCurrencies().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            salaryModel.AvailableCountries = _countryService.GetCountries().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Duplicate the salary.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public ActionResult Copy(int sourceId)
        {
            _salaryService.CopySalary(sourceId);

            var accountId = CurrentAccount;

            return RedirectToAction("Index", new { accountId });
        }
    }
}
