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
using PersonalFinanceManager.Utils.Exceptions;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services
{
    public class BankService : IBankService
    {
        private ApplicationDbContext _db;

        public BankService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IList<BankListModel> GetBanks()
        {
            var banks = _db.BankModels.Include(u => u.Country).ToList();

            var banksModel = banks.Select(x => Mapper.Map<BankListModel>(x)).ToList();

            banksModel.ForEach(bank =>
            {
                var hasAccounts = _db.AccountModels.Any(x => x.BankId == bank.Id);
                bank.CanBeDeleted = !hasAccounts;
            });

            return banksModel;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Validate(BankEditModel bankEditModel)
        {
            var duplicateName = _db.BankModels.Any(x => x.Name.ToLower() == bankEditModel.Name.Trim().ToLower() && x.Id != bankEditModel.Id);
            if (duplicateName)
            {
                throw new BusinessException("Name", BusinessExceptionMessage.BankDuplicateName);
            }
        }

        public void CreateBank(BankEditModel bankEditModel)
        {
            Validate(bankEditModel);

            var bankModel = Mapper.Map<BankModel>(bankEditModel);
            _db.BankModels.Add(bankModel);
            _db.SaveChanges();

            var bankBranchModel = Mapper.Map<BankBrandModel>(bankEditModel.FavoriteBranch);
            bankBranchModel.BankId = bankModel.Id;
            _db.BankBranchModels.Add(bankBranchModel);
            _db.SaveChanges();
        }

        public BankEditModel GetById(int id)
        {
            var bank = _db.BankModels.SingleOrDefault(x => x.Id == id);

            if (bank == null)
            {
                return null;
            }

            var mappedBank = Mapper.Map<BankEditModel>(bank);

            var bankBranch = _db.BankBranchModels.SingleOrDefault(x => x.BankId == id);
            mappedBank.FavoriteBranch = Mapper.Map<BankBrandEditModel>(bankBranch);

            return mappedBank;
        }

        public void EditBank(BankEditModel bankEditModel)
        {
            Validate(bankEditModel);

            var bankModel = _db.BankModels.AsNoTracking().SingleOrDefault(x => x.Id == bankEditModel.Id);
            var oldFileDestination = bankModel.IconPath;

            bankModel = Mapper.Map<BankModel>(bankEditModel);
            _db.Entry(bankModel).State = EntityState.Modified;
            _db.SaveChanges();

            var bankBranchModel = _db.BankBranchModels.AsNoTracking().SingleOrDefault(x => x.BankId == bankModel.Id);
            bankBranchModel = Mapper.Map<BankBrandModel>(bankEditModel.FavoriteBranch);
            _db.Entry(bankBranchModel).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteBank(int id)
        {
            var bankBranchModel = _db.BankBranchModels.SingleOrDefault(x => x.BankId == id);
            _db.BankBranchModels.Remove(bankBranchModel);
            _db.SaveChanges();

            var bankModel = _db.BankModels.Find(id);
            _db.BankModels.Remove(bankModel);
            _db.SaveChanges();
        }
    }
}