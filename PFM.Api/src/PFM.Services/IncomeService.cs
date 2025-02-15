﻿using AutoMapper;
using PFM.Api.Contracts.Income;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.MovementStrategy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using PFM.Services.Caches;
using PFM.Services.Core.Exceptions;

namespace PFM.Services;

public interface IIncomeService : IBaseService
{
    Task<bool> CreateIncomes(List<IncomeDetails> incomeDetails);

    Task<bool> CreateIncome(IncomeDetails incomeDetails);

    Task<IList<IncomeList>> GetIncomes(int accountId);

    Task<IncomeDetails> GetById(int id);

    Task<bool> DeleteIncome(int id);
}

public class IncomeService(
        IMapper mapper,
        IIncomeRepository incomeRepository,
        IBankAccountCache bankAccountCache,
        ContextMovementStrategy contextMovementStrategy)
        : IIncomeService
{
    public async Task<bool> CreateIncomes(List<IncomeDetails> incomeDetails)
    {
        var resultBatch = true;

        foreach (var income in incomeDetails)
        {
            var result = await CreateIncome(income);
            if (!result)
                resultBatch = false;
        }

        return resultBatch; 
    }

    public async Task<bool> CreateIncome(IncomeDetails incomeDetails)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var income = mapper.Map<Income>(incomeDetails);

            var movement = new Movement(incomeDetails);

            var strategy = contextMovementStrategy.GetMovementStrategy(DataAccessLayer.Enumerations.PaymentMethod.Transfer);
            var result = await strategy.Credit(movement);

            incomeRepository.Create(income);

            scope.Complete();

            return result;
        }
    }

    public async Task<IList<IncomeList>> GetIncomes(int accountId)
    {
        var incomes = incomeRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

        var mappedIncomes = new List<IncomeList>();
        
        foreach (var income in incomes)
        {
            var map = mapper.Map<IncomeList>(income);

            var account = await bankAccountCache.GetById(income.AccountId);
            map.AccountCurrencySymbol = account.CurrencySymbol;

            mappedIncomes.Add(map);
        }

        return mappedIncomes.ToList();
    }

    public Task<IncomeDetails> GetById(int id)
    {
        var entity = incomeRepository.GetById(id);

        if (entity == null)
        {
            throw new BusinessException(nameof(Income),$"No entity found for id {id}");
        }

        return Task.FromResult(mapper.Map<IncomeDetails>(entity));
    }

    public async Task<bool> DeleteIncome(int id)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var income = incomeRepository.GetById(id);
            var incomeDetails = mapper.Map<IncomeDetails>(income);

            var strategy = contextMovementStrategy.GetMovementStrategy(DataAccessLayer.Enumerations.PaymentMethod.Transfer);
            var result = await strategy.Debit(new Movement(incomeDetails));

            incomeRepository.Delete(income);

            scope.Complete();

            return result;
        }
    }
}