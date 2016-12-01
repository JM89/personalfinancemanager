using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Expenditure;
using System.Globalization;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Helpers;
using PersonalFinanceManager.Models.Helpers.Chart;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;
using PersonalFinanceManager.Models.Dashboard;

namespace PersonalFinanceManager.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IBudgetPlanService _budgetPlanService;

        public DashboardController(IExpenditureService expenditureService, IExpenditureTypeService expenditureTypeService,
            IBankAccountService bankAccountService, IBudgetPlanService budgetPlanService)
        {
            this._budgetPlanService = budgetPlanService;
            this._expenditureTypeService = expenditureTypeService;
            this._expenditureService = expenditureService;
            this._bankAccountService = bankAccountService;
        }

        public ActionResult SplitByTypeDashboard()
        {
            var currentMonthInterval = new Interval(DateTime.Now.AddMonths(1), DateTimeUnitEnums.Months, 1);
            var previousMonthInterval = new Interval(DateTime.Now, DateTimeUnitEnums.Months, 1);

            var account = _bankAccountService.GetById(CurrentAccount);
            var expenditures = _expenditureService.GetExpenditures(new ExpenditureSearch() { AccountId = CurrentAccount });
            var expenditureTypesOrder = expenditures.GroupBy(x => x.TypeExpenditureId).ToDictionary(x => x.Key, y => y.Sum(z => z.Cost));
            var expenditureTypes = _expenditureTypeService.GetExpenditureTypes().Join(expenditureTypesOrder, x => x.Id, y => y.Key, (x, y) => new { x.Id, x.Name, x.GraphColor, y.Value }).OrderByDescending(x => x.Value);
            var budgetPlan = _budgetPlanService.GetCurrent(CurrentAccount);

            var splitByTypeDashboard = new SplitByTypeDashboardModel()
            {
                DisplayDashboard = expenditures.Any(),
                CurrencySymbol = account.CurrencySymbol,
                CurrentMonthName = currentMonthInterval.GetSingleMonthName(),
                PreviousMonthName = previousMonthInterval.GetSingleMonthName(),
                FirstMovementDate = DateTimeHelper.GetStringFormat(expenditures.OrderBy(x => x.DateExpenditure).First().DateExpenditure),
                BudgetPlanName = budgetPlan != null ? budgetPlan.Name : string.Empty
            };

            var expendituresByTypeModel = new List<SplitByTypeModel>();
            foreach (var expenditureType in expenditureTypes)
            {
                var splitByTypeModel = new SplitByTypeModel()
                {
                    ExpenditureTypeId = expenditureType.Id,
                    ExpenditureTypeName = expenditureType.Name,
                    GraphColor = expenditureType.GraphColor
                };

                var expendituresByType = expenditures.Where(x => x.TypeExpenditureId == expenditureType.Id);

                if (expendituresByType.Any())
                {
                    splitByTypeModel.AverageCost = expendituresByType.Average(x => x.Cost);
                    splitByTypeModel.PreviousMonthCost = expendituresByType.Where(x => previousMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);
                    splitByTypeModel.CurrentMonthCost = expendituresByType.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);

                    splitByTypeDashboard.AverageTotalCost += splitByTypeModel.AverageCost;
                    splitByTypeDashboard.PreviousMonthTotalCost += splitByTypeModel.PreviousMonthCost;
                    splitByTypeDashboard.CurrentMonthTotalCost += splitByTypeModel.CurrentMonthCost;
                }

                if (budgetPlan != null)
                {
                    var budgetPlanExpType = budgetPlan.ExpenditureTypes.SingleOrDefault(x => x.ExpenditureType.Id == expenditureType.Id);
                    if (budgetPlanExpType != null)
                    {
                        splitByTypeModel.ExpectedCost = budgetPlanExpType.ExpectedValue;
                        splitByTypeDashboard.ExpectedTotalCost += splitByTypeModel.ExpectedCost.Value;
                    }
                }

                splitByTypeDashboard.SplitByTypes.Add(splitByTypeModel);
            }

            return View("SplitByTypeDashboard", splitByTypeDashboard);
        }

        public JsonResult GetSplitByTypeOverLast12Months(int expenditureTypeId)
        {
            var oneYearInterval = new Interval(DateTime.Now.AddMonths(1), DateTimeUnitEnums.Months, 12);
            var intervalsByMonth = oneYearInterval.GetIntervalsByMonth();

            var account = _bankAccountService.GetById(CurrentAccount);

            var expenditures = _expenditureService.GetExpenditures(new ExpenditureSearch() {
                AccountId = CurrentAccount,
                StartDate = oneYearInterval.StartDate,
                EndDate = oneYearInterval.EndDate,
                ExpenditureTypeId = expenditureTypeId });

            var expenditureType = _expenditureTypeService.GetById(expenditureTypeId);

            var splitOverTime = new SplitByTypeOverTimeModel()
            {
                ExpenditureTypeName = expenditureType.Name,
                GraphColor = expenditureType.GraphColor,
                CurrencySymbol = account.CurrencySymbol,
                AverageCost = ComputeAverage(expenditures),
                DifferenceCurrentPreviousCost = ComputeDifferenceCurrentPreviousCost(expenditures),
                Values = new List<SplitByTypeOverTimeValueModel>()
            };

            foreach (var intervalByMonth in intervalsByMonth)
            {
                var expendituresInInterval = expenditures.Where(x => intervalByMonth.Value.IsBetween(x.DateExpenditure));
                splitOverTime.Values.Add(new SplitByTypeOverTimeValueModel() { MonthName = intervalByMonth.Key, Value = expendituresInInterval.Sum(x => x.Cost).ToString() });
            }

            return Json(splitOverTime, JsonRequestBehavior.AllowGet);
        }

        private decimal ComputeDifferenceCurrentPreviousCost(IList<ExpenditureListModel> expenditures)
        {
            var currentMonthInterval = new Interval(DateTime.Now.AddMonths(1), DateTimeUnitEnums.Months, 1);
            var currentMonthSum = expenditures.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);
            var previousMonthInterval = new Interval(DateTime.Now, DateTimeUnitEnums.Months, 1);
            var previousMonthSum = expenditures.Where(x => previousMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);
            return currentMonthSum - previousMonthSum;
        }

        private decimal ComputeAverage(IList<ExpenditureListModel> expenditures)
        {
            var previousMonth = DateTime.Now.AddMonths(-1);

            var first = expenditures.OrderBy(x => x.DateExpenditure).FirstOrDefault();

            if (first != null)
            {
                var interval = new Interval(first.DateExpenditure, previousMonth);

                var nbMonths = interval.Count(DateTimeUnitEnums.Months);

                if (nbMonths > 0)
                {
                    return expenditures.Where(x => interval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost) / nbMonths;
                }
            }

            return 0;
        }
    }
}