using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class IncomeController : BaseController
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._incomeService = incomeService;
        }

        /// <summary>
        /// Return the list of incomes.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            AccountBasicInfo();

            var model = _incomeService.GetIncomes(accountId).OrderByDescending(x => x.DateIncome).ThenByDescending(x => x.Id).ToList();

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
        public ActionResult Create(IncomeEditModel incomeEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;
                incomeEditModel.AccountId = accountId;

                _incomeService.CreateIncome(incomeEditModel);

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

            var incomeModel = _incomeService.GetById(id.Value);
            
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
        public ActionResult Edit(IncomeEditModel incomeEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = CurrentAccount;
                incomeEditModel.AccountId = accountId;

                _incomeService.EditIncome(incomeEditModel);
                
                return RedirectToAction("Index");
            }
            return View(incomeEditModel);
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
            _incomeService.DeleteIncome(id);

            return Content(Url.Action("Index"));
        }
    }
}
