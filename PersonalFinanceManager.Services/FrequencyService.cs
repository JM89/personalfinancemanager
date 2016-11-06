using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Frequency;
using PersonalFinanceManager.DataAccess;

namespace PersonalFinanceManager.Services
{
    public class FrequencyService: IDisposable
    {
        ApplicationDbContext db;

        public FrequencyService()
        {
            db = new ApplicationDbContext();
        }

        public IList<FrequencyListModel> GetFrequencies()
        {
            var frequencies = db.FrequencyModels.ToList();

            return frequencies.Select(x => Mapper.Map<FrequencyListModel>(x)).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}