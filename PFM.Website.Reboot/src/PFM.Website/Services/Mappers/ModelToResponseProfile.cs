using AutoMapper;

namespace PFM.Website.Services.Mappers
{
	public class ModelToResponseProfile : Profile
    {
		public ModelToResponseProfile()
		{
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeList, Models.ExpenseTypeModel>();
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails, Models.ExpenseTypeModel>();
        }
    }
}

