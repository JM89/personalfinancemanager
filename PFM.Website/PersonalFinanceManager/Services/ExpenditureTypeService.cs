using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureTypeService : IExpenditureTypeService
    {
        private readonly Serilog.ILogger _logger;

        public ExpenditureTypeService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            IList<ExpenditureTypeListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.ExpenseType.ExpenseTypeList>($"/ExpenseType/GetList");
                result = response.Select(AutoMapper.Mapper.Map<ExpenditureTypeListModel>).ToList();
            }
            return result;
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            ExpenditureTypeEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>($"/ExpenseType/Get/{id}");
                result = AutoMapper.Mapper.Map<ExpenditureTypeEditModel>(response);
            }
            return result;
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>(model);
                httpClient.Post($"/ExpenseType/Create", dto);
            }
        }

        public void EditExpenditureType(ExpenditureTypeEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>(model);
                httpClient.Put($"/ExpenseType/Edit/{model.Id}", dto);
            }
        }

        public void DeleteExpenditureType(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/ExpenseType/Delete/{id}");
            }
        }
    }
}