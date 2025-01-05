using System;
using AutoMapper;

namespace Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<PFM.TNP.Api.Contracts.Pension.PensionSaveRequest, DataAccessLayer.Entities.Pension>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            
            CreateMap<PFM.TNP.Api.Contracts.IncomeTaxReport.IncomeTaxReportSaveRequest, DataAccessLayer.Entities.IncomeTaxReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(x => x.PayDay, y => y.MapFrom(x => x.PayDay.ToDateTime(TimeOnly.MinValue)));
        }
    }
}
