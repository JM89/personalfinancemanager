using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class BankHelper
    {
        public static BankModel CreateBankModel()
        {
            var entity = new BankModel();
            return entity;
        }
    }
}
