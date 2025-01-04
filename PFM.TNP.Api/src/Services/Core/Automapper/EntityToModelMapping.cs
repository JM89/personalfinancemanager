using AutoMapper;

namespace Services.Core.Automapper;

public class EntityToModelMapping : Profile
{
    public EntityToModelMapping()
    {
        CreateMap<DataAccessLayer.Entities.Pension, PFM.Pension.Api.Contracts.Pension.PensionList>();
        CreateMap<DataAccessLayer.Entities.Pension, PFM.Pension.Api.Contracts.Pension.PensionDetails>();
    }
}