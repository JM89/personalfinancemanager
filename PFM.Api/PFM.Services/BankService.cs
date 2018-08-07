using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DTOs.Bank;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Core.Exceptions;
using PFM.DataAccessLayer.Entities;

namespace PFM.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankBranchRepository _bankBranchRepository;

        public BankService(IBankRepository bankRepository, IBankAccountRepository bankAccountRepository, IBankBranchRepository bankBranchRepository)
        {
            this._bankRepository = bankRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._bankBranchRepository = bankBranchRepository;
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

            var bank = Mapper.Map<Bank>(bankDetails);
            _bankRepository.Create(bank);
         
            var bankBranch = Mapper.Map<BankBranch>(bankDetails.FavoriteBranch);
            bankBranch.BankId = bank.Id;
            _bankBranchRepository.Create(bankBranch);
        }

        public BankDetails GetById(int id)
        {
            var bank = _bankRepository.GetById(id);

            if (bank == null)
            {
                return null;
            }

            var mappedBank = Mapper.Map<BankDetails>(bank);

            var bankBranch = _bankBranchRepository.GetList().SingleOrDefault(x => x.BankId == id);
            mappedBank.FavoriteBranch = Mapper.Map<BankBranchDetails>(bankBranch);

            return mappedBank;
        }

        public void EditBank(BankDetails bankDetails)
        {
            Validate(bankDetails);

            var bank = _bankRepository.GetListAsNoTracking().SingleOrDefault(x => x.Id == bankDetails.Id);
            bank = Mapper.Map<Bank>(bankDetails);
            _bankRepository.Update(bank);

            var bankBranch = _bankBranchRepository.GetListAsNoTracking().SingleOrDefault(x => x.BankId == bank.Id);
            bankBranch = Mapper.Map<BankBranch>(bankDetails.FavoriteBranch);
            _bankBranchRepository.Update(bankBranch);
        }

        public void DeleteBank(int id)
        {
            var bankBranch = _bankBranchRepository.GetList().SingleOrDefault(x => x.BankId == id);
            _bankBranchRepository.Delete(bankBranch);

            var bank = _bankRepository.GetList().Find(id);
            _bankRepository.Delete(bank);
        }
    }
}