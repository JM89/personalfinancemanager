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
        
        public SalaryService(ISalaryRepository salaryRepository)
        {
            this._salaryRepository = salaryRepository;
        }

        public IList<SalaryListModel> GetSalaries(string userId)
        {
            var salaries = _salaryRepository.GetList().Include(u => u.Currency).Where(x => x.UserId == userId).ToList();

            var salariesModel = salaries.Select(Mapper.Map<SalaryListModel>);

            return salariesModel.ToList();
        }

        public void CreateSalary(SalaryEditModel salaryEditModel)
        {
            var salariesModel = Mapper.Map<SalaryModel>(salaryEditModel);
            _salaryRepository.Create(salariesModel);
        }

        public SalaryEditModel GetById(int id)
        {
            var salary = _salaryRepository.GetList().Include(u => u.Currency).SingleOrDefault(x => x.Id == id);
            if (salary == null)
            {
                return null;
            }
            return Mapper.Map<SalaryEditModel>(salary);
        }

        public void EditSalary(SalaryEditModel salaryEditModel)
        {
            var salary = Mapper.Map<SalaryModel>(salaryEditModel);
            _salaryRepository.Update(salary);
        }

        public void DeleteSalary(int id)
        {
            var salary = _salaryRepository.GetById(id);

            _salaryRepository.Delete(salary);
        }
    }
}