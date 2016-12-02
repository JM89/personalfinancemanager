using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class SplitByTypeModel
    {
        public int ExpenditureTypeId { get; set; }

        public string ExpenditureTypeName { get; set; }

        public string GraphColor { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal CurrentMonthCost { get; set; }

        public decimal? ExpectedCost { get; set; }

        public decimal? DifferenceCost {
            get {
                if (ExpectedCost.HasValue)
                {
                    return CurrentMonthCost - ExpectedCost;
                }
                return CurrentMonthCost;
            }
        }

        public decimal PreviousMonthCost { get; set; }

        public decimal? DifferencePreviousCost
        {
            get
            {
                if (ExpectedCost.HasValue)
                {
                    return PreviousMonthCost - ExpectedCost;
                }
                return PreviousMonthCost;
            }
        }

        public string DisplayExpectedCost => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpectedCost, this.CurrencySymbol);

        public string DisplayCurrentMonthCost => DecimalFormatHelper.GetDisplayDecimalValue(this.CurrentMonthCost, this.CurrencySymbol);

        public string DisplayDifferenceCost => DecimalFormatHelper.GetDisplayDecimalValue(this.DifferenceCost, this.CurrencySymbol);

        public string DisplayPreviousMonthCost => DecimalFormatHelper.GetDisplayDecimalValue(this.PreviousMonthCost, this.CurrencySymbol);

        public string DisplayDifferencePreviousCost => DecimalFormatHelper.GetDisplayDecimalValue(this.DifferencePreviousCost, this.CurrencySymbol);
    }
}
