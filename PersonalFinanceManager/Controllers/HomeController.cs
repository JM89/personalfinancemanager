using PersonalFinanceManager.Models.Helpers.Chart;
using PersonalFinanceManager.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var fakeData = new HomePageModel();
            fakeData.TotalNumberOfDebitMovements = 12;
            fakeData.FirstMovementDate = DateTime.Now;
            fakeData.UserYearlyWages = 23000;
            fakeData.FavoriteAccountCurrencySymbol = "£";
            fakeData.FavoriteConversionRate = new ConversionRateModel() {
                BaseCurrencySymbol = "€", 
                CurrencySymbol = "£", 
                CurrencyConversionRate = 1.17M
            };
            fakeData.FavoriteBankDetails = new Models.Bank.BankEditModel()
            {
                FileName = "/Resources/bank_icons/bank1.jpg",
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
            fakeData.AmountDebitMovementPercentagePerPaymentMethods = new List<NumberOfMvtPerPaymentMethodModel>();
            fakeData.AmountDebitMovementPercentagePerPaymentMethods.Add(new NumberOfMvtPerPaymentMethodModel() {
                AmountExpendituresPercent = 10,
                PaymentMethod = new Models.PaymentMethod.PaymentMethodListModel() {
                    Name = "CB",
                    CssClass = "primary"
                }
            });
            fakeData.AmountDebitMovementPercentagePerPaymentMethods.Add(new NumberOfMvtPerPaymentMethodModel()
            {
                AmountExpendituresPercent = 30,
                PaymentMethod = new Models.PaymentMethod.PaymentMethodListModel()
                {
                    Name = "ATM Withdraw",
                    CssClass = "info"
                }
            });
            fakeData.AmountDebitMovementPercentagePerPaymentMethods.Add(new NumberOfMvtPerPaymentMethodModel()
            {
                AmountExpendituresPercent = 40,
                PaymentMethod = new Models.PaymentMethod.PaymentMethodListModel()
                {
                    Name = "Direct Debit",
                    CssClass = "success"
                }
            });
            fakeData.AmountDebitMovementPercentagePerPaymentMethods.Add(new NumberOfMvtPerPaymentMethodModel()
            {
                AmountExpendituresPercent = 5,
                PaymentMethod = new Models.PaymentMethod.PaymentMethodListModel()
                {
                    Name = "Transfer",
                    CssClass = "warning"
                }
            });
            fakeData.AmountDebitMovementPercentagePerPaymentMethods.Add(new NumberOfMvtPerPaymentMethodModel()
            {
                AmountExpendituresPercent = 15,
                PaymentMethod = new Models.PaymentMethod.PaymentMethodListModel()
                {
                    Name = "Internal Transfer",
                    CssClass = "danger"
                }
            });
            return View(fakeData);
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