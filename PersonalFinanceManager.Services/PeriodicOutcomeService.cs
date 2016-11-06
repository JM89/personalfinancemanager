using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.PeriodicOutcome;
using PersonalFinanceManager.DataAccess;


using AutoMapper;

namespace PersonalFinanceManager.Services
{
    public class PeriodicOutcomeService
    {
        ApplicationDbContext db;

        public PeriodicOutcomeService()
        {
            db = new ApplicationDbContext();
        }

        public IList<PeriodicOutcomeListModel> GetPeriodicOutcomes(int accountId)
        {
            var periodicOutcomes = db.PeriodicOutcomeModels
                .Include(u => u.Account.Currency)
                .Include(u => u.TypeExpenditure)
                .Include(u => u.Frequency).ToList();

            var periodicOutcomesModel = periodicOutcomes.Select(x => Mapper.Map<PeriodicOutcomeListModel>(x));

            return periodicOutcomesModel.ToList();
        }

        public PeriodicOutcomeEditModel GetById(int id)
        {
            var outcome = db.PeriodicOutcomeModels
                .SingleOrDefault(x => x.Id == id);

            if (outcome == null)
            {
                return null;
            }

            return Mapper.Map<PeriodicOutcomeEditModel>(outcome);
        }

        public void CreatePeriodicOutcome(PeriodicOutcomeEditModel PeriodicOutcomeEditModel)
        {
            var PeriodicOutcomeModel = Mapper.Map<PeriodicOutcomeModel>(PeriodicOutcomeEditModel);
            PeriodicOutcomeModel.IsEnabled = true;

            db.PeriodicOutcomeModels.Add(PeriodicOutcomeModel);

            db.SaveChanges();
        }

        public void EditPeriodicOutcome(PeriodicOutcomeEditModel PeriodicOutcomeEditModel)
        {
            var PeriodicOutcomeModel = db.PeriodicOutcomeModels.SingleOrDefault(x => x.Id == PeriodicOutcomeEditModel.Id);

            PeriodicOutcomeModel.Description = PeriodicOutcomeEditModel.Description;
            PeriodicOutcomeModel.Cost = PeriodicOutcomeEditModel.Cost;
            PeriodicOutcomeModel.AccountId = PeriodicOutcomeEditModel.AccountId;
            PeriodicOutcomeModel.FrequencyId = PeriodicOutcomeEditModel.FrequencyId;
            PeriodicOutcomeModel.StartDate = PeriodicOutcomeEditModel.StartDate;
            PeriodicOutcomeModel.TypeExpenditureId = PeriodicOutcomeEditModel.TypeExpenditureId;

            db.Entry(PeriodicOutcomeModel).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeletePeriodicOutcome(int id)
        {
            PeriodicOutcomeModel PeriodicOutcomeModel = db.PeriodicOutcomeModels.Find(id);
            db.PeriodicOutcomeModels.Remove(PeriodicOutcomeModel);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }


        public void ChangeStatus(int id, bool status)
        {
            var periodicOutcomeModel = db.PeriodicOutcomeModels.SingleOrDefault(x => x.Id == id);

            periodicOutcomeModel.IsEnabled = status;
            if (!status)
            {
                periodicOutcomeModel.EndDate = DateTime.Now;
            }

            db.Entry(periodicOutcomeModel).State = EntityState.Modified;

            db.SaveChanges();
        }
    }
}