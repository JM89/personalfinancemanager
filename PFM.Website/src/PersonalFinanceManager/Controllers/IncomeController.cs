using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class IncomeController : BaseController
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._incomeService = incomeService;
        }

        /// <summary>
        /// Return the list of incomes.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var accountId = await GetCurrentAccount();

            await AccountBasicInfo();

            var model = (await _incomeService.GetIncomes(accountId)).OrderByDescending(x => x.DateIncome).ThenByDescending(x => x.Id).ToList();

            return View(model);
        }
        
        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            await AccountBasicInfo();

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
        public async Task<ActionResult> Create(IncomeEditModel incomeEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = await GetCurrentAccount();
                incomeEditModel.AccountId = accountId;

                await _incomeService.CreateIncome(incomeEditModel);

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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _incomeService.DeleteIncome(id);

            return Content(Url.Action("Index"));
        }
    }
}
