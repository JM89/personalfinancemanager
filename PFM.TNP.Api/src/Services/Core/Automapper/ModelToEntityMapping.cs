using AutoMapper;

namespace Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<PFM.Pension.Api.Contracts.Pension.PensionDetails, DataAccessLayer.Entities.Pension>();
            CreateMap<PFM.Pension.Api.Contracts.Pension.PensionSaveRequest, DataAccessLayer.Entities.Pension>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
