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

        public BankService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<BankListModel> GetBanks()
        {
            IList<BankListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Bank.BankList>($"/Bank/GetList");
                result = response.Select(AutoMapper.Mapper.Map<BankListModel>).ToList();
            }
            return result;
        }

        public void CreateBank(BankEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
                httpClient.Post($"/Bank/Create", dto);
            }
        }

        public BankEditModel GetById(int id)
        {
            BankEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Bank.BankDetails>($"/Bank/Get/{id}");
                result = AutoMapper.Mapper.Map<BankEditModel>(response);
            }
            return result;
        }

        public void EditBank(BankEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Bank.BankDetails>(model);
                httpClient.Put($"/Bank/Edit/{model.Id}", dto);
            }
        }

        public void DeleteBank(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/Bank/Delete/{id}");
            }
        }
    }
}