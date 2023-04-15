using System.Collections.Generic;
using PFM.Api.Contracts.Saving;

namespace PFM.Services.Interfaces
{
    public interface ISavingService : IBaseService
    {
        IList<SavingList> GetSavingsByAccountId(int accountId);

        void CreateSaving(SavingDetails savingDetails);

        SavingDetails GetById(int id);

        void EditSaving(SavingDetails savingDetails);

        void DeleteSaving(int id);
    }
}