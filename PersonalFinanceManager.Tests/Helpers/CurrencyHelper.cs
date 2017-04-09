using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class CurrencyHelper
    {
        public static CurrencyModel CreateCurrencyModel()
        {
            var currencyModel = new CurrencyModel();
            return currencyModel;
        }
    }
}
