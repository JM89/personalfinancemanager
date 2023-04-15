using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using PFM.Api.Contracts.Pension;

namespace PFM.Services
{
    public class PensionService : IPensionService
    {
        private readonly IPensionRepository _pensionRepository;
        
        public PensionService(IPensionRepository pensionRepository)
        {
            this._pensionRepository = pensionRepository;
        }

        public IList<PensionList> GetPensions(string userId)
        {
            var pensions = _pensionRepository.GetList2(u => u.Currency, u => u.Country).Where(x => x.UserId == userId).ToList();

            var mappedPensions = pensions.Select(Mapper.Map<PensionList>);

            return mappedPensions.ToList();
        }

        public void CreatePension(PensionDetails pensionDetails)
        {
            var pension = Mapper.Map<Pension>(pensionDetails);
            _pensionRepository.Create(pension);
        }

        public PensionDetails GetById(int id)
        {
            var pension = _pensionRepository.GetById(id);
            if (pension == null)
            {
                return null;
            }
            return Mapper.Map<PensionDetails>(pension);
        }

        public void EditPension(PensionDetails pensionDetails)
        {
            var pension = Mapper.Map<Pension>(pensionDetails);
            _pensionRepository.Update(pension);
        }

        public void DeletePension(int id)
        {
            var pension = _pensionRepository.GetById(id);
            
            _pensionRepository.Delete(pension);
        }
    }
}