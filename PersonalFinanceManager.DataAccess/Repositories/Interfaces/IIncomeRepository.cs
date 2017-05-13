﻿using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface IIncomeRepository : IBaseRepository<IncomeModel>
    {
        int CountIncomes();

        decimal GetIncomeCost(int id);
    }
}
