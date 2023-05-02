using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Core.Automapper
{
    public class SearchParametersMapping : Profile
    {
        public SearchParametersMapping()
        {
            CreateMap<Api.Contracts.SearchParameters.ExpenseGetListSearchParameters, DataAccessLayer.SearchParameters.ExpenseGetListSearchParameters>();
        }
    }
}
