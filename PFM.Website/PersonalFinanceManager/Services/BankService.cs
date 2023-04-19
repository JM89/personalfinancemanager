using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class BankService : IBankService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public BankService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<BankListModel>> GetBanks()
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Bank.BankList>($"/Bank/GetList");
            return response.Select(AutoMapper.Mapper.Map<BankListModel>).ToList();
        }

        public async Task<bool> CreateBank(BankEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
            return await _httpClientExtended.Post($"/Bank/Create", dto);
        }

        public async Task<BankEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Bank.BankDetails>($"/Bank/Get/{id}");
            return AutoMapper.Mapper.Map<BankEditModel>(response);
        }

        public async Task<bool> EditBank(BankEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
            return await _httpClientExtended.Put($"/Bank/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeleteBank(int id)
        {
            return await _httpClientExtended.Delete($"/Bank/Delete/{id}");
        }
    }
}