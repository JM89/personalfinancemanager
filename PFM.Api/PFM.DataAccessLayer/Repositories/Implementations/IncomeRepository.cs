﻿using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class IncomeRepository : BaseRepository<Income>, IIncomeRepository
    {
        public IncomeRepository(PFMContext db) : base(db)
        {

        }

        public int CountIncomes()
        {
            return GetList().Count();
        }

        public decimal GetIncomeCost(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.Cost;
        }
    }
}
