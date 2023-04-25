using PersonalFinanceManager.Helpers;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class BudgetPlanController : BaseController
    {
        private readonly IBudgetPlanService _budgetPlanService;

        public BudgetPlanController(IBudgetPlanService budgetPlanService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._budgetPlanService = budgetPlanService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _budgetPlanService.GetBudgetPlans(await GetCurrentAccount());

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var budgetPlanEditModel = await _budgetPlanService.BuildBudgetPlan(await GetCurrentAccount());
            return View(budgetPlanEditModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BudgetPlanEditModel budgetPlanEditModel)
        {
            var result = false;
            IList<JsonError> errorMessages = null;

            if (ModelState.IsValid)
            {
                await _budgetPlanService.CreateBudgetPlan(budgetPlanEditModel, await GetCurrentAccount());
                result = true;
            }
            else
            {
                errorMessages = ModelStateJsonConvertor.Convert(ModelState);
            }
            
            var model = new {
                Result = result,
                RedirectLocation = "/BudgetPlan/Index",
                ErrorMessages = errorMessages
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> View(int? id)
        {
            var budgetPlanEditModel = await _budgetPlanService.BuildBudgetPlan(await GetCurrentAccount(), id);
            return View(budgetPlanEditModel);
        }
        
        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Budget id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            var budgetPlanEditModel = await _budgetPlanService.BuildBudgetPlan(await GetCurrentAccount(), id);
            return View(budgetPlanEditModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(BudgetPlanEditModel budgetPlanEditModel)
        {
            var result = false;
            IList<JsonError> errorMessages = null;

            if (ModelState.IsValid)
            {
                await _budgetPlanService.EditBudgetPlan(budgetPlanEditModel, await GetCurrentAccount());
                result = true;
            }
            else
            {
                errorMessages = ModelStateJsonConvertor.Convert(ModelState);
            }

            var model = new
            {
                Result = result,
                RedirectLocation = "/BudgetPlan/Index",
                ErrorMessages = errorMessages
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> StartBudgetPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _budgetPlanService.StartBudgetPlan(id.Value, await GetCurrentAccount());

            return RedirectToAction("View", new { id=id });
        }

        public async Task<ActionResult> StopBudgetPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _budgetPlanService.StopBudgetPlan(id.Value);

            return RedirectToAction("View", new { id = id });
        }
    }
}