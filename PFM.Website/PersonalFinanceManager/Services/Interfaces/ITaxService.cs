using PersonalFinanceManager.Models.Tax;
using PersonalFinanceManager.Models.TaxType;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ITaxService : IBaseService
    {
        Task<IList<TaxListModel>> GetTaxes(string userId);

        Task<bool> CreateTax(TaxEditModel taxEditModel);

        Task<TaxEditModel> GetById(int id);

        Task<bool> EditTax(TaxEditModel taxEditModel);

        Task<bool> DeleteTax(int id);

        Task<IList<TaxListModel>> GetTaxesByType(string currentUser, TaxType incomeTax);
    }
}