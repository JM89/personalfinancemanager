using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.TaxType;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Models.FrequenceOption;

namespace PersonalFinanceManager.Services
{
    public class FrequenceOptionService : IFrequenceOptionService
    {
        private readonly IFrequenceOptionRepository _frequencyOptionRepository;

        public FrequenceOptionService(IFrequenceOptionRepository frequencyOptionRepository)
        {
            this._frequencyOptionRepository = frequencyOptionRepository;
        }

        public IList<FrequenceOptionListModel> GetFrequencyOptions()
        {
            var frequencyOptions = _frequencyOptionRepository.GetList().ToList();

            var frequencyOptionsModel = frequencyOptions.Select(Mapper.Map<FrequenceOptionListModel>).ToList();

            return frequencyOptionsModel;
        }
    }
}