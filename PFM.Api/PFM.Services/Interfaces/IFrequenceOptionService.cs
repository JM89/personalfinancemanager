using System.Collections.Generic;
using PFM.Services.DTOs.FrequenceOption;

namespace PFM.Services.Interfaces
{
    public interface IFrequenceOptionService : IBaseService
    {
        IList<FrequenceOptionList> GetFrequencyOptions();
    }
}