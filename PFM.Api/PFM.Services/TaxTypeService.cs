using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DTOs.TaxType;

namespace PFM.Services
{
    public class TaxTypeService : ITaxTypeService
    {
        private readonly ITaxTypeRepository _taxTypeRepository;

        public TaxTypeService(ITaxTypeRepository taxTypeRepository)
        {
            this._taxTypeRepository = taxTypeRepository;
        }

        public IList<TaxTypeList> GetTaxTypes()
        {
            var taxTypes = _taxTypeRepository.GetList().ToList();

            var mappedTaxTypes = taxTypes.Select(Mapper.Map<TaxTypeList>).ToList();

            return mappedTaxTypes;
        }
    }
}