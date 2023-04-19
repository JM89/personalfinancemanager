using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public IList<BankListModel> GetBanks()
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Bank.BankList>($"/Bank/GetList");
            return response.Select(AutoMapper.Mapper.Map<BankListModel>).ToList();
        }

        public void CreateBank(BankEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
            _httpClientExtended.Post($"/Bank/Create", dto);
        }

        public BankEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Bank.BankDetails>($"/Bank/Get/{id}");
            return AutoMapper.Mapper.Map<BankEditModel>(response);
        }

        public void EditBank(BankEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
            _httpClientExtended.Put($"/Bank/Edit/{model.Id}", dto);
        }

        public void DeleteBank(int id)
        {
            _httpClientExtended.Delete($"/Bank/Delete/{id}");
        }
    }
}