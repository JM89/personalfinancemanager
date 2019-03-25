using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.DTOs.FrequenceOption;

namespace PFM.Services
{
    public class FrequenceOptionService : IFrequenceOptionService
    {
        private readonly IFrequenceOptionRepository _frequencyOptionRepository;

        public FrequenceOptionService(IFrequenceOptionRepository frequencyOptionRepository)
        {
            this._frequencyOptionRepository = frequencyOptionRepository;
        }

        public IList<FrequenceOptionList> GetFrequencyOptions()
        {
            var frequencyOptions = _frequencyOptionRepository.GetList().ToList();

            var mappedFrequencyOptions = frequencyOptions.Select(Mapper.Map<FrequenceOptionList>).ToList();

            return mappedFrequencyOptions;
        }
    }
}