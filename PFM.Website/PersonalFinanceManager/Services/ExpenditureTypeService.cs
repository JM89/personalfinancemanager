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
        private readonly IHttpClientExtended _httpClientExtended;

        public ExpenditureTypeService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            IList<ExpenditureTypeListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.ExpenseType.ExpenseTypeList>($"/ExpenseType/GetList");
            result = response.Select(AutoMapper.Mapper.Map<ExpenditureTypeListModel>).ToList();
            return result;
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            ExpenditureTypeEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>($"/ExpenseType/Get/{id}");
            result = AutoMapper.Mapper.Map<ExpenditureTypeEditModel>(response);
            return result;
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>(model);
            _httpClientExtended.Post($"/ExpenseType/Create", dto);
        }

        public void EditExpenditureType(ExpenditureTypeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>(model);
            _httpClientExtended.Put($"/ExpenseType/Edit/{model.Id}", dto);
        }

        public void DeleteExpenditureType(int id)
        {
            _httpClientExtended.Delete($"/ExpenseType/Delete/{id}");
        }
    }
}