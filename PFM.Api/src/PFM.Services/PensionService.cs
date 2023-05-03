﻿using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using PFM.Api.Contracts.Pension;
using PFM.Services.Caches.Interfaces;
using PFM.Api.Contracts.Salary;

namespace PFM.Services
{
    public class PensionService : IPensionService
    {
        private readonly IPensionRepository _pensionRepository;
        private readonly ICountryCache _countryCache;
        private readonly ICurrencyCache _currencyCache;

        public PensionService(IPensionRepository pensionRepository, ICountryCache countryCache, ICurrencyCache currencyCache)
        {
            this._pensionRepository = pensionRepository;
            this._countryCache = countryCache;
            this._currencyCache = currencyCache;
        }

        public IList<PensionList> GetPensions(string userId)
        {
            var pensions = _pensionRepository.GetList2().Where(x => x.UserId == userId).ToList();

            var mappedPensions = new List<PensionList>();
            pensions.ForEach(async p => {
                var map = Mapper.Map<PensionList>(p);
                var country = await _countryCache.GetById(p.CountryId);
                map.CountryName = country.Name;
            });

            return mappedPensions.ToList();
        }

        public void CreatePension(PensionDetails pensionDetails)
        {
            var pension = Mapper.Map<Pension>(pensionDetails);
            _pensionRepository.Create(pension);
        }

        public PensionDetails GetById(int id)
        {
            var pension = _pensionRepository.GetById(id);
            if (pension == null)
            {
                return null;
            }
            return Mapper.Map<PensionDetails>(pension);
        }

        public void EditPension(PensionDetails pensionDetails)
        {
            var pension = Mapper.Map<Pension>(pensionDetails);
            _pensionRepository.Update(pension);
        }

        public void DeletePension(int id)
        {
            var pension = _pensionRepository.GetById(id);
            
            _pensionRepository.Delete(pension);
        }
    }
}