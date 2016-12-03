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
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureTypeService : IExpenditureTypeService
    {
        private ApplicationDbContext _db;

        public ExpenditureTypeService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            var expenditures = _db.ExpenditureModels;

            var expenditureTypes = _db.ExpenditureTypeModels.ToList();

            var expenditureTypesModel = expenditureTypes.Select(x => Mapper.Map<ExpenditureTypeListModel>(x)).ToList();

            expenditureTypesModel.ForEach(expenditureType =>
            {
                expenditureType.CanBeDeleted = !expenditures.Any(x => x.TypeExpenditureId == expenditureType.Id);
            });

            return expenditureTypesModel;
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            var expenditureType = _db.ExpenditureTypeModels.SingleOrDefault(x => x.Id == id);

            if (expenditureType == null)
            {
                return null;
            }

            return Mapper.Map<ExpenditureTypeEditModel>(expenditureType);
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            var expenditureTypeModel = Mapper.Map<ExpenditureTypeModel>(expenditureTypeEditModel);

            _db.ExpenditureTypeModels.Add(expenditureTypeModel);
            _db.SaveChanges();
        }

        public void EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            var expenditureTypeModel = _db.ExpenditureTypeModels.SingleOrDefault(x => x.Id == expenditureTypeEditModel.Id);
            expenditureTypeModel.Name = expenditureTypeEditModel.Name;
            expenditureTypeModel.GraphColor = expenditureTypeEditModel.GraphColor;
            expenditureTypeModel.ShowOnDashboard = expenditureTypeEditModel.ShowOnDashboard;

            _db.Entry(expenditureTypeModel).State = EntityState.Modified;

            _db.SaveChanges();
        }

        public void DeleteExpenditureType(int id)
        {
            ExpenditureTypeModel expenditureTypeModel = _db.ExpenditureTypeModels.Find(id);
            _db.ExpenditureTypeModels.Remove(expenditureTypeModel);
            _db.SaveChanges();
        }
    }
}