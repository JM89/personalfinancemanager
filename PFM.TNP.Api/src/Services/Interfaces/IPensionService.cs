using System;
using PFM.Pension.Api.Contracts.Pension;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPensionService 
    {
        Task<List<PensionList>> GetList(string userId);

        Task<bool> Create(PensionDetails pensionDetails, string userId);

        Task<PensionDetails> GetById(Guid id);

        Task<bool> Edit(PensionDetails pensionDetails, string userId);

        Task<bool> Delete(Guid id);
    }
}