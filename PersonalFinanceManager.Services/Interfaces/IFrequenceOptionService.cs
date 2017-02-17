using System.Collections.Generic;
using PersonalFinanceManager.Models.FrequenceOption;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IFrequenceOptionService : IBaseService
    {
        IList<FrequenceOptionListModel> GetFrequencyOptions();
    }
}