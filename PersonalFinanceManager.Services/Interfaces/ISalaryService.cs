using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ISalaryService : IBaseService
    {
        IList<SalaryListModel> GetSalaries(string userId);

        void CreateSalary(SalaryEditModel salaryEditModel);

        SalaryEditModel GetById(int id);

        void EditSalary(SalaryEditModel salaryEditModel);

        void DeleteSalary(int id);
    }
}