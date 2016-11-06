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

namespace PersonalFinanceManager.Services
{
    public class BankService
    {
        ApplicationDbContext db;

        public BankService()
        {
            db = new ApplicationDbContext();
        }

        public IList<BankListModel> GetBanks()
        {
            var banks = db.BankModels.Include(u => u.Country).ToList();

            return banks.Select(x => Mapper.Map<BankListModel>(x)).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void CreateBank(BankEditModel bankEditModel, string folderPath)
        {
            var duplicateName = db.BankModels.Any(x => x.Name.ToLower() == bankEditModel.Name.Trim().ToLower());

            if (duplicateName)
            {
                throw new BusinessException("Name", BusinessExceptionMessage.BankDuplicateName);
            }

            var bankModel = Mapper.Map<BankModel>(bankEditModel);

            var fileSource = bankEditModel.UrlPreview;
            var fileDestination = fileSource.Replace("preview", "bank_icons");

            bankModel.IconPath = fileDestination;

            File.Copy(folderPath + fileSource, folderPath + fileDestination);
            
            db.BankModels.Add(bankModel);
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

            mappedBank.UrlPreview = bank.IconPath;

            return mappedBank;
        }

        public void EditBank(BankEditModel bankEditModel, string folderPath)
        {
            var bankModel = db.BankModels.SingleOrDefault(x => x.Id == bankEditModel.Id);

            bankModel.Name = bankEditModel.Name;
            bankModel.CountryId = bankEditModel.CountryId;

            var fileSource = bankEditModel.UrlPreview;
            var fileDestination = fileSource.Replace("preview", "bank_icons");

            bankModel.IconPath = fileDestination;

            File.Copy(folderPath + fileSource, folderPath + fileDestination);

            db.Entry(bankModel).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeleteBank(int id)
        {
            BankModel bankModel = db.BankModels.Find(id);
            db.BankModels.Remove(bankModel);
            db.SaveChanges();
        }
    }
}