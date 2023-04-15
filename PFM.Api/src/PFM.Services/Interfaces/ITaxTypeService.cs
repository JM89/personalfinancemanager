using System.Collections.Generic;
using PFM.Api.Contracts.TaxType;

namespace PFM.Services.Interfaces
{
    public interface ITaxTypeService : IBaseService
    {
        IList<TaxTypeList> GetTaxTypes();
    }
}