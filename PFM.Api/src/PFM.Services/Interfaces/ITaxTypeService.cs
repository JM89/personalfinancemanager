using System.Collections.Generic;
using PFM.Services.DTOs.TaxType;

namespace PFM.Services.Interfaces
{
    public interface ITaxTypeService : IBaseService
    {
        IList<TaxTypeList> GetTaxTypes();
    }
}