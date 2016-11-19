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
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBankService : IBaseService
    {
        IList<BankListModel> GetBanks();

        void CreateBank(BankEditModel bankEditModel, string folderPath);

        BankEditModel GetById(int id);

        void EditBank(BankEditModel bankEditModel, string folderPath);

        void DeleteBank(int id);
    }
}