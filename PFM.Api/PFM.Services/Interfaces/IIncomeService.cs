using System.Collections.Generic;
using PFM.DataAccessLayer.Entities;
using PFM.DTOs.Income;
using PFM.Services.Core;

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