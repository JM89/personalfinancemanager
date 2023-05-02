using System.Collections.Generic;
using PFM.Api.Contracts.Pension;

namespace PFM.Services.Interfaces
{
    public interface IPensionService : IBaseService
    {
        IList<PensionList> GetPensions(string userId);

        void CreatePension(PensionDetails pensionDetails);

        PensionDetails GetById(int id);

        void EditPension(PensionDetails pensionDetails);

        void DeletePension(int id);
    }
}