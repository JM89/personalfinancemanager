using System.Collections.Generic;
using PFM.DTOs.FrequenceOption;
using PFM.Services.Core;

namespace PFM.Services.Interfaces
{
    public interface IFrequenceOptionService : IBaseService
    {
        IList<FrequenceOptionList> GetFrequencyOptions();
    }
}