using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _db;

        public PaymentMethodRepository(ApplicationDbContext db) 
        {
            this._db = db;
        }

        public DbSet<PaymentMethodModel> GetList()
        {
            return _db.Set(typeof(PaymentMethodModel)).Cast<PaymentMethodModel>();
        }
    }
}
