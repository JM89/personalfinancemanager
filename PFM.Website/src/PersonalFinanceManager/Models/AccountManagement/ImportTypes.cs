using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.AccountManagement
{
    public enum ImportTypes
    {
        [Description("Incomes")]
        Incomes,

        [Description("Expenses")]
        Expenses,

        [Description("ATM Withdraws")]
        AtmWithdraws
    }
}
