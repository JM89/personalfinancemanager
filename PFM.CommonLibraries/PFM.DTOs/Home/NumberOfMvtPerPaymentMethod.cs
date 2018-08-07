using PFM.DTOs.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DTOs.Home
{
    public class NumberOfMvtPerPaymentMethod
    {
        public int AmountExpensesPercent { get; set; }

        public decimal AmountExpenses { get; set; }

        public PaymentMethodList PaymentMethod { get; set; }
    }
}
