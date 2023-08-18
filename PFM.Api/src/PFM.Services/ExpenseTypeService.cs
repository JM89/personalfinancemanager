using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Api.Contracts.ExpenseType;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly IExpenseRepository _expenditureRepository;

        public ExpenseTypeService(IExpenseTypeRepository expenditureTypeRepository, IExpenseRepository expenseTypeRepository)
        {
            this._expenseTypeRepository = expenditureTypeRepository;
            this._expenditureRepository = expenseTypeRepository;
        }

        public async Task<IList<ExpenseTypeList>> GetExpenseTypes(string userId)
        {
            var expenses = _expenditureRepository.GetList();

            var expenseTypes = _expenseTypeRepository
                .GetList()
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedExpenditureTypes = expenseTypes.Select(x => Mapper.Map<ExpenseTypeList>(x)).ToList();

            mappedExpenditureTypes.ForEach(expenditureType =>
            {
                expenditureType.CanBeDeleted = !expenses.Any(x => x.ExpenseTypeId == expenditureType.Id);
            });

            return await Task.FromResult(mappedExpenditureTypes);
        }

        public async Task<ExpenseTypeDetails> GetById(int id)
        {
            var expenditureType = _expenseTypeRepository.GetById(id);

            if (expenditureType == null)
            {
                return null;
            }

            return await Task.FromResult(Mapper.Map<ExpenseTypeDetails>(expenditureType));
        }

        public async Task<bool> CreateExpenseType(ExpenseTypeDetails expenditureTypeDetails, string userId)
        {
            var expenseType = Mapper.Map<ExpenseType>(expenditureTypeDetails);
            expenseType.User_Id = userId;

            _expenseTypeRepository.Create(expenseType);

            return await Task.FromResult(true);
        }

        public async Task<bool> EditExpenseType(ExpenseTypeDetails expenseTypeDetails, string userId)
        {
            var expenseType = _expenseTypeRepository.GetById(expenseTypeDetails.Id);
            expenseType.Name = expenseTypeDetails.Name;
            expenseType.GraphColor = expenseTypeDetails.GraphColor;
            expenseType.ShowOnDashboard = expenseTypeDetails.ShowOnDashboard;
            expenseType.User_Id = userId;

            _expenseTypeRepository.Update(expenseType);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteExpenseType(int id)
        {
            var expenditureType = _expenseTypeRepository.GetById(id);
            _expenseTypeRepository.Delete(expenditureType);
            return await Task.FromResult(true);
        }
    }
}