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
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Income.IncomeList>($"/Income/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<IncomeListModel>).ToList();
        }

        public IncomeEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Income.IncomeDetails>($"/Income/Get/{id}");
            return AutoMapper.Mapper.Map<IncomeEditModel>(response);
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