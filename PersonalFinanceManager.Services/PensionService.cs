using AutoMapper;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Pension;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace PersonalFinanceManager.Services
{
    public class PensionService : IPensionService
    {
        private readonly IPensionRepository _pensionRepository;
        
        public PensionService(IPensionRepository pensionRepository)
        {
            this._pensionRepository = pensionRepository;
        }

        public IList<PensionListModel> GetPensions(string userId)
        {
            var pensions = _pensionRepository.GetList().Include(u => u.Currency).Include(u => u.Country).Where(x => x.UserId == userId).ToList();

            var pensionsModel = pensions.Select(Mapper.Map<PensionListModel>);

            return pensionsModel.ToList();
        }

        public void CreatePension(PensionEditModel pensionEditModel)
        {
            var pensionModel = Mapper.Map<PensionModel>(pensionEditModel);
            _pensionRepository.Create(pensionModel);
        }

        public PensionEditModel GetById(int id)
        {
            var pension = _pensionRepository.GetById(id);
            if (pension == null)
            {
                return null;
            }
            return Mapper.Map<PensionEditModel>(pension);
        }

        public void EditPension(PensionEditModel pensionEditModel)
        {
            var pension = Mapper.Map<PensionModel>(pensionEditModel);
            _pensionRepository.Update(pension);
        }

        public void DeletePension(int id)
        {
            var pension = _pensionRepository.GetById(id);
            
            _pensionRepository.Delete(pension);
        }
    }
}