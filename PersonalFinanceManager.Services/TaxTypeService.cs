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

namespace PersonalFinanceManager.Services
{
    public class TaxTypeService : ITaxTypeService
    {
        private readonly ITaxTypeRepository _taxTypeRepository;

        public TaxTypeService(ITaxTypeRepository taxTypeRepository)
        {
            this._taxTypeRepository = taxTypeRepository;
        }

        public IList<TaxTypeListModel> GetTaxTypes()
        {
            var taxTypes = _taxTypeRepository.GetList().ToList();

            var taxTypesModel = taxTypes.Select(Mapper.Map<TaxTypeListModel>).ToList();

            return taxTypesModel;
        }
    }
}