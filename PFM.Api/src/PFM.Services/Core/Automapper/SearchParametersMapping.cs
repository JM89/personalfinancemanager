using AutoMapper;

namespace PFM.Services.Core.Automapper
{
    public class SearchParametersMapping : Profile
    {
        public SearchParametersMapping()
        {
            CreateMap<Api.Contracts.SearchParameters.ExpenseGetListSearchParameters, DataAccessLayer.SearchParameters.ExpenseGetListSearchParameters>();
            CreateMap<Api.Contracts.SearchParameters.MovementSummarySearchParameters, DataAccessLayer.SearchParameters.MovementSummarySearchParameters>();
        }
    }
}
