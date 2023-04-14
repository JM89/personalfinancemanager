using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Core.Automapper
{
    public class EntityToEntityMapping : Profile
    {
        public EntityToEntityMapping()
        {
            CreateMap<DataAccessLayer.Entities.Salary, DataAccessLayer.Entities.Salary>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<DataAccessLayer.Entities.SalaryDeduction, DataAccessLayer.Entities.SalaryDeduction>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
