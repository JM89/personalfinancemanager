using PersonalFinanceManager.Models.Saving;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ISavingService : IBaseService
    {
        Task<IList<SavingListModel>> GetSavingsByAccountId(int accountId);

        Task<bool> CreateSaving(SavingEditModel savingEditModel);

        Task<SavingEditModel> GetById(int id);

        Task<bool> EditSaving(SavingEditModel savingEditModel);

        Task<bool> DeleteSaving(int id);
    }
}