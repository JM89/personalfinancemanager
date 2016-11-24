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
        ApplicationDbContext db;

        public BankService()
        {
            db = new ApplicationDbContext();
        }

        public IList<BankListModel> GetBanks()
        {
            var banks = db.BankModels.Include(u => u.Country).ToList();

            var banksModel = banks.Select(x => Mapper.Map<BankListModel>(x)).ToList();

            banksModel.ForEach(bank =>
            {
                var hasAccounts = db.AccountModels.Any(x => x.BankId == bank.Id);
                bank.CanBeDeleted = !hasAccounts;
            });

            return banksModel;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void Validate(BankEditModel bankEditModel)
        {
            var duplicateName = db.BankModels.Any(x => x.Name.ToLower() == bankEditModel.Name.Trim().ToLower() && x.Id != bankEditModel.Id);
            if (duplicateName)
            {
                throw new BusinessException("Name", BusinessExceptionMessage.BankDuplicateName);
            }
        }

        public void CreateBank(BankEditModel bankEditModel)
        {
            Validate(bankEditModel);

            var bankModel = Mapper.Map<BankModel>(bankEditModel);
            db.BankModels.Add(bankModel);
            db.SaveChanges();

            var bankBranchModel = Mapper.Map<BankBrandModel>(bankEditModel.FavoriteBranch);
            bankBranchModel.BankId = bankModel.Id;
            db.BankBranchModels.Add(bankBranchModel);
            db.SaveChanges();
        }

        public BankEditModel GetById(int id)
        {
            var bank = db.BankModels.SingleOrDefault(x => x.Id == id);

            if (bank == null)
            {
                return null;
            }

            var mappedBank = Mapper.Map<BankEditModel>(bank);

            var bankBranch = db.BankBranchModels.SingleOrDefault(x => x.BankId == id);
            mappedBank.FavoriteBranch = Mapper.Map<BankBrandEditModel>(bankBranch);

            return mappedBank;
        }

        public void EditBank(BankEditModel bankEditModel)
        {
            Validate(bankEditModel);

            var bankModel = db.BankModels.AsNoTracking().SingleOrDefault(x => x.Id == bankEditModel.Id);
            var oldFileDestination = bankModel.IconPath;

            bankModel = Mapper.Map<BankModel>(bankEditModel);
            db.Entry(bankModel).State = EntityState.Modified;
            db.SaveChanges();

            var bankBranchModel = db.BankBranchModels.AsNoTracking().SingleOrDefault(x => x.BankId == bankModel.Id);
            bankBranchModel = Mapper.Map<BankBrandModel>(bankEditModel.FavoriteBranch);
            db.Entry(bankBranchModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteBank(int id)
        {
            var bankBranchModel = db.BankBranchModels.SingleOrDefault(x => x.BankId == id);
            db.BankBranchModels.Remove(bankBranchModel);
            db.SaveChanges();

            var bankModel = db.BankModels.Find(id);
            db.BankModels.Remove(bankModel);
            db.SaveChanges();
        }
    }
}