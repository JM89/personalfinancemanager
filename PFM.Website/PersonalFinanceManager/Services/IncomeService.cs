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

        public IncomeService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void CreateIncomes(List<IncomeEditModel> models)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>).ToList();
                httpClient.Post($"/Income/CreateIncomes", dto);
            }
        }

        public void CreateIncome(IncomeEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>(model);
                httpClient.Post($"/Income/Create", dto);
            }
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            IList<IncomeListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Income.IncomeList>($"/Income/GetList/{accountId}");
                result = response.Select(AutoMapper.Mapper.Map<IncomeListModel>).ToList();
            }
            return result;
        }

        public IncomeEditModel GetById(int id)
        {
            IncomeEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Income.IncomeDetails>($"/Income/Get/{id}");
                result = AutoMapper.Mapper.Map<IncomeEditModel>(response);
            }
            return result;
        }

        public void EditIncome(IncomeEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Income.IncomeDetails>(model);
                httpClient.Put($"/Income/Edit/{model.Id}", dto);
            }
        }

        public void DeleteIncome(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/Income/Delete/{id}");
            }
        }
    }
}