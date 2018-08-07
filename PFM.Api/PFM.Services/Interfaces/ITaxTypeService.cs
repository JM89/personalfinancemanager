using System.Collections.Generic;
using PFM.DTOs.TaxType;
using PFM.Services.Core;

namespace PFM.Services.Interfaces
{
    public interface ITaxTypeService : IBaseService
    {
        IList<TaxTypeList> GetTaxTypes();
    }
}