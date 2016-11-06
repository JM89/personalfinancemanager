using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.ExpenditureType;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureTypeService
    {
        ApplicationDbContext db;

        public ExpenditureTypeService()
        {
            db = new ApplicationDbContext();
        }

        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            var expenditures = db.ExpenditureModels;

            var expenditureTypes = db.ExpenditureTypeModels.ToList();

            var expenditureTypesModel = expenditureTypes.Select(x => Mapper.Map<ExpenditureTypeListModel>(x)).ToList();

            expenditureTypesModel.ForEach(expenditureType =>
            {
                expenditureType.CanBeDeleted = !expenditures.Any(x => x.TypeExpenditureId == expenditureType.Id);
            });

            return expenditureTypesModel;
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            var expenditureType = db.ExpenditureTypeModels.SingleOrDefault(x => x.Id == id);

            if (expenditureType == null)
            {
                return null;
            }

            return Mapper.Map<ExpenditureTypeEditModel>(expenditureType);
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            var expenditureTypeModel = Mapper.Map<ExpenditureTypeModel>(expenditureTypeEditModel);

            db.ExpenditureTypeModels.Add(expenditureTypeModel);
            db.SaveChanges();
        }

        public void EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            var expenditureTypeModel = db.ExpenditureTypeModels.SingleOrDefault(x => x.Id == expenditureTypeEditModel.Id);
            expenditureTypeModel.Name = expenditureTypeEditModel.Name;
            expenditureTypeModel.GraphColor = expenditureTypeEditModel.GraphColor;
            expenditureTypeModel.ShowOnDashboard = expenditureTypeEditModel.ShowOnDashboard;

            db.Entry(expenditureTypeModel).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeleteExpenditureType(int id)
        {
            ExpenditureTypeModel expenditureTypeModel = db.ExpenditureTypeModels.Find(id);
            db.ExpenditureTypeModels.Remove(expenditureTypeModel);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}