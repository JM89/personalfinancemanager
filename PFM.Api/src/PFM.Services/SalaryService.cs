using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.Api.Contracts.Salary;

namespace PFM.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly ISalaryDeductionRepository _salaryDeductionRepository;

        public SalaryService(ISalaryRepository salaryRepository, ISalaryDeductionRepository salaryDeductionRepository)
        {
            this._salaryRepository = salaryRepository;
            this._salaryDeductionRepository = salaryDeductionRepository;
        }

        public IList<SalaryList> GetSalaries(string userId)
        {
            var salaries = _salaryRepository.GetList2(u => u.Currency, u => u.Country).Where(x => x.UserId == userId).ToList();

            var mappedSalaries = salaries.Select(Mapper.Map<SalaryList>);

            return mappedSalaries.ToList();
        }

        public void CreateSalary(SalaryDetails salaryDetails)
        {
            var salary = Mapper.Map<Salary>(salaryDetails);
            var savedSalary = _salaryRepository.Create(salary);

            foreach (var salaryDeductionDetails in salaryDetails.SalaryDeductions)
            {
                var salaryDeduction = Mapper.Map<SalaryDeduction>(salaryDeductionDetails);
                salaryDeduction.SalaryId = savedSalary.Id;
                _salaryDeductionRepository.Create(salaryDeduction);
            }
        }

        public void CopySalary(int sourceId)
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
        }

        public SalaryDetails GetById(int id)
        {
            var salary = _salaryRepository.GetList2(u => u.Currency).SingleOrDefault(x => x.Id == id);
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

            return mappedSalary;
        }

        public void EditSalary(SalaryDetails salaryDetails)
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
        }

        public void DeleteSalary(int id)
        {
            var salaryDeductions = _salaryDeductionRepository.GetList().Where(x => x.SalaryId == id).ToList();
            foreach (var salaryDeduction in salaryDeductions)
            {
                _salaryDeductionRepository.Delete(salaryDeduction);
            }

            var salary = _salaryRepository.GetById(id);
            _salaryRepository.Delete(salary);
        }
    }
}