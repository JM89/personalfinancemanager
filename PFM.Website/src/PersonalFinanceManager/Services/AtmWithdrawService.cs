using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public AtmWithdrawService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<AtmWithdrawListModel>> GetAtmWithdrawsByAccountId(int accountId)
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList>($"/AtmWithdraw/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<AtmWithdrawListModel>).ToList();
        }

        public async Task<bool> CreateAtmWithdraws(List<AtmWithdrawEditModel> models)
        {
            var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>).ToList();
            return await _httpClientExtended.Post($"/AtmWithdraw/CreateAtmWithdraws", dto);
        }

        public async Task<bool> CreateAtmWithdraw(AtmWithdrawEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>(model);
            return await _httpClientExtended.Post($"/AtmWithdraw/Create", dto);
        }

        public async Task<AtmWithdrawEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>($"/AtmWithdraw/Get/{id}");
            return AutoMapper.Mapper.Map<AtmWithdrawEditModel>(response);
        }
        
        public async Task<bool> CloseAtmWithdraw(int id)
        {
            return await _httpClientExtended.Post($"/AtmWithdraw/CloseAtmWithdraw/{id}");
        }

        public async Task<bool> DeleteAtmWithdraw(int id)
        {
            return await _httpClientExtended.Delete($"/AtmWithdraw/Delete/{id}");
        }

        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await _httpClientExtended.Delete($"/AtmWithdraw/ChangeDebitStatus/{id}/{debitStatus}");
        }
    }
}