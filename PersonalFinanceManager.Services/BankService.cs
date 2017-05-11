using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Bank;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.IO;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Core.Exceptions;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.Core;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
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

        public IList<BankListModel> GetBanks()
        {
            var banks = _bankRepository.GetList().Include(u => u.Country).ToList();

            var banksModel = banks.Select(x => Mapper.Map<BankListModel>(x)).ToList();

            banksModel.ForEach(bank =>
            {
                var hasAccounts = _bankAccountRepository.GetList().Any(x => x.BankId == bank.Id);
                bank.CanBeDeleted = !hasAccounts;
            });

            return banksModel;
        }

        public void Validate(BankEditModel bankEditModel)
        {
            var duplicateName = _bankRepository.GetList().Any(x => x.Name.ToLowerInvariant() == bankEditModel.Name.Trim().ToLowerInvariant() && x.Id != bankEditModel.Id);
            if (duplicateName)
            {
                throw new BusinessException("Name", BusinessExceptionMessage.BankDuplicateName);
            }
        }

        public void CreateBank(BankEditModel bankEditModel)
        {
            Validate(bankEditModel);

            var bankModel = Mapper.Map<BankModel>(bankEditModel);
            _bankRepository.Create(bankModel);
         
            var bankBranchModel = Mapper.Map<BankBrandModel>(bankEditModel.FavoriteBranch);
            bankBranchModel.BankId = bankModel.Id;
            _bankBranchRepository.Create(bankBranchModel);
        }

        public BankEditModel GetById(int id)
        {
            var bank = _bankRepository.GetById(id);

            if (bank == null)
            {
                return null;
            }

            var mappedBank = Mapper.Map<BankEditModel>(bank);

            var bankBranch = _bankBranchRepository.GetList().SingleOrDefault(x => x.BankId == id);
            mappedBank.FavoriteBranch = Mapper.Map<BankBrandEditModel>(bankBranch);

            return mappedBank;
        }

        public void EditBank(BankEditModel bankEditModel)
        {
            Validate(bankEditModel);

            var bankModel = _bankRepository.GetList().AsNoTracking().SingleOrDefault(x => x.Id == bankEditModel.Id);
            bankModel = Mapper.Map<BankModel>(bankEditModel);
            _bankRepository.Update(bankModel);

            var bankBranchModel = _bankBranchRepository.GetList().AsNoTracking().SingleOrDefault(x => x.BankId == bankModel.Id);
            bankBranchModel = Mapper.Map<BankBrandModel>(bankEditModel.FavoriteBranch);
            _bankBranchRepository.Update(bankBranchModel);
        }

        public void DeleteBank(int id)
        {
            var bankBranchModel = _bankBranchRepository.GetList().SingleOrDefault(x => x.BankId == id);
            _bankBranchRepository.Delete(bankBranchModel);

            var bankModel = _bankRepository.GetList().Find(id);
            _bankRepository.Delete(bankModel);
        }
    }
}