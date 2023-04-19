using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<ExpenditureTypeListModel>> GetExpenditureTypes()
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.ExpenseType.ExpenseTypeList>($"/ExpenseType/GetList");
            return response.Select(AutoMapper.Mapper.Map<ExpenditureTypeListModel>).ToList();
        }

        public async Task<ExpenditureTypeEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>($"/ExpenseType/Get/{id}");
            return AutoMapper.Mapper.Map<ExpenditureTypeEditModel>(response);
        }

        public async Task<bool> CreateExpenditureType(ExpenditureTypeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>(model);
            return await _httpClientExtended.Post($"/ExpenseType/Create", dto);
        }

        public async Task<bool> EditExpenditureType(ExpenditureTypeEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>(model);
            return await _httpClientExtended.Put($"/ExpenseType/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeleteExpenditureType(int id)
        {
            return await _httpClientExtended.Delete($"/ExpenseType/Delete/{id}");
        }
    }
}