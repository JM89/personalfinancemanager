using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using Moq.Language;
using Moq.Language.Flow;

namespace PersonalFinanceManager.Tests
{
    public static class DbSetMocking
    {
        public static Mock<DbSet<T>> CreateMockSet<T>(IList<T> data)
                where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
                    .Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression)
                    .Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType)
                    .Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                    .Returns(queryableData.GetEnumerator());
            return mockSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="setup"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <example>MockBankAccountRepository.Setup(x => x.GetList()).ReturnsDbSet(accounts);</example>
        public static IReturnsResult<TRepository> ReturnsDbSet<TEntity, TRepository>(
                this IReturns<TRepository, DbSet<TEntity>> setup,
                IList<TEntity> entities)
            where TEntity : class
            where TRepository : class
        {
            var mockSet = CreateMockSet(entities);
            return setup.Returns(mockSet.Object);
        }
    }
}
