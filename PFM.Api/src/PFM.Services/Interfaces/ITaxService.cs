using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Api.Contracts.Tax;

namespace PFM.Services.Interfaces
{
    public interface ITaxService : IBaseService
    {
        Task<IList<TaxList>> GetTaxes(string userId);

        Task<bool> CreateTax(TaxDetails taxDetails);

        Task<TaxDetails> GetById(int id);

        Task<bool> EditTax(TaxDetails taxDetails);

        Task<bool> DeleteTax(int id);

        Task<List<TaxList>> GetTaxesByType(string currentUser, int taxTypeId);
    }
}