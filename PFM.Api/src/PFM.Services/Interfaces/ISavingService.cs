using PFM.Api.Contracts.Saving;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface ISavingService : IBaseService
    {
        Task<IList<SavingList>> GetSavingsByAccountId(int accountId);

        Task<bool> CreateSaving(SavingDetails savingDetails);

        Task<SavingDetails> GetById(int id);

        Task<bool> DeleteSaving(int id);
    }
}