using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Models.Tax;
using System;

namespace PersonalFinanceManager.Services
{
    public class TaxService : ITaxService
    {
        public void CreateTax(TaxEditModel taxEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteTax(int id)
        {
            throw new NotImplementedException();
        }

        public void EditTax(TaxEditModel taxEditModel)
        {
            throw new NotImplementedException();
        }

        public TaxEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<TaxListModel> GetTaxes(string userId)
        {
            throw new NotImplementedException();
        }

        public IList<TaxListModel> GetTaxesByType(string currentUser, TaxType incomeTax)
        {
            throw new NotImplementedException();
        }
    }
}