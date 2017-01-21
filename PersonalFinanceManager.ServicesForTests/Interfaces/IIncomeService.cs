﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.ServicesForTests.Interfaces
{
    public interface IIncomeService
    {
        int CountIncomes();

        decimal GetIncomeCost(int id);
    }
}
