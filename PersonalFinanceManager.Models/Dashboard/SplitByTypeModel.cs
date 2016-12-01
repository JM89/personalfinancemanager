using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Models.ExpenditureType;
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

        public decimal AverageCost { get; set; }
    }
}
