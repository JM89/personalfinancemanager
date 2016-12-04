using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class SavingsModel : PersistedEntity
    {
        public DateTime DateSavings { get; set; }
        public decimal Budget { get; set; }
        public decimal Actual { get; set; }
    }
}
