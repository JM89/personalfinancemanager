using System.Collections.Generic;
using PersonalFinanceManager.Models.TaxType;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ITaxTypeService : IBaseService
    {
        IList<TaxTypeListModel> GetTaxTypes();
    }
}