using System.Threading.Tasks;

namespace PFM.Services.Caches.Interfaces
{
    public interface IExpenseTypeCache
    {
        Task<string> GetById(int id);
    }
}
