using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class SavingsPlanModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public IList<SavingsModel> Savings { get; set; }
        public AccountModel SourceAccount { get; set; }
        public AccountModel DestinationAccount { get; set; }
    }
}
