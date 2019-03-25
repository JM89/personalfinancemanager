using PFM.Services.DTOs.Income;
using System.Collections.Generic;

namespace PFM.Services.Interfaces
{
    public interface IIncomeService : IBaseService
    {
        void CreateIncomes(List<IncomeDetails> incomeDetails);

        void CreateIncome(IncomeDetails incomeDetails);

        IList<IncomeList> GetIncomes(int accountId);

        IncomeDetails GetById(int id);

        void EditIncome(IncomeDetails incomeDetails);

        void DeleteIncome(int id);
    }
}