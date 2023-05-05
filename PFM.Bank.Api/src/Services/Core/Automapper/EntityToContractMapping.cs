using AutoMapper;

namespace PFM.Services.Core.Automapper
{
    public class EntityToContractMapping : Profile
    {
        public EntityToContractMapping()
        {
            CreateMap<DataAccessLayer.Entities.Account, PFM.Bank.Event.Contracts.BankAccountCreated>()
                .ForMember(a => a.UserId, opt => opt.MapFrom(x => x.User_Id));
            CreateMap<DataAccessLayer.Entities.Account, PFM.Bank.Event.Contracts.BankAccountDeleted>()
                .ForMember(a => a.UserId, opt => opt.MapFrom(x => x.User_Id));
        }
    }
}
