using AutoMapper;

namespace PFM.Website.Services.Mappers
{
	public class RequestToModelProfile : Profile
    {
		public RequestToModelProfile()
		{
            CreateMap<Models.ExpenseTypeModel, PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>();
        }
	}
}

