using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Bank;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace PersonalFinanceManager.Services
{
    public class DashboardService
    {
        ApplicationDbContext db;

        public DashboardService()
        {
            db = new ApplicationDbContext();
        }

        public List<ExpenditurePerTypeModel> GetExpendituresPerExpenditureTypes(int accountId)
        {
            var allExpenditures = db.ExpenditureModels
                .Include(u => u.Account.Currency)
                .Include(u => u.TypeExpenditure)
                .Include(u => u.PaymentMethod)
                .Where(x => x.Account.Id == accountId).ToList(); 

            var currencySymbol = allExpenditures.First().Account.Currency.Symbol;

            var expendituresByType = allExpenditures
                      .GroupBy(x => x.TypeExpenditure, x => x)
                      .ToList();

            var expendituresByTypeModel = new List<ExpenditurePerTypeModel>();
            foreach (var typeExpenditure in expendituresByType)
            {
                var getCostOverTime = GetCostOverTime(typeExpenditure.ToList(), 2);

                ViewBag.Labels = getCostOverTime.Labels;

                expendituresByTypeModel.Add(new ExpenditurePerTypeModel()
                {
                    TypeExpenditureGraphColor = typeExpenditure.Key.GraphColor,
                    TypeExpenditureName = typeExpenditure.Key.Name,
                    TwoMonthAgoCost = getCostOverTime.Values[0],
                    PreviousMonthCost = getCostOverTime.Values[1],
                    CurrentMonthCost = getCostOverTime.Values[2],
                    CurrencySymbol = currencySymbol
                });
            }

            return expendituresByTypeModel;
        }


        public void Dispose()
        {
            db.Dispose();
        }
    }
}