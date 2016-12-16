using AutoMapper;
using Moq;
using PersonalFinanceManager.Utils.Automapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.UnitTests
{
    public static class TestHelper
    {
        public static void ConfigureAutomapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
            });
        }

        public static Mock<DbSet<T>> GetDbSet<T>(IList<T> data) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();
            var dataSet = data.AsQueryable();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataSet.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataSet.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataSet.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(dataSet.GetEnumerator());
            dbSet.Setup(m => m.Local).Returns(new ObservableCollection<T>(data));
            return dbSet;
        }
    }
}
