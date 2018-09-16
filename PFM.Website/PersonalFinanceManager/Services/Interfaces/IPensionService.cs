using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Models.Pension;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IPensionService : IBaseService
    {
        IList<PensionListModel> GetPensions(string userId);

        void CreatePension(PensionEditModel pensionEditModel);

        PensionEditModel GetById(int id);

        void EditPension(PensionEditModel pensionEditModel);

        void DeletePension(int id);
    }
}