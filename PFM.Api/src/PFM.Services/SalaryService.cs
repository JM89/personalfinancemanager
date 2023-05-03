using AutoMapper;
using PFM.Api.Contracts.Salary;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly ISalaryDeductionRepository _salaryDeductionRepository;
        private readonly ICountryCache _countryCache;
        private readonly ICurrencyCache _currencyCache;

        public SalaryService(ISalaryRepository salaryRepository, ISalaryDeductionRepository salaryDeductionRepository, ICountryCache countryCache, ICurrencyCache currencyCache)
        {
            this._salaryRepository = salaryRepository;
            this._salaryDeductionRepository = salaryDeductionRepository;
            this._countryCache = countryCache;
            this._currencyCache = currencyCache;
        }

        public async Task<IList<SalaryList>> GetSalaries(string userId)
        {
            var salaries = _salaryRepository.GetList2().Where(x => x.UserId == userId).ToList();

            var mappedSalaries = new List<SalaryList>();
            foreach (var s in salaries)
            {
                var map = Mapper.Map<SalaryList>(s);
                var country = await _countryCache.GetById(s.CountryId);
                map.CountryName = country.Name;
                var currency = await _currencyCache.GetById(s.CurrencyId);
                map.CurrencySymbol = currency.Symbol;

                mappedSalaries.Add(map);
            }

            return mappedSalaries.ToList();
        }

        public Task<bool> CreateSalary(SalaryDetails salaryDetails)
        {
            var salary = Mapper.Map<Salary>(salaryDetails);
            var savedSalary = _salaryRepository.Create(salary);

            foreach (var salaryDeductionDetails in salaryDetails.SalaryDeductions)
            {
                var salaryDeduction = Mapper.Map<SalaryDeduction>(salaryDeductionDetails);
                salaryDeduction.SalaryId = savedSalary.Id;
                _salaryDeductionRepository.Create(salaryDeduction);
            }

            return Task.FromResult(true);
        }

        public Task<bool> CopySalary(int sourceId)
        {
            var sourceSalary = _salaryRepository.GetById(sourceId, true);
            var copySalary = Mapper.Map<Salary>(sourceSalary);
            copySalary.Description = "Copy " + copySalary.Description;
            copySalary = _salaryRepository.Create(copySalary);

            var sourceSalaryDeductions = _salaryDeductionRepository.GetListAsNoTracking().Where(x => x.SalaryId == sourceId).ToList();
            foreach (var sourceSalaryDeduction in sourceSalaryDeductions)
            {
                var copySalaryDeduction = Mapper.Map<SalaryDeduction>(sourceSalaryDeduction);
                copySalaryDeduction.SalaryId = copySalary.Id;
                _salaryDeductionRepository.Create(copySalaryDeduction);
            }

            return Task.FromResult(true);
        }

        public Task<SalaryDetails> GetById(int id)
        {
            var salary = _salaryRepository.GetList2().SingleOrDefault(x => x.Id == id);
            if (salary == null)
            {
                return null;
            }
            var mappedSalary = Mapper.Map<SalaryDetails>(salary);

            mappedSalary.SalaryDeductions = new List<SalaryDeductionDetails>();
            var salaryDeductions = _salaryDeductionRepository.GetList().Where(x => x.SalaryId == id).ToList();
            foreach (var salaryDeduction in salaryDeductions)
            {
                var mappedSalaryDeduction = Mapper.Map<SalaryDeductionDetails>(salaryDeduction);
                mappedSalary.SalaryDeductions.Add(mappedSalaryDeduction);
            }

            return Task.FromResult(mappedSalary);
        }

        public Task<bool> EditSalary(SalaryDetails salaryDetails)
        {
            var salary = Mapper.Map<Salary>(salaryDetails);
            _salaryRepository.Update(salary);

            var updatedSalaryDeductionsIds = salaryDetails.SalaryDeductions.Where(x => x.Id != 0).Select(x => x.Id);
            var existingSalaryDeductionsIds = _salaryDeductionRepository.GetListAsNoTracking().Where(x => x.SalaryId == salaryDetails.Id).ToList();

            // Create
            var newSalaryDeductions = salaryDetails.SalaryDeductions.Where(x => x.Id == 0);
            foreach (var newSalaryDeduction in newSalaryDeductions)
            {
                var updatedSalaryDeductionEntity = Mapper.Map<SalaryDeduction>(newSalaryDeduction);
                updatedSalaryDeductionEntity.SalaryId = salaryDetails.Id;
                _salaryDeductionRepository.Create(updatedSalaryDeductionEntity);
            }

            // Update
            var updatedSalaryDeductions = salaryDetails.SalaryDeductions.Where(x => x.Id != 0);
            foreach (var updatedSalaryDeduction in updatedSalaryDeductions)
            {
                var updatedSalaryDeductionEntity = Mapper.Map<SalaryDeduction>(updatedSalaryDeduction);
                updatedSalaryDeductionEntity.SalaryId = salaryDetails.Id;
                _salaryDeductionRepository.Update(updatedSalaryDeductionEntity);
            }

            // Delete
            var excludedSalaryDeductions = existingSalaryDeductionsIds.Where(x => !updatedSalaryDeductionsIds.Contains(x.Id)).ToList();
            foreach (var excludedSalaryDeduction in excludedSalaryDeductions)
            {
                var refreshedSalaryDeduction = _salaryDeductionRepository.GetById(excludedSalaryDeduction.Id);
                _salaryDeductionRepository.Delete(refreshedSalaryDeduction);
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeleteSalary(int id)
        {
            var salaryDeductions = _salaryDeductionRepository.GetList().Where(x => x.SalaryId == id).ToList();
            foreach (var salaryDeduction in salaryDeductions)
            {
                _salaryDeductionRepository.Delete(salaryDeduction);
            }

            var salary = _salaryRepository.GetById(id);
            _salaryRepository.Delete(salary);

            return Task.FromResult(true);
        }
    }
}