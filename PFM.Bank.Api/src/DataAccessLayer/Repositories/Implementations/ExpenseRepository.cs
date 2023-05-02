﻿using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.SearchParameters;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(PFMContext db) : base(db)
        {

        }

        public List<Expense> GetByParameters(ExpenseGetListSearchParameters search)
        {
            var expenditures = GetList()
                .Where(x =>
                    (!search.AccountId.HasValue || (search.AccountId.HasValue && x.AccountId == search.AccountId.Value))
                    && (!search.StartDate.HasValue || (search.StartDate.HasValue && x.DateExpense >= search.StartDate))
                    && (!search.EndDate.HasValue || (search.EndDate.HasValue && x.DateExpense < search.EndDate))
                    && (!search.ShowOnDashboard.HasValue || (x.ExpenseType.ShowOnDashboard == search.ShowOnDashboard))
                    && (!search.ExpenseTypeId.HasValue || (search.ExpenseTypeId.HasValue && x.ExpenseTypeId == search.ExpenseTypeId.Value)))
                .Include(u => u.Account.Currency)
                .Include(u => u.ExpenseType)
                .Include(u => u.PaymentMethod).ToList();

            return expenditures;
        }
    }
}
