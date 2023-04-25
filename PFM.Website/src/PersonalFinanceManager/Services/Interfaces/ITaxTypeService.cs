using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.TaxType;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ITaxTypeService : IBaseService
    {
        Task<IList<TaxTypeListModel>> GetTaxTypes();
    }
}