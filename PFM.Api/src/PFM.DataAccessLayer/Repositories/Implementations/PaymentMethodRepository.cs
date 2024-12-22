using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class PaymentMethodRepository(PFMContext db) : IPaymentMethodRepository
    {
        public DbSet<PaymentMethod> GetList()
        {
            return db.Set<PaymentMethod>();
        }
    }
}
