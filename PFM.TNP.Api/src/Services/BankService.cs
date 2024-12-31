using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.Core.Exceptions;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class BankService(IBankRepository bankRepository) : IBankService
    {
        public Task<List<BankList>> GetBanks(string userId)
        {
            var banks = bankRepository
                .GetList2(u => u.Country)
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedBanks = banks.Select(x => Mapper.Map<BankList>(x)).ToList();

            mappedBanks.ForEach(bank =>
            {
            });

            return Task.FromResult(mappedBanks);
        }

        public Task<bool> Validate(BankDetails bankDetails)
        {
            var duplicateName = bankRepository.GetList().Any(x => x.Name.ToLower() == bankDetails.Name.Trim().ToLower() && x.Id != bankDetails.Id);
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

            bankRepository.Create(bank);

            return true;
        }

        public Task<BankDetails> GetById(int id)
        {
            var bank = bankRepository.GetById(id);

            if (bank == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<BankDetails>(bank));
        }

        public async Task<bool> EditBank(BankDetails bankDetails, string userId)
        {
            await Validate(bankDetails);

            var bank = bankRepository.GetListAsNoTracking().SingleOrDefault(x => x.Id == bankDetails.Id);
            bank = Mapper.Map<DataAccessLayer.Entities.Bank>(bankDetails);
            bank.User_Id = userId;

            bankRepository.Update(bank);

            return true;
        }

        public Task<bool> DeleteBank(int id)
        {
            var bank = bankRepository.GetList().Find(id);
            bankRepository.Delete(bank);
            return Task.FromResult(true);
        }
    }
}