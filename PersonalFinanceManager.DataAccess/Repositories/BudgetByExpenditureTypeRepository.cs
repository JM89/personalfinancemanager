﻿using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class BudgetByExpenditureTypeRepository : BaseRepository<BudgetByExpenditureTypeModel>, IBudgetByExpenditureTypeRepository
    {
        public BudgetByExpenditureTypeRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
