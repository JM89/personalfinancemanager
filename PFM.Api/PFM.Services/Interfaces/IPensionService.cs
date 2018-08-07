using System.Collections.Generic;
using PFM.DataAccessLayer.Entities;
using PFM.DTOs.Pension;
using PFM.Services.Core;

namespace PFM.Services.Interfaces
{
    public interface IPensionService : IBaseService
    {
        IList<Pension> GetPensions(string userId);

        void CreatePension(PensionDetails pensionDetails);

        PensionDetails GetById(int id);

        void EditPension(PensionDetails pensionDetails);

        void DeletePension(int id);
    }
}