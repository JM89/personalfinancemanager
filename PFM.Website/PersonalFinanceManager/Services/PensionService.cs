using PersonalFinanceManager.Models.Pension;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class PensionService : IPensionService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public PensionService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<PensionListModel>> GetPensions(string userId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Pension.PensionList>($"/Pension/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<PensionListModel>).ToList();
        }

        public async Task<bool> CreatePension(PensionEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Pension.PensionDetails>(model);
            return await _httpClientExtended.Post($"/Pension/Create", dto);
        }

        public async Task<PensionEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Pension.PensionDetails>($"/Pension/Get/{id}");
            return AutoMapper.Mapper.Map<PensionEditModel>(response);
        }

        public async Task<bool> EditPension(PensionEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Pension.PensionDetails>(model);
            return await _httpClientExtended.Put($"/Pension/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeletePension(int id)
        {
            return await _httpClientExtended.Delete($"/Pension/Delete/{id}");
        }
    }
}