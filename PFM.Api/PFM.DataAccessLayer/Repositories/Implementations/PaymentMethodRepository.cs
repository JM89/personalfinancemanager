using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly PFMContext _db;

        public PaymentMethodRepository(PFMContext db) 
        {
            this._db = db;
        }

        public DbSet<PaymentMethod> GetList()
        {
            return _db.Set<PaymentMethod>();
        }
    }
}
