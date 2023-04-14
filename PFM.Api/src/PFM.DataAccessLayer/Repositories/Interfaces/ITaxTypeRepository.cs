using Microsoft.EntityFrameworkCore;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface ITaxTypeRepository
    {
        DbSet<TaxType> GetList();
    }
}
