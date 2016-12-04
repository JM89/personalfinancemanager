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
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureTypeService : IExpenditureTypeService
    {
        private readonly IExpenditureTypeRepository _expenditureTypeRepository;
        private readonly IExpenditureRepository _expenditureRepository;

        public ExpenditureTypeService(IExpenditureTypeRepository expenditureTypeRepository, IExpenditureRepository expenditureRepository)
        {
            this._expenditureTypeRepository = expenditureTypeRepository;
            this._expenditureRepository = expenditureRepository;
        }

        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            var expenditures = _expenditureRepository.GetList();

            var expenditureTypes = _expenditureTypeRepository.GetList().ToList();

            var expenditureTypesModel = expenditureTypes.Select(x => Mapper.Map<ExpenditureTypeListModel>(x)).ToList();

            expenditureTypesModel.ForEach(expenditureType =>
            {
                expenditureType.CanBeDeleted = !expenditures.Any(x => x.TypeExpenditureId == expenditureType.Id);
            });

            return expenditureTypesModel;
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            var expenditureType = _expenditureTypeRepository.GetById(id);

            if (expenditureType == null)
            {
                return null;
            }

            return Mapper.Map<ExpenditureTypeEditModel>(expenditureType);
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            var expenditureTypeModel = Mapper.Map<ExpenditureTypeModel>(expenditureTypeEditModel);
            _expenditureTypeRepository.Create(expenditureTypeModel);
        }

        public void EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            var expenditureTypeModel = _expenditureTypeRepository.GetById(expenditureTypeEditModel.Id);
            expenditureTypeModel.Name = expenditureTypeEditModel.Name;
            expenditureTypeModel.GraphColor = expenditureTypeEditModel.GraphColor;
            expenditureTypeModel.ShowOnDashboard = expenditureTypeEditModel.ShowOnDashboard;
            _expenditureTypeRepository.Update(expenditureTypeModel);
        }

        public void DeleteExpenditureType(int id)
        {
            var expenditureTypeModel = _expenditureTypeRepository.GetById(id);
            _expenditureTypeRepository.Delete(expenditureTypeModel);
        }
    }
}