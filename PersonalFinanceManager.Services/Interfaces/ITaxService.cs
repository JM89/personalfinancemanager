using System.Collections.Generic;
using PersonalFinanceManager.Models.Tax;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ITaxService : IBaseService
    {
        IList<TaxListModel> GetTaxes(string userId);

        void CreateTax(TaxEditModel taxEditModel);

        TaxEditModel GetById(int id);

        void EditTax(TaxEditModel taxEditModel);

        void DeleteTax(int id);
    }
}