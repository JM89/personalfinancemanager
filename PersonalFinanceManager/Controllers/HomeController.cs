using Microsoft.AspNet.Identity;
using PersonalFinanceManager.Helpers;
using PersonalFinanceManager.Models.Helpers.Chart;
using PersonalFinanceManager.Models.Home;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IUserProfileService _userProfileService;

        public HomeController(IExpenditureService expenditureService, IPaymentMethodService paymentMethodService, IBankAccountService bankAccountService, IUserProfileService userProfileService)
        {
            this._expenditureService = expenditureService;
            this._paymentMethodService = paymentMethodService;
            this._bankAccountService = bankAccountService;
            this._userProfileService = userProfileService;
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login");
            }
            
            var model = new HomePageModel();

            var debitMvts = this._expenditureService.GetAll();

            if (debitMvts.Count != 0)
            {
                var sumDebitMvt = debitMvts.Sum(x => x.Cost);

                model.AmountDebitMovementPercentagePerPaymentMethods = new List<NumberOfMvtPerPaymentMethodModel>();
                var paymentMethods = this._paymentMethodService.GetPaymentMethods();

                foreach (var paymentMethod in paymentMethods)
                {
                    var costPerPaymentType = debitMvts.Where(x => x.PaymentMethodId == paymentMethod.Id).Sum(x => x.Cost);

                    var debitMvtPerPaymentType = new NumberOfMvtPerPaymentMethodModel()
                    {
                        PaymentMethod = paymentMethod,
                        AmountExpenditures = costPerPaymentType,
                        AmountExpendituresPercent = sumDebitMvt != 0 ? (int)(costPerPaymentType / sumDebitMvt * 100) : 0
                    };

                    model.AmountDebitMovementPercentagePerPaymentMethods.Add(debitMvtPerPaymentType);
                }
                model.FirstMovementDate = debitMvts.OrderBy(x => x.DateExpenditure).First().DateExpenditure;
            }

            model.TotalNumberOfDebitMovements = debitMvts.Count();

            var userProfile = _userProfileService.GetByUserId(User.Identity.GetUserId());
            if (userProfile.Id != 0)
            {
                model.UserYearlyWages = userProfile.YearlyWages;
            }

            var account = _bankAccountService.GetAccountsByUser(User.Identity.GetUserId()).SingleOrDefault(x => x.IsFavorite);
            if (account != null)
            {
                var fullAccountDetails = _bankAccountService.GetById(account.Id);
                model.FavoriteAccountCurrencySymbol = fullAccountDetails.CurrencySymbol;
                model.FavoriteConversionRate = new ConversionRateModel()
                {
                    BaseCurrencySymbol = "€",
                    CurrencySymbol = fullAccountDetails.CurrencySymbol,
                    CurrencyConversionRate = 1.17M
                };
                model.FavoriteBankDetails = new Models.Bank.BankEditModel()
                {
                    IconPath = fullAccountDetails.BankIconPath,
                    FavoriteBranch = new Models.Bank.BankBrandEditModel()
                    {
                        Name = fullAccountDetails.BankBranchName,
                        AddressLine1 = fullAccountDetails.BankBranchAddressLine1,
                        AddressLine2 = fullAccountDetails.BankBranchAddressLine2,
                        PostCode = fullAccountDetails.BankBranchPostCode,
                        City = fullAccountDetails.BankBranchCity,
                        PhoneNumber = fullAccountDetails.BankBranchPhoneNumber
                    },
                    Website = fullAccountDetails.BankWebsite,
                };
            }
            
            return View(model);
        }

        public JsonResult GetDebitMovementsOverTime()
        {
            var interval = DateTimeHelper.GetInterval(DateTime.Now, DateTimeUnitEnums.Months, 6);

            var intervalsByMonth = interval.GetIntervalsByMonth();

            var dataSetActualExpenditures = new ChartDataset();
            var expenditures = _expenditureService.GetExpenditures(interval.StartDate, interval.EndDate);
            foreach (var intervalByMonth in intervalsByMonth)
            {
                var expendituresByMonth = expenditures.Where(x => intervalByMonth.Value.IsBetween(x.DateExpenditure));
                dataSetActualExpenditures.Values.Add(((int)expendituresByMonth.Sum(x => x.Cost)).ToString());
            }

            var chartData = new ChartData()
            {
                Labels = intervalsByMonth.Keys.ToList(),
                ChartDatasets = new List<ChartDataset>()
                {
                    dataSetActualExpenditures
                }
            };
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult SaveCurrentAccount(int accountId, int indexAccountList)
        {
            Session["PreviousAccount"] = Session["CurrentAccount"];
            Session["CurrentAccount"] = accountId;
            Session["IndexAccountList"] = indexAccountList;

            if (Session["PreviousAccount"] == null || (int)Session["PreviousAccount"] == (int)Session["CurrentAccount"])
            {
                Session["ReloadPage"] = false;
            }
            else
            {
                Session["ReloadPage"] = true;
            }
            
            return Json(new { Data = new
                {
                    accountId = accountId, 
                    reloadPage = Session["ReloadPage"] 
                 }
                }, JsonRequestBehavior.AllowGet);
        }
    }
}