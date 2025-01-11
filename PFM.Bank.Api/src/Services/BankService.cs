﻿using AutoMapper;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Services.Core;

namespace Services
{
    public interface IBankService : IBaseService
    {
        Task<List<BankList>> GetBanks(string userId);

        Task<bool> CreateBank(BankDetails bankDetails, string userId);

        Task<BankDetails> GetById(int id);

        Task<bool> EditBank(BankDetails bankDetails, string userId);

        Task<bool> DeleteBank(int id);
    }
    
    public class BankService(IBankRepository repository, IBankAccountRepository bankAccountRepository)
        : IBankService
    {
        public Task<List<BankList>> GetBanks(string userId)
        {
            var banks = repository
                .GetList2(u => u.Country)
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedBanks = banks.Select(x => Mapper.Map<BankList>(x)).ToList();

            mappedBanks.ForEach(bank =>
            {
                var hasAccounts = bankAccountRepository.GetList().Any(x => x.BankId == bank.Id);
                bank.CanBeDeleted = !hasAccounts;
            });

            return Task.FromResult(mappedBanks);
        }

        public Task<bool> Validate(BankDetails bankDetails)
        {
            var duplicateName = repository.GetList().Any(x => x.Name.ToLower() == bankDetails.Name.Trim().ToLower() && x.Id != bankDetails.Id);
            if (duplicateName)
            {
                throw new BusinessException("Name", BusinessExceptionMessage.BankDuplicateName);
            }

            return Task.FromResult(true);
        }

        public async Task<bool> CreateBank(BankDetails bankDetails, string userId)
        {
            await Validate(bankDetails);

            var bank = Mapper.Map<DataAccessLayer.Entities.Bank>(bankDetails);
            bank.User_Id = userId;

            repository.Create(bank);

            return true;
        }

        public Task<BankDetails> GetById(int id)
        {
            var bank = repository.GetById(id);

            if (bank == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<BankDetails>(bank));
        }

        public async Task<bool> EditBank(BankDetails bankDetails, string userId)
        {
            await Validate(bankDetails);

            var bank = repository.GetListAsNoTracking().SingleOrDefault(x => x.Id == bankDetails.Id);
            bank = Mapper.Map<DataAccessLayer.Entities.Bank>(bankDetails);
            bank.User_Id = userId;

            repository.Update(bank);

            return true;
        }

        public Task<bool> DeleteBank(int id)
        {
            var bank = repository.GetList().Find(id);
            repository.Delete(bank);
            return Task.FromResult(true);
        }
    }
}