using PersonalFinanceManager.Models.Saving;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class SavingService : ISavingService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public SavingService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<bool> CreateSaving(SavingEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Saving.SavingDetails>(model);
            return await _httpClientExtended.Post($"/Saving/Create", dto);
        }

        public async Task<bool> DeleteSaving(int id)
        {
            return await _httpClientExtended.Delete($"/Saving/Delete/{id}");
        }

        public async Task<SavingEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Saving.SavingDetails>($"/Saving/Get/{id}");
            return AutoMapper.Mapper.Map<SavingEditModel>(response);
        }

        public async Task<IList<SavingListModel>> GetSavingsByAccountId(int accountId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Saving.SavingList>($"/Saving/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<SavingListModel>).ToList();
        }
    }
}