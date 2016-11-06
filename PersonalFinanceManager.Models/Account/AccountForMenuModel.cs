using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Account
{
    public class AccountForMenuModel
    {
        public object BankIcon { get; set; }

        public bool HasExpenditures { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
