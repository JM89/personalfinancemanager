﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Api.Contracts.ExpenseType;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class ExpenseTypeService(
        IMapper mapper,
        IExpenseTypeRepository expenditureTypeRepository,
        IExpenseRepository expenseTypeRepository)
        : IExpenseTypeService
    {
        public async Task<IList<ExpenseTypeList>> GetExpenseTypes(string userId)
        {
            var expenses = expenseTypeRepository.GetList();

            var expenseTypes = expenditureTypeRepository
                .GetList()
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedExpenditureTypes = expenseTypes.Select(mapper.Map<ExpenseTypeList>).ToList();

            mappedExpenditureTypes.ForEach(expenditureType =>
            {
                expenditureType.CanBeDeleted = !expenses.Any(x => x.ExpenseTypeId == expenditureType.Id);
            });

            return await Task.FromResult(mappedExpenditureTypes);
        }

        public async Task<ExpenseTypeDetails> GetById(int id)
        {
            var expenditureType = expenditureTypeRepository.GetById(id);

            if (expenditureType == null)
            {
                return null;
            }

            return await Task.FromResult(mapper.Map<ExpenseTypeDetails>(expenditureType));
        }

        public async Task<bool> CreateExpenseType(ExpenseTypeDetails expenditureTypeDetails, string userId)
        {
            var expenseType = mapper.Map<ExpenseType>(expenditureTypeDetails);
            expenseType.User_Id = userId;

            expenditureTypeRepository.Create(expenseType);

            return await Task.FromResult(true);
        }

        public async Task<bool> EditExpenseType(ExpenseTypeDetails expenseTypeDetails, string userId)
        {
            var expenseType = expenditureTypeRepository.GetById(expenseTypeDetails.Id);
            expenseType.Name = expenseTypeDetails.Name;
            expenseType.GraphColor = expenseTypeDetails.GraphColor;
            expenseType.ShowOnDashboard = expenseTypeDetails.ShowOnDashboard;
            expenseType.User_Id = userId;

            expenditureTypeRepository.Update(expenseType);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteExpenseType(int id)
        {
            var expenditureType = expenditureTypeRepository.GetById(id);
            expenditureTypeRepository.Delete(expenditureType);
            return await Task.FromResult(true);
        }
    }
}