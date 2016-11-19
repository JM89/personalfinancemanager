using PersonalFinanceManager.Models.ExpenditureType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class PlannedExpenditure
    {
        public int ExpenditureId;

        public DateTime StartDate;

        public DateTime EndDate;

        public decimal ExpectedValue;
    }
}
