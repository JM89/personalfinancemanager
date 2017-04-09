using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.ExpenditureType;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class ExpenditureTypeHelper
    {
        public static ExpenditureTypeListModel CreateExpenditureTypeListModel(int id, string name)
        {
            var model = new ExpenditureTypeListModel
            {
                Id = id,
                Name = name
            };
            return model;
        }

        public static ExpenditureTypeModel CreateExpenditureTypeModel(int id, string name)
        {
            var entity = new ExpenditureTypeModel
            {
                Id = id,
                Name = name
            };
            return entity;
        }
    }
}
