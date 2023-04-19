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
            IList<BankListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Bank.BankList>($"/Bank/GetList");
            result = response.Select(AutoMapper.Mapper.Map<BankListModel>).ToList();
            return result;
        }

        public void CreateBank(BankEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
            _httpClientExtended.Post($"/Bank/Create", dto);
        }

        public BankEditModel GetById(int id)
        {
            BankEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Bank.BankDetails>($"/Bank/Get/{id}");
            result = AutoMapper.Mapper.Map<BankEditModel>(response);
            return result;
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