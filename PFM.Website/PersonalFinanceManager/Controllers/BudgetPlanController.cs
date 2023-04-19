using PersonalFinanceManager.Helpers;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Models.SearchParameters;
using PFM.Utils.Helpers;

namespace PersonalFinanceManager.Controllers
{
    public class BudgetPlanController : BaseController
    {
        private readonly IBudgetPlanService _budgetPlanService;

        public BudgetPlanController(IBudgetPlanService budgetPlanService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._budgetPlanService = budgetPlanService;
        }

        public ActionResult Index()
        {
            var model = _budgetPlanService.GetBudgetPlans(GetCurrentAccount());

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var budgetPlanEditModel = _budgetPlanService.BuildBudgetPlan(GetCurrentAccount());
            return View(budgetPlanEditModel);
        }

        [HttpPost]
        public ActionResult Create(BudgetPlanEditModel budgetPlanEditModel)
        {
            var result = false;
            IList<JsonError> errorMessages = null;

            if (ModelState.IsValid)
            {
                _budgetPlanService.CreateBudgetPlan(budgetPlanEditModel, GetCurrentAccount());
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

        public ActionResult View(int? id)
        {
            var budgetPlanEditModel = _budgetPlanService.BuildBudgetPlan(GetCurrentAccount(), id);
            return View(budgetPlanEditModel);
        }
        
        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Budget id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            var budgetPlanEditModel = _budgetPlanService.BuildBudgetPlan(GetCurrentAccount(), id);
            return View(budgetPlanEditModel);
        }

        [HttpPost]
        public ActionResult Edit(BudgetPlanEditModel budgetPlanEditModel)
        {
            var result = false;
            IList<JsonError> errorMessages = null;

            if (ModelState.IsValid)
            {
                _budgetPlanService.EditBudgetPlan(budgetPlanEditModel, GetCurrentAccount());
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

        public ActionResult StartBudgetPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _budgetPlanService.StartBudgetPlan(id.Value, GetCurrentAccount());

            return RedirectToAction("View", new { id=id });
        }

        public ActionResult StopBudgetPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _budgetPlanService.StopBudgetPlan(id.Value);

            return RedirectToAction("View", new { id = id });
        }
    }
}