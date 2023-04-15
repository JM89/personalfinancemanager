using System.Collections.Generic;
using PFM.Api.Contracts.FrequenceOption;

namespace PFM.Services.Interfaces
{
    public interface IFrequenceOptionService : IBaseService
    {
        IList<FrequenceOptionList> GetFrequencyOptions();
    }
}