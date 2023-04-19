using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.FrequenceOption;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IFrequenceOptionService : IBaseService
    {
        Task<IList<FrequenceOptionListModel>> GetFrequencyOptions();
    }
}