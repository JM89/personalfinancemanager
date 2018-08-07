using PFM.DTOs.Bank;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PFM.DTOs.Home
{
    public class HomePageModel
    {
        public HomePageModel()
        {
            this.AmountDebitMovementPercentagePerPaymentMethods = new List<NumberOfMvtPerPaymentMethod>();
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
        
        public BankDetails FavoriteBankDetails { get; set; }

        public string DisplayFavoriteBankFullAddress
        {
            get
            {
                return string.Format("{0} {1} {2} {3}", FavoriteBankDetails.FavoriteBranch.AddressLine1,
                    FavoriteBankDetails.FavoriteBranch.AddressLine2, FavoriteBankDetails.FavoriteBranch.PostCode,
                    FavoriteBankDetails.FavoriteBranch.City);
            }
        }

        public ConversionRate FavoriteConversionRate { get; set; }

        public IList<NumberOfMvtPerPaymentMethod> AmountDebitMovementPercentagePerPaymentMethods { get; set; }
    }
}
