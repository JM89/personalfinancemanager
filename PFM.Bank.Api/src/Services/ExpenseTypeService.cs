using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseTypeRepository _expenditureTypeRepository;
        private readonly IExpenseRepository _expenditureRepository;

        public ExpenseTypeService(IExpenseTypeRepository expenditureTypeRepository, IExpenseRepository expenditureRepository)
        {
            this._expenditureTypeRepository = expenditureTypeRepository;
            this._expenditureRepository = expenditureRepository;
        }

        public IList<ExpenseTypeList> GetExpenseTypes()
        {
            var expenditures = _expenditureRepository.GetList();

            var expenditureTypes = _expenditureTypeRepository.GetList().ToList();

            var mappedExpenditureTypes = expenditureTypes.Select(x => Mapper.Map<ExpenseTypeList>(x)).ToList();

            mappedExpenditureTypes.ForEach(expenditureType =>
            {
                expenditureType.CanBeDeleted = !expenditures.Any(x => x.ExpenseTypeId == expenditureType.Id);
            });

            return mappedExpenditureTypes;
        }

        public ExpenseTypeDetails GetById(int id)
        {
            var expenditureType = _expenditureTypeRepository.GetById(id);

            if (expenditureType == null)
            {
                return null;
            }

            return Mapper.Map<ExpenseTypeDetails>(expenditureType);
        }

        public void CreateExpenseType(ExpenseTypeDetails expenditureTypeDetails)
        {
            var expenditureType = Mapper.Map<ExpenseType>(expenditureTypeDetails);
            _expenditureTypeRepository.Create(expenditureType);
        }

        public void EditExpenseType(ExpenseTypeDetails expenditureTypeDetails)
        {
            var expenditureType = _expenditureTypeRepository.GetById(expenditureTypeDetails.Id);
            expenditureType.Name = expenditureTypeDetails.Name;
            expenditureType.GraphColor = expenditureTypeDetails.GraphColor;
            expenditureType.ShowOnDashboard = expenditureTypeDetails.ShowOnDashboard;
            _expenditureTypeRepository.Update(expenditureType);
        }

        public void DeleteExpenseType(int id)
        {
            var expenditureType = _expenditureTypeRepository.GetById(id);
            _expenditureTypeRepository.Delete(expenditureType);
        }
    }
}