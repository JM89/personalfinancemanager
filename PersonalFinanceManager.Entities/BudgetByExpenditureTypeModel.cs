using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class BudgetByExpenditureTypeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ExpenditureTypeModel ExpenditureType { get; set; }
        public decimal Budget { get; set; }
        public decimal? Actual { get; set; }
        public AccountModel Account { get; set; }
    }
}
