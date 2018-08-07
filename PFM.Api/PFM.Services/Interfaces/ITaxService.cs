using System.Collections.Generic;
using PFM.Services.Core;
using PFM.DTOs.Tax;

namespace PFM.Services.Interfaces
{
    public interface ITaxService : IBaseService
    {
        IList<TaxList> GetTaxes(string userId);

        void CreateTax(TaxDetails taxDetails);

        TaxDetails GetById(int id);

        void EditTax(TaxDetails taxDetails);

        void DeleteTax(int id);

        IList<TaxList> GetTaxesByType(string currentUser, int taxTypeId);
    }
}