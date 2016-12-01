using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Models.ExpenditureType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class SplitByTypeDashboardModel
    {
        public bool DisplayDashboard { get; set; }

        public IList<SplitByTypeModel> SplitByTypes { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal CurrentMonthTotalCost;

        public decimal ExpectedTotalCost { get; set; }

        public decimal PreviousMonthTotalCost { get; set; }

        public decimal AverageTotalCost { get; set; }

        public string BudgetPlanName { get; set; }

        public string FirstMovementDate { get; set; }

        public string CurrentMonthName { get; set; }

        public string PreviousMonthName { get; set; }

        public SplitByTypeDashboardModel()
        {
            this.SplitByTypes = new List<SplitByTypeModel>();
            this.AverageTotalCost = 0;
            this.CurrentMonthTotalCost = 0;
            this.PreviousMonthTotalCost = 0;
            this.ExpectedTotalCost = 0;
        }
    }
}
