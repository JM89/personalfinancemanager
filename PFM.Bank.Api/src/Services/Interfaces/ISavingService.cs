using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Api.Contracts.Saving;

namespace PFM.Services.Interfaces
{
    public interface ISavingService : IBaseService
    {
        IList<SavingList> GetSavingsByAccountId(int accountId);

        Task<bool> CreateSaving(SavingDetails savingDetails);

        SavingDetails GetById(int id);

        Task<bool> DeleteSaving(int id);
    }
}