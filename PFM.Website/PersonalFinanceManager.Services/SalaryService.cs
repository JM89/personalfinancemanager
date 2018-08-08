using System.Collections.Generic;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Salary;
using System;

namespace PersonalFinanceManager.Services
{
    public class SalaryService : ISalaryService
    {
        public IList<SalaryListModel> GetSalaries(string userId)
        {
            throw new NotImplementedException();
        }

        public void CreateSalary(SalaryEditModel salaryEditModel)
        {
            throw new NotImplementedException();
        }

        public void CopySalary(int sourceId)
        {
            throw new NotImplementedException();
        }

        public SalaryEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditSalary(SalaryEditModel salaryEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteSalary(int id)
        {
            throw new NotImplementedException();
        }
    }
}