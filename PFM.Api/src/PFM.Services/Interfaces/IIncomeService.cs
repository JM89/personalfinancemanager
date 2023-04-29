using PFM.Api.Contracts.Income;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IIncomeService : IBaseService
    {
        Task<bool> CreateIncomes(List<IncomeDetails> incomeDetails);

        Task<bool> CreateIncome(IncomeDetails incomeDetails);

        IList<IncomeList> GetIncomes(int accountId);

        IncomeDetails GetById(int id);

        Task<bool> EditIncome(IncomeDetails incomeDetails);

        Task<bool> DeleteIncome(int id);
    }
}