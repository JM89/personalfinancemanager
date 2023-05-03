using PFM.Api.Contracts.Pension;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IPensionService : IBaseService
    {
        Task<IList<PensionList>> GetPensions(string userId);

        Task<bool> CreatePension(PensionDetails pensionDetails);

        Task<PensionDetails> GetById(int id);

        Task<bool> EditPension(PensionDetails pensionDetails);

        Task<bool> DeletePension(int id);
    }
}