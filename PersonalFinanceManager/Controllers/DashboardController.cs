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

namespace PersonalFinanceManager.Controllers
{
    public class DashboardController : BaseController
    {
        private ExpenditureService expenditureService = new ExpenditureService();
        private ExpenditureTypeService expenditureTypeService = new ExpenditureTypeService();
        private BankAccountService bankAccountService = new BankAccountService();

        // GET: Dashboard
        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            var account = bankAccountService.GetById(accountId);

            var baseStartDate = DateTime.Today.AddMonths(-11);
            var startDate = new DateTime(baseStartDate.Year, baseStartDate.Month, 1);

            var endBaseDate = DateTime.Today;
            var endDate = new DateTime(endBaseDate.Year, endBaseDate.Month, 1).AddMonths(1).AddDays(-1);

            var allExpenditures = GetExpendituresFor12Months(accountId);

            if (!allExpenditures.Any())
            {
                ViewBag.ExpendituresFound = false;
                return View(new List<ExpenditurePerTypeModel>());
            }
            else
            {
                ViewBag.ExpendituresFound = true;

                var expendituresByType = allExpenditures
                          .GroupBy(x => x.TypeExpenditure, x => x)
                          .OrderBy(x => x.Key.Name)
                          .ToList();

                var expendituresByTypeModel = new List<ExpenditurePerTypeModel>();
                foreach (var typeExpenditure in expendituresByType)
                {
                    var getCostOverTime = GetCostOverTime(typeExpenditure.ToList(), 2);

                    ViewBag.Labels = getCostOverTime.Labels;

                    expendituresByTypeModel.Add(new ExpenditurePerTypeModel()
                    {
                        TypeExpenditureGraphColor = typeExpenditure.Key.GraphColor,
                        TypeExpenditureName = typeExpenditure.Key.Name,
                        TwoMonthAgoCost = getCostOverTime.Values[0],
                        PreviousMonthCost = getCostOverTime.Values[1],
                        CurrentMonthCost = getCostOverTime.Values[2],
                        CurrencySymbol = account.CurrencySymbol
                    });
                }

                ViewBag.AccountId = accountId;
                ViewBag.AccountName = account.Name;
                ViewBag.StartDate = startDate.ToString("MMMM yyyy");
                ViewBag.EndDate = endDate.ToString("MMMM yyyy");

                return View(expendituresByTypeModel);
            }
        }

        // GET: ExpenditureModels
        public JsonResult GetExpendituresOverTime(int accountId)
        {
            var expenditureTypeColor = expenditureTypeService.GetExpenditureTypes().First().GraphColor;

            var color = System.Drawing.ColorTranslator.FromHtml("#" + expenditureTypeColor);

            string fillColor = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 0.5);
            string strokeColor = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 0.8);
            string highlightFill = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 0.75);
            string highlightStroke = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 1);

            var allExpenditures = GetExpendituresFor12Months(accountId);

            var costOverTime = GetCostOverTime(allExpenditures.ToList(), 11);

            var currencySymbol = allExpenditures.First().Account.Currency.Symbol;

            var tooltipTemplate = " <%if (label) {%><%= label %>: <%}%> " + currencySymbol + "<%= parseFloat(Math.round(value * 100) / 100).toFixed(2) %>";

            return Json(
                new
                {
                    data = new
                    {
                        labels = costOverTime.Labels,
                        datasets =
                        new[]
                        {
                            new {
                                label = "My First dataset",
                                fillColor = fillColor,
                                strokeColor = strokeColor,
                                highlightFill = highlightFill,
                                highlightStroke = highlightStroke,
                                data = costOverTime.Values
                            }
                        }
                    },
                    options = new
                    {
                        tooltipFontSize = 10,
                        tooltipTemplate = tooltipTemplate
                    }
                }, JsonRequestBehavior.AllowGet);
        }

        // GET: ExpenditureModels
        public JsonResult GetCategoryExpendituresForGivenMonth(DateTime selectedDate)
        {
            var accountId = CurrentAccount;

            var allExpenditures = expenditureService.GetExpendituresByAccountIdForDashboard(accountId, selectedDate, selectedDate.AddMonths(1));

            var totalSum = allExpenditures.Sum(x => x.Cost);

            var list = allExpenditures
                        .GroupBy(x => x.TypeExpenditure, x => x.Cost)
                        .OrderBy(x => x.Key.Name)
                        .ToList()
                        .Select(x => new
                        {
                            label = x.Key.Name,
                            color = "#" + x.Key.GraphColor,
                            value = x.Sum()
                        });

            var currency = allExpenditures.First().Account.Currency;

            var tooltipTemplate = " <%if (label) {%><%= label %>: <%}%> <%= parseFloat(Math.round((value / " + totalSum + ") * 100)).toFixed(0) %>% (" + currency.Symbol + "<%= parseFloat(Math.round(value * 100) / 100).toFixed(2) %>)";

            return Json(new { data = list, options = new
            {
                tooltipFontSize = 10,
                tooltipTemplate = tooltipTemplate } }, JsonRequestBehavior.AllowGet);
        }

        // GET: ExpenditureModels
        public JsonResult GetCategoryExpenditures()
        {
            var accountId = CurrentAccount;

            var allExpenditures = GetExpendituresFor12Months(accountId);

            var totalSum = allExpenditures.Sum(x => x.Cost);

            var list = allExpenditures
                        .GroupBy(x => x.TypeExpenditure, x => x.Cost)
                        .OrderBy(x => x.Key.Name)
                        .ToList()
                        .Select(x => new
                        {
                            label = x.Key.Name,
                            color = "#" + x.Key.GraphColor,
                            value = x.Sum() 
                        });

            var currency = allExpenditures.First().Account.Currency;

            var tooltipTemplate = " <%if (label) {%><%= label %>: <%}%> <%= parseFloat(Math.round((value / " + totalSum + ") * 100)).toFixed(0) %>% (" + currency.Symbol + "<%= parseFloat(Math.round(value * 100) / 100).toFixed(2) %>)";

            return Json(new { data = list, options = new
            {
                tooltipFontSize = 10,
                tooltipTemplate = tooltipTemplate } }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryExpendituresOverTime(string selectedExpenditureType)
        {
            var accountId = CurrentAccount;

            var expenditureTypeColor = expenditureTypeService.GetExpenditureTypes().Single(x => x.Name == selectedExpenditureType).GraphColor;

            var color = System.Drawing.ColorTranslator.FromHtml("#"+expenditureTypeColor);
            
            string fillColor = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 0.5);
            string strokeColor = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 0.8); 
            string highlightFill = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 0.75); 
            string highlightStroke = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, 1);

            var allExpenditures = GetExpendituresFor12Months(accountId);

            var expenditurePerType = allExpenditures.Where(x => x.TypeExpenditure.Name == selectedExpenditureType);

            var firstExpenditureDate = allExpenditures.OrderBy(x => x.DateExpenditure).First().DateExpenditure;
            var referenceDate = new DateTime(firstExpenditureDate.Year, firstExpenditureDate.Month, 1);

            var costOverTime = GetCostOverTime(expenditurePerType.ToList(), 11, referenceDate, true);

            var currencySymbol = allExpenditures.First().Account.Currency.Symbol;
            
            var tooltipTemplate = " <%if (label) {%><%= label %>: <%}%> " + currencySymbol + "<%= parseFloat(Math.round(value * 100) / 100).toFixed(2) %>";

            var average = costOverTime.Average.ToString("#.00", CultureInfo.InvariantCulture) + currencySymbol; 

            return Json(
                new
                {
                    data = new {
                        labels = costOverTime.Labels,
                        datasets =
                        new[]
                        {
                            new {
                                label = "My First dataset",
                                fillColor = fillColor,
                                strokeColor = strokeColor,
                                highlightFill = highlightFill,
                                highlightStroke = highlightStroke,
                                data = costOverTime.Values
                            }
                        }
                    },
                    averageInfo = new
                    {
                        average = average,
                        color = strokeColor
                    },
                    options = new
                    {
                        tooltipFontSize = 10,
                        tooltipTemplate = tooltipTemplate
                    }
                }, JsonRequestBehavior.AllowGet);
        }

        public class CostOverTime
        {
            public List<string> Labels = new List<string>();
            public List<decimal> Values = new List<decimal>();
            public decimal Average;
        }

        public class SumCostPerMonthAndYear
        {
            public int Month;
            public int Year;
            public bool Registered;
            public decimal SumCost;
        }

        public CostOverTime GetCostOverTime(IList<ExpenditureModel> allExpenditures, int numberOfMonth, DateTime? referenceDate = null, bool computeAverage = false)
        {
            var costOverTime = new CostOverTime();

            var now = DateTime.Now;
            now = now.Date.AddDays(1 - now.Day);
            var months = Enumerable.Range(0 - numberOfMonth, numberOfMonth + 1)
                .Select(x => new {
                    year = now.AddMonths(x).Year,
                    month = now.AddMonths(x).Month, 
                    day = now.AddMonths(x),
                    registered = now.AddMonths(x) >= referenceDate
                });

            var expendituresPerYearAndMonth =
                months.GroupJoin(allExpenditures,
                    m => new {
                        month = m.month,
                        year = m.year
                    },
                    expenditure => new {
                        month = expenditure.DateExpenditure.Month,
                        year = expenditure.DateExpenditure.Year
                    },
                    (p, g) => new SumCostPerMonthAndYear() {
                        Month = p.month,
                        Year = p.year,
                        Registered = p.registered,
                        SumCost = g.Sum(a => a.Cost)
                    }).ToList();

            foreach (var expenditure in expendituresPerYearAndMonth)
            {
                var label = (MonthNames)expenditure.Month + " " + expenditure.Year;
                costOverTime.Labels.Add(label);
                costOverTime.Values.Add(expenditure.SumCost);
            }

            if (computeAverage)
            {
                costOverTime.Average = expendituresPerYearAndMonth.Where(x => x.Registered).Average(x => x.SumCost);
            }

            return costOverTime;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                expenditureService.Dispose();
                expenditureTypeService.Dispose();
                bankAccountService.Dispose();
            }
            base.Dispose(disposing);
        }

        private IList<ExpenditureModel> GetExpendituresFor12Months(int accountId)
        {
            var baseStartDate = DateTime.Today.AddMonths(-11);
            var startDate = new DateTime(baseStartDate.Year, baseStartDate.Month, 1);

            var endBaseDate = DateTime.Today;
            var endDate = new DateTime(endBaseDate.Year, endBaseDate.Month, 1).AddMonths(1).AddDays(-1);

            var expenditures = expenditureService.GetExpendituresByAccountIdForDashboard(accountId, startDate, endDate);

            return expenditures;
        }
    }
}