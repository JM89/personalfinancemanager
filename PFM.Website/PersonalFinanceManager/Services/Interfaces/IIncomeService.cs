using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IIncomeService : IBaseService
    {
        Task<bool> CreateIncomes(List<IncomeEditModel> incomeEditModel);

        Task<bool> CreateIncome(IncomeEditModel incomeEditModel);

        Task<IList<IncomeListModel>> GetIncomes(int accountId);

        Task<IncomeEditModel> GetById(int id);

        Task<bool> EditIncome(IncomeEditModel incomeEditModel);

        Task<bool> DeleteIncome(int id);
    }
}