using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Core.Automapper
{
    public class EntityToEntityMapping : Profile
    {
        public EntityToEntityMapping()
        {
            CreateMap<Entities.SalaryModel, Entities.SalaryModel>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Entities.SalaryDeductionModel, Entities.SalaryDeductionModel>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
