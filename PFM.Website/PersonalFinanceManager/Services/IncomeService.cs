using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class IncomeService: IIncomeService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public IncomeService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public void CreateIncomes(List<IncomeEditModel> models)
        {
            var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>).ToList();
            _httpClientExtended.Post($"/Income/CreateIncomes", dto);
        }

        public void CreateIncome(IncomeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>(model);
            _httpClientExtended.Post($"/Income/Create", dto);
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            IList<IncomeListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Income.IncomeList>($"/Income/GetList/{accountId}");
            result = response.Select(AutoMapper.Mapper.Map<IncomeListModel>).ToList();
            return result;
        }

        public IncomeEditModel GetById(int id)
        {
            IncomeEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Income.IncomeDetails>($"/Income/Get/{id}");
            result = AutoMapper.Mapper.Map<IncomeEditModel>(response);
            return result;
        }

        public void EditIncome(IncomeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>(model);
            _httpClientExtended.Put($"/Income/Edit/{model.Id}", dto);
        }

        public void DeleteIncome(int id)
        {
            _httpClientExtended.Delete($"/Income/Delete/{id}");
        }
    }
}