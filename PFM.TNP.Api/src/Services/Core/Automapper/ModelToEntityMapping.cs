using AutoMapper;

namespace Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<PFM.Pension.Api.Contracts.Pension.PensionDetails, DataAccessLayer.Entities.Pension>();
        }
    }
}
