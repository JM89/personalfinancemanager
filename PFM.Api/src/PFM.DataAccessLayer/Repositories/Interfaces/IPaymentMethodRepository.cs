using Microsoft.EntityFrameworkCore;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface IPaymentMethodRepository 
    {
        DbSet<PaymentMethod> GetList();
    }
}
