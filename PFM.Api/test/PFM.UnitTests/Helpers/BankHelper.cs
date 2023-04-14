using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.UnitTests.Helpers
{
    public static class BankHelper
    {
        public static Bank CreateBankModel()
        {
            var entity = new Bank();
            return entity;
        }
    }
}
