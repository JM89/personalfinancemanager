using PersonalFinanceManager.Models.Salary;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ISalaryService : IBaseService
    {
        Task<IList<SalaryListModel>> GetSalaries(string userId);

        Task<bool> CreateSalary(SalaryEditModel salaryEditModel);

        Task<SalaryEditModel> GetById(int id);

        Task<bool> EditSalary(SalaryEditModel salaryEditModel);

        Task<bool> DeleteSalary(int id);

        Task<bool> CopySalary(int sourceId);
    }
}