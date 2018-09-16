using System.Collections.Generic;
using PersonalFinanceManager.Services.Core;
using PersonalFinanceManager.Models.Saving;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ISavingService : IBaseService
    {
        IList<SavingListModel> GetSavingsByAccountId(int accountId);

        void CreateSaving(SavingEditModel savingEditModel);

        SavingEditModel GetById(int id);

        void EditSaving(SavingEditModel savingEditModel);

        void DeleteSaving(int id);
    }
}