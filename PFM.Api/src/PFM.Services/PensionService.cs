﻿using AutoMapper;
using PFM.Api.Contracts.Pension;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<PensionList>> GetPensions(string userId)
        {
            var pensions = _pensionRepository.GetList2().Where(x => x.UserId == userId).ToList();

            var mappedPensions = new List<PensionList>();

            foreach (var p in pensions)
            {
                var map = Mapper.Map<PensionList>(p);
                var country = await _countryCache.GetById(p.CountryId);
                map.CountryName = country.Name;
                mappedPensions.Add(map);
            }

            return mappedPensions.ToList();
        }

        public Task<bool> CreatePension(PensionDetails pensionDetails)
        {
            var pension = Mapper.Map<Pension>(pensionDetails);
            _pensionRepository.Create(pension);

            return Task.FromResult(true);
        }

        public Task<PensionDetails> GetById(int id)
        {
            var pension = _pensionRepository.GetById(id);
            if (pension == null)
            {
                return null;
            }
            return Task.FromResult(Mapper.Map<PensionDetails>(pension));
        }

        public Task<bool> EditPension(PensionDetails pensionDetails)
        {
            var pension = Mapper.Map<Pension>(pensionDetails);
            _pensionRepository.Update(pension);

            return Task.FromResult(true);
        }

        public Task<bool> DeletePension(int id)
        {
            var pension = _pensionRepository.GetById(id);
            
            _pensionRepository.Delete(pension);

            return Task.FromResult(true);
        }
    }
}