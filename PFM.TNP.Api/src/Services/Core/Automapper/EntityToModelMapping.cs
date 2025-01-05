using System;
using AutoMapper;

namespace Services.Core.Automapper;

public class EntityToModelMapping : Profile
{
    public EntityToModelMapping()
    {
        CreateMap<DataAccessLayer.Entities.Pension, PFM.TNP.Api.Contracts.Pension.PensionList>();
        CreateMap<DataAccessLayer.Entities.Pension, PFM.TNP.Api.Contracts.Pension.PensionDetails>();
        
        CreateMap<DataAccessLayer.Entities.IncomeTaxReport, PFM.TNP.Api.Contracts.IncomeTaxReport.IncomeTaxReportList>()
            .ForMember(x => x.PayDay, y => y.MapFrom(x => DateOnly.FromDateTime(x.PayDay)));
        CreateMap<DataAccessLayer.Entities.IncomeTaxReport, PFM.TNP.Api.Contracts.IncomeTaxReport.IncomeTaxReportDetails>()
            .ForMember(x => x.PayDay, y => y.MapFrom(x => DateOnly.FromDateTime(x.PayDay)));
    }
}