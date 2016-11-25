using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Models.Helpers.Chart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Home
{
    public class HomePageModel
    {
        public HomePageModel()
        {
            this.AmountDebitMovementPercentagePerPaymentMethods = new List<NumberOfMvtPerPaymentMethodModel>();
        }

        public int TotalNumberOfDebitMovements { get; set; }

        public DateTime? FirstMovementDate { get; set; }

        public string DisplayFirstMovementDate {
            get
            {
                return FirstMovementDate == null ? "-" : FirstMovementDate.Value.ToString("dd/MM/yyyy");
            }
        }

        public string FavoriteAccountCurrencySymbol { get; set; }

        public decimal UserYearlyWages { get; set; }
      
        public string DisplayUserYearlyIncome { get {
                return FavoriteAccountCurrencySymbol + UserYearlyWages.ToString(CultureInfo.CurrentCulture);
            }
        }
        
        public BankEditModel FavoriteBankDetails { get; set; }

        public string DisplayFavoriteBankFullAddress
        {
            get
            {
                return $"{FavoriteBankDetails.FavoriteBranch.AddressLine1} {FavoriteBankDetails.FavoriteBranch.AddressLine2}"
                    + $"{FavoriteBankDetails.FavoriteBranch.PostCode} {FavoriteBankDetails.FavoriteBranch.City}";
            }
        }

        public ConversionRateModel FavoriteConversionRate { get; set; }

        public IList<NumberOfMvtPerPaymentMethodModel> AmountDebitMovementPercentagePerPaymentMethods { get; set; }
    }
}
