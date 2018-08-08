using PersonalFinanceManager.Models.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Home
{
    public class NumberOfMvtPerPaymentMethodModel
    {
        public int AmountExpendituresPercent { get; set; }

        public decimal AmountExpenditures { get; set; }

        public PaymentMethodListModel PaymentMethod { get; set; }
    }
}
