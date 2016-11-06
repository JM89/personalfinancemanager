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
    public class HistoricMovementService
    {
        ApplicationDbContext db;

        public HistoricMovementService()
        {
            db = new ApplicationDbContext();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}