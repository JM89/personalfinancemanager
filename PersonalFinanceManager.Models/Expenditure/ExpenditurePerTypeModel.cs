using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Expenditure
{
    public class ExpenditurePerTypeModel
    {
        public string TypeExpenditureGraphColor { get; set; }

        public decimal CurrentMonthCost;

        public string TypeExpenditureName { get; set; }

        public string CurrencySymbol { get; set; }

        public string DisplayedCurrentMonthCost
        {
            get
            {
                return CurrencySymbol + this.CurrentMonthCost;
            }
        }

        public string DisplayedTwoMonthAgoCost
        {
            get
            {
                return CurrencySymbol + this.TwoMonthAgoCost;
            }
        }

        public string DisplayedPreviousMonthCost
        {
            get
            {
                return CurrencySymbol + this.PreviousMonthCost;
            }
        }

        public decimal TwoMonthAgoCost { get; set; }
        public decimal PreviousMonthCost { get; set; }
    }
}
