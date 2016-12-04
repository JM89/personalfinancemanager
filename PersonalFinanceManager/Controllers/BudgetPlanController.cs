﻿using PersonalFinanceManager.Helpers;
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

namespace PersonalFinanceManager.Controllers
{
    public class BudgetPlanController : BaseController
    {
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IExpenditureService _expenditureService;
        private readonly IIncomeService _incomeService;
        private readonly IBudgetPlanService _budgetPlanService;
        private readonly IBankAccountService _accountService;

        public BudgetPlanController(IExpenditureTypeService expenditureTypeService, IExpenditureService expenditureService,
            IIncomeService incomeService, IBudgetPlanService budgetPlanService, IBankAccountService accountService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._accountService = accountService;
            this._expenditureService = expenditureService;
            this._expenditureTypeService = expenditureTypeService;
            this._budgetPlanService = budgetPlanService;
            this._incomeService = incomeService;
        }

        public ActionResult Index()
        {
            var model = _budgetPlanService.GetBudgetPlans(CurrentAccount);

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var budgetPlanEditModel = buildBudgetPlan();
            return View(budgetPlanEditModel);
        }


        [HttpPost]
        public ActionResult Create(BudgetPlanEditModel budgetPlanEditModel)
        {
            var result = false;
            IList<JsonError> errorMessages = null;

            if (ModelState.IsValid)
            {
                _budgetPlanService.CreateBudgetPlan(budgetPlanEditModel, CurrentAccount);
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
            var budgetPlanEditModel = buildBudgetPlan(id);
            return View(budgetPlanEditModel);
        }
        
        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Budget id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            var budgetPlanEditModel = buildBudgetPlan(id);
            return View(budgetPlanEditModel);
        }

        [HttpPost]
        public ActionResult Edit(BudgetPlanEditModel budgetPlanEditModel)
        {
            var result = false;
            IList<JsonError> errorMessages = null;

            if (ModelState.IsValid)
            {
                _budgetPlanService.EditBudgetPlan(budgetPlanEditModel, CurrentAccount);
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

        private BudgetPlanEditModel buildBudgetPlan(int? id = null)
        {
            BudgetPlanEditModel existingBudgetPlan = null;
            if (id.HasValue)
            {
                existingBudgetPlan = _budgetPlanService.GetById(id.Value);
            }

            var currencySymbol = _accountService.GetById(CurrentAccount).CurrencySymbol;

            var nextMonth = DateTime.Now.AddMonths(1);
            var firstOfNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

            BudgetPlanEditModel budgetPlanEditModel = null;

            if (existingBudgetPlan != null)
            {
                budgetPlanEditModel = new BudgetPlanEditModel()
                {
                    Id = existingBudgetPlan.Id,
                    Name = existingBudgetPlan.Name,
                    ExpenditureTypes = new List<BudgetPlanExpenditureType>(),
                    CurrencySymbol = currencySymbol,
                    StartDate = existingBudgetPlan.StartDate,
                    EndDate = existingBudgetPlan.EndDate,
                    PlannedStartDate = firstOfNextMonth
                };
            }
            else
            {
                budgetPlanEditModel = new BudgetPlanEditModel()
                {
                    ExpenditureTypes = new List<BudgetPlanExpenditureType>(),
                    CurrencySymbol = currencySymbol
                };
            }

            var expenditureTypes = _expenditureTypeService.GetExpenditureTypes();

            var lastMonth = DateTime.Today.AddMonths(-1);
            var firstDayLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
            var lastDayLastMonth = DateTime.Today.AddMonths(1).AddDays(-1);

            var expenditures = _expenditureService.GetExpenditures(new ExpenditureSearch() { AccountId=CurrentAccount });

            var nbMonthsSinceFirstExpenditures = 1;
            var firstExpenditure = expenditures.OrderBy(x => x.DateExpenditure).FirstOrDefault();
            if (firstExpenditure != null)
            {
                var now = DateTime.Now;
                var firstExpenditureDate = firstExpenditure.DateExpenditure;
                nbMonthsSinceFirstExpenditures = ((now.Year - firstExpenditureDate.Year) * 12) + now.Month - firstExpenditureDate.Month;
                if (nbMonthsSinceFirstExpenditures == 0)
                {
                    nbMonthsSinceFirstExpenditures = 1;
                }
            }

            decimal expenditurePreviousMonthValue = 0;
            decimal expenditureAverageMonthValue = 0;
            foreach (var expenditureType in expenditureTypes)
            {
                var budgetPlanExpenditureType = new BudgetPlanExpenditureType()
                {
                    CurrencySymbol = currencySymbol,
                    ExpenditureType = expenditureType
                };

                decimal previousMonthValue = 0;
                decimal averageMonthValue = 0;
                var expendituresPerType = expenditures.Where(x => x.TypeExpenditureId == expenditureType.Id);
                if (expendituresPerType.Any())
                {
                    previousMonthValue = expendituresPerType.Where(x => x.DateExpenditure >= firstDayLastMonth &&
                                                                        x.DateExpenditure <= lastDayLastMonth)
                                                            .Average(x => x.Cost);

                    averageMonthValue = expendituresPerType.Sum(x => x.Cost) / nbMonthsSinceFirstExpenditures;
                }

                budgetPlanExpenditureType.PreviousMonthValue = previousMonthValue;
                budgetPlanExpenditureType.AverageMonthValue = averageMonthValue;
                expenditurePreviousMonthValue += previousMonthValue;
                expenditureAverageMonthValue += averageMonthValue;

                decimal expectedValue = 0;
                if (existingBudgetPlan != null)
                {
                    var existingExpenditureType = existingBudgetPlan.ExpenditureTypes.SingleOrDefault(x => expenditureType.Id == x.ExpenditureType.Id);
                    if (existingExpenditureType != null)
                    {
                        expectedValue =  existingExpenditureType.ExpectedValue;
                    }
                }

                budgetPlanExpenditureType.ExpectedValue = expectedValue;

                budgetPlanEditModel.ExpenditureTypes.Add(budgetPlanExpenditureType);
            }

            budgetPlanEditModel.ExpenditurePreviousMonthValue = expenditurePreviousMonthValue;
            budgetPlanEditModel.ExpenditureAverageMonthValue = expenditureAverageMonthValue;

            var incomes = _incomeService.GetIncomes(CurrentAccount);
            decimal incomePreviousMonthValue = 0;
            decimal incomeAverageMonthValue = 0;
            if (incomes.Any())
            {
                incomePreviousMonthValue = incomes.Where(x => x.DateIncome >= firstDayLastMonth && x.DateIncome <= lastDayLastMonth)
                                                  .Sum(x => x.Cost);
                incomeAverageMonthValue = incomes.Sum(x => x.Cost) / nbMonthsSinceFirstExpenditures;
            }
            budgetPlanEditModel.IncomePreviousMonthValue = incomePreviousMonthValue;
            budgetPlanEditModel.IncomeAverageMonthValue = incomeAverageMonthValue;

            budgetPlanEditModel.TotalPreviousMonthValue = budgetPlanEditModel.IncomePreviousMonthValue - budgetPlanEditModel.ExpenditurePreviousMonthValue;
            budgetPlanEditModel.TotalAverageMonthValue = budgetPlanEditModel.IncomeAverageMonthValue - budgetPlanEditModel.ExpenditureAverageMonthValue;

            return budgetPlanEditModel;
        }

        public ActionResult StartBudgetPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _budgetPlanService.StartBudgetPlan(id.Value);

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