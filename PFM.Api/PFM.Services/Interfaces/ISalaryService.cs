using System.Collections.Generic;
using PFM.Services.DTOs.Salary;

namespace PFM.Services.Interfaces
{
    public interface ISalaryService : IBaseService
    {
        IList<SalaryList> GetSalaries(string userId);

        void CreateSalary(SalaryDetails salaryDetails);

        SalaryDetails GetById(int id);

        void EditSalary(SalaryDetails salaryDetails);

        void DeleteSalary(int id);

        void CopySalary(int sourceId);
    }
}