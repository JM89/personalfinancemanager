using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Pension;
using PersonalFinanceManager.Models.Salary;
using System.Data.Entity;

namespace PersonalFinanceManager.Services
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

        public IList<SalaryListModel> GetSalaries(string userId)
        {
            var salaries = _salaryRepository.GetList().Include(u => u.Currency).Include(u => u.Country).Where(x => x.UserId == userId).ToList();

            var salariesModel = salaries.Select(Mapper.Map<SalaryListModel>);

            return salariesModel.ToList();
        }

        public void CreateSalary(SalaryEditModel salaryEditModel)
        {
            var salaryModel = Mapper.Map<SalaryModel>(salaryEditModel);
            var savedSalary = _salaryRepository.Create(salaryModel);

            foreach (var salaryDeductionEditModel in salaryEditModel.SalaryDeductions)
            {
                var salaryDeductionModel = Mapper.Map<SalaryDeductionModel>(salaryDeductionEditModel);
                salaryDeductionModel.SalaryId = savedSalary.Id;
                _salaryDeductionRepository.Create(salaryDeductionModel);
            }
        }

        public void CopySalary(int sourceId)
        {
            var sourceSalary = _salaryRepository.GetById(sourceId, true);
            var copySalary = Mapper.Map<SalaryModel>(sourceSalary);
            copySalary.Description = "Copy " + copySalary.Description;
            copySalary = _salaryRepository.Create(copySalary);

            var sourceSalaryDeductions = _salaryDeductionRepository.GetList().AsNoTracking().Where(x => x.SalaryId == sourceId).ToList();
            foreach (var sourceSalaryDeduction in sourceSalaryDeductions)
            {
                var copySalaryDeduction = Mapper.Map<SalaryDeductionModel>(sourceSalaryDeduction);
                copySalaryDeduction.SalaryId = copySalary.Id;
                _salaryDeductionRepository.Create(copySalaryDeduction);
            }
        }

        public SalaryEditModel GetById(int id)
        {
            var salary = _salaryRepository.GetList().Include(u => u.Currency).SingleOrDefault(x => x.Id == id);
            if (salary == null)
            {
                return null;
            }
            var mappedSalary = Mapper.Map<SalaryEditModel>(salary);

            mappedSalary.SalaryDeductions = new List<SalaryDeductionEditModel>();
            var salaryDeductions = _salaryDeductionRepository.GetList().Where(x => x.SalaryId == id).ToList();
            foreach (var salaryDeduction in salaryDeductions)
            {
                var mappedSalaryDeduction = Mapper.Map<SalaryDeductionEditModel>(salaryDeduction);
                mappedSalary.SalaryDeductions.Add(mappedSalaryDeduction);
            }

            return mappedSalary;
        }

        public void EditSalary(SalaryEditModel salaryEditModel)
        {
            var salary = Mapper.Map<SalaryModel>(salaryEditModel);
            _salaryRepository.Update(salary);

            var updatedSalaryDeductionsIds = salaryEditModel.SalaryDeductions.Where(x => x.Id != 0).Select(x => x.Id);
            var existingSalaryDeductionsIds = _salaryDeductionRepository.GetList().AsNoTracking().Where(x => x.SalaryId == salaryEditModel.Id).ToList();

            // Create
            var newSalaryDeductions = salaryEditModel.SalaryDeductions.Where(x => x.Id == 0);
            foreach (var newSalaryDeduction in newSalaryDeductions)
            {
                var updatedSalaryDeductionEntity = Mapper.Map<SalaryDeductionModel>(newSalaryDeduction);
                updatedSalaryDeductionEntity.SalaryId = salaryEditModel.Id;
                _salaryDeductionRepository.Create(updatedSalaryDeductionEntity);
            }

            // Update
            var updatedSalaryDeductions = salaryEditModel.SalaryDeductions.Where(x => x.Id != 0);
            foreach (var updatedSalaryDeduction in updatedSalaryDeductions)
            {
                var updatedSalaryDeductionEntity = Mapper.Map<SalaryDeductionModel>(updatedSalaryDeduction);
                updatedSalaryDeductionEntity.SalaryId = salaryEditModel.Id;
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