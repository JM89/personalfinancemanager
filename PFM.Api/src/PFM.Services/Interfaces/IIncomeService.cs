using PFM.Api.Contracts.Income;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IIncomeService : IBaseService
    {
        Task<bool> CreateIncomes(List<IncomeDetails> incomeDetails);

        Task<bool> CreateIncome(IncomeDetails incomeDetails);

        Task<IList<IncomeList>> GetIncomes(int accountId);

        Task<IncomeDetails> GetById(int id);

        Task<bool> DeleteIncome(int id);
    }
}