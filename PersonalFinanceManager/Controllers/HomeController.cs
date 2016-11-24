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

        public HomeController(IExpenditureService expenditureService, IPaymentMethodService paymentMethodService)
        {
            this._expenditureService = expenditureService;
            this._paymentMethodService = paymentMethodService;
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
            model.UserYearlyWages = 23000;
            model.FavoriteAccountCurrencySymbol = "£";
            model.FavoriteConversionRate = new ConversionRateModel() {
                BaseCurrencySymbol = "€", 
                CurrencySymbol = "£", 
                CurrencyConversionRate = 1.17M
            };
            model.FavoriteBankDetails = new Models.Bank.BankEditModel()
            {
                //FileName = "/Resources/bank_icons/bank1.jpg",
                FavoriteBranch = new Models.Bank.BankBrandEditModel()
                {
                    Name = "HIGH LORTON Branch",
                    AddressLine1 = "72 Nenthead Road",
                    AddressLine2 = "1st Floor",
                    PostCode = "A130GS",
                    City = "HIGH LORTON",
                    PhoneNumber = "077 8851 8618"
                },
                Website = "www.myfavoritebank.com",
                GeneralEnquiryPhoneNumber = "077 8851 8614"
            };
            return View(model);
        }

        public JsonResult GetDebitMovementsOverTime()
        {
            var chartData = new ChartData()
            {
                Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" }
            };
            chartData.ChartDatasets = new List<ChartDataset>()
            {
                new ChartDataset()
                {
                    Values = new List<string>() {
                        "20", "30", "40", "50", "45", "87", "65"
                    }
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