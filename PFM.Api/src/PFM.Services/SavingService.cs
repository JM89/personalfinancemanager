using System;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using AutoMapper;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.MovementStrategy;
using PFM.Api.Contracts.Saving;
using System.Transactions;
using System.Threading.Tasks;
using PFM.Services.Caches;

namespace PFM.Services;

public interface ISavingService : IBaseService
{
    Task<IList<SavingList>> GetSavingsByAccountId(int accountId);

    Task<bool> CreateSaving(SavingDetails savingDetails);

    Task<SavingDetails> GetById(int id);

    Task<bool> DeleteSaving(int id);
}

public class SavingService(
    IMapper mapper,
    ISavingRepository savingRepository,
    IBankAccountCache bankAccountCache,
    ContextMovementStrategy contextMovementStrategy)
    : ISavingService
{
    public async Task<bool> CreateSaving(SavingDetails savingDetails)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var saving = mapper.Map<Saving>(savingDetails);

        var movement = new Movement(savingDetails);

        var strategy = contextMovementStrategy.GetMovementStrategy(movement.PaymentMethod);
        var result  = await strategy.Debit(movement);

        if (!movement.TargetIncomeId.HasValue)
            throw new ArgumentException("Target Income ID should not be null.");

        saving.GeneratedIncomeId = movement.TargetIncomeId.Value;

        savingRepository.Create(saving);

        scope.Complete();

        return result;
    }

    public async Task<bool> DeleteSaving(int id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var saving = savingRepository.GetById(id);
        var savingDetails = mapper.Map<SavingDetails>(saving);

        savingRepository.Delete(saving);

        var strategy = contextMovementStrategy.GetMovementStrategy(DataAccessLayer.Enumerations.PaymentMethod.InternalTransfer);
        var result = await strategy.Credit(new Movement(savingDetails));

        scope.Complete();

        return result;
    }

    public async Task<SavingDetails> GetById(int id)
    {
        var saving = savingRepository.GetById(id);

        if (saving == null)
        {
            return null;
        }

        var mappedSaving = mapper.Map<SavingDetails>(saving);

        var targetAccount = await bankAccountCache.GetById(saving.TargetInternalAccountId);
        mappedSaving.TargetInternalAccountName = targetAccount.Name;

        return mappedSaving;
    }

    public async Task<IList<SavingList>> GetSavingsByAccountId(int accountId)
    {
        var savings = savingRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

        var mappedSavings = new List<SavingList>();

        foreach (var saving in savings)
        {
            var map = mapper.Map<SavingList>(saving);

            var srcAccount = await bankAccountCache.GetById(saving.AccountId);
            map.AccountCurrencySymbol = srcAccount.CurrencySymbol;

            var targetAccount = await bankAccountCache.GetById(saving.TargetInternalAccountId);
            map.TargetInternalAccountName = targetAccount.Name;

            mappedSavings.Add(map);
        }

        return mappedSavings.ToList();
    }
}