using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DTOs.AccountManagement
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
