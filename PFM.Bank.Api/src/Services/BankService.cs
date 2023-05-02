using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.Core.Exceptions;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public BankService(IBankRepository bankRepository, IBankAccountRepository bankAccountRepository)
        {
            this._bankRepository = bankRepository;
            this._bankAccountRepository = bankAccountRepository;
        }

        public IList<BankList> GetBanks()
        {
            var banks = _bankRepository.GetList2(u => u.Country).ToList();

            var mappedBanks = banks.Select(x => Mapper.Map<BankList>(x)).ToList();

            mappedBanks.ForEach(bank =>
            {
                var hasAccounts = _bankAccountRepository.GetList().Any(x => x.BankId == bank.Id);
                bank.CanBeDeleted = !hasAccounts;
            });

            return mappedBanks;
        }

        public void Validate(BankDetails bankDetails)
        {
            var duplicateName = _bankRepository.GetList().Any(x => x.Name.ToLower() == bankDetails.Name.Trim().ToLower() && x.Id != bankDetails.Id);
            if (duplicateName)
            {
                throw new BusinessException("Name", BusinessExceptionMessage.BankDuplicateName);
            }
        }

        public void CreateBank(BankDetails bankDetails)
        {
            Validate(bankDetails);

            var bank = Mapper.Map<DataAccessLayer.Entities.Bank>(bankDetails);
            _bankRepository.Create(bank);
        }

        public BankDetails GetById(int id)
        {
            var bank = _bankRepository.GetById(id);

            if (bank == null)
            {
                return null;
            }

            return Mapper.Map<BankDetails>(bank);
        }

        public void EditBank(BankDetails bankDetails)
        {
            Validate(bankDetails);

            var bank = _bankRepository.GetListAsNoTracking().SingleOrDefault(x => x.Id == bankDetails.Id);
            bank = Mapper.Map<DataAccessLayer.Entities.Bank>(bankDetails);
            _bankRepository.Update(bank);
        }

        public void DeleteBank(int id)
        {
            var bank = _bankRepository.GetList().Find(id);
            _bankRepository.Delete(bank);
        }
    }
}