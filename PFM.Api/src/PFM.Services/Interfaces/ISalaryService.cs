using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Api.Contracts.Salary;

namespace PFM.Services.Interfaces
{
    public interface ISalaryService : IBaseService
    {
        Task<IList<SalaryList>> GetSalaries(string userId);

        Task<bool> CreateSalary(SalaryDetails salaryDetails);

        Task<SalaryDetails> GetById(int id);

        Task<bool> EditSalary(SalaryDetails salaryDetails);

        Task<bool> DeleteSalary(int id);

        Task<bool> CopySalary(int sourceId);
    }
}