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
    public class SplitByTypeOverTimeModel
    {
        public string ExpenditureTypeName { get; set; }

        public string GraphColor { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal AverageCost { get; set; }

        public string DisplayAverageCost {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(AverageCost, CurrencySymbol);
            }
        }

        public decimal DifferenceCurrentPreviousCost { get; set; }

        public string DisplayDifferenceCurrentPreviousCost
        {
            get
            {
                return DecimalFormatHelper.GetDisplayDecimalValue(DifferenceCurrentPreviousCost, CurrencySymbol);
            }
        }

        public IList<SplitByTypeOverTimeValueModel> Values { get; set; }
    }
}
