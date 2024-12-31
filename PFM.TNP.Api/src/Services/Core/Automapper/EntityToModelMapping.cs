using AutoMapper;

namespace Services.Core.Automapper
{
    public class EntityToModelMapping : Profile
    {
        public EntityToModelMapping()
        {
            CreateMap<DataAccessLayer.Entities.Bank, PFM.Bank.Api.Contracts.Bank.BankList>();
            CreateMap<DataAccessLayer.Entities.Bank, PFM.Bank.Api.Contracts.Bank.BankDetails>()
                .ForMember(a => a.OwnerId, opt => opt.MapFrom(x => x.User_Id));
        }
    }
}
