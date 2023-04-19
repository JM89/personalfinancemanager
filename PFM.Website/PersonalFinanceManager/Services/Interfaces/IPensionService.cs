using PersonalFinanceManager.Models.Pension;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IPensionService : IBaseService
    {
        Task<IList<PensionListModel>> GetPensions(string userId);

        Task<bool> CreatePension(PensionEditModel pensionEditModel);

        Task<PensionEditModel> GetById(int id);

        Task<bool> EditPension(PensionEditModel pensionEditModel);

        Task<bool> DeletePension(int id);
    }
}