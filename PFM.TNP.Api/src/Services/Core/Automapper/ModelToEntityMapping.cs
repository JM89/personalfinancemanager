using AutoMapper;

namespace Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankDetails, DataAccessLayer.Entities.Bank>();
        }
    }
}
