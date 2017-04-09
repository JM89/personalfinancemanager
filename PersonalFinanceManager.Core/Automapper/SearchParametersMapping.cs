using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Core.Automapper
{
    public class SearchParametersMapping : Profile
    {
        public SearchParametersMapping()
        {
            CreateMap<Models.SearchParameters.ExpenditureGetListSearchParameters, Entities.SearchParameters.ExpenditureGetListSearchParameters>();
        }
    }
}
