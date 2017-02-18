using AutoMapper;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Models.Tax;
using System;

namespace PersonalFinanceManager.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;

        public TaxService(ITaxRepository taxRepository)
        {
            this._taxRepository = taxRepository;
        }

        public void CreateTax(TaxEditModel taxEditModel)
        {
            var taxModel = Mapper.Map<TaxModel>(taxEditModel);
            _taxRepository.Create(taxModel);
        }

        public void DeleteTax(int id)
        {
            var tax = _taxRepository.GetById(id);
            _taxRepository.Delete(tax);
        }

        public void EditTax(TaxEditModel taxEditModel)
        {
            var taxModel = Mapper.Map<TaxModel>(taxEditModel);
            _taxRepository.Update(taxModel);
        }

        public TaxEditModel GetById(int id)
        {
            var tax = _taxRepository.GetById(id);
            if (tax == null)
            {
                return null;
            }
            return Mapper.Map<TaxEditModel>(tax);
        }

        public IList<TaxListModel> GetTaxes(string userId)
        {
            var taxes = _taxRepository.GetList()
                            .Include(x => x.Country)
                            .Include(x => x.Currency)
                            .Include(x => x.FrequenceOption)
                            .Include(x => x.TaxType)
                            .Where(x => x.UserId == userId).ToList();
            
            var taxesModel = new List<TaxListModel>();

            foreach (var tax in taxes)
            {
                var taxModel = Mapper.Map<TaxListModel>(tax);

                switch ((FrequenceOption)tax.FrequenceOptionId)
                {
                    case FrequenceOption.Once:
                        taxModel.FrequenceDescription = "Once";
                        break;
                    case FrequenceOption.EveryXMonths:
                        taxModel.FrequenceDescription = $"Every {tax.Frequence} months";
                        break;
                }

                taxesModel.Add(taxModel);
            }

            return taxesModel.ToList();
        }

        public IList<TaxListModel> GetTaxesByType(string currentUser, TaxType incomeTax)
        {
            var incomeTaxTypeId = (int) incomeTax;
            var taxes = _taxRepository.GetList().Where(x => x.TaxTypeId == incomeTaxTypeId).ToList();
            return taxes.Select(Mapper.Map<TaxListModel>).ToList();
        }
    }
}