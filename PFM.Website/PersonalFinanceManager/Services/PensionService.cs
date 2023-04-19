using PersonalFinanceManager.Models.Pension;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class PensionService : IPensionService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public PensionService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public IList<PensionListModel> GetPensions(string userId)
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Pension.PensionList>($"/Pension/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<PensionListModel>).ToList();
        }

        public void CreatePension(PensionEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Pension.PensionDetails>(model);
            _httpClientExtended.Post($"/Pension/Create", dto);
        }

        public PensionEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Pension.PensionDetails>($"/Pension/Get/{id}");
            return AutoMapper.Mapper.Map<PensionEditModel>(response);
        }

        public void EditPension(PensionEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Pension.PensionDetails>(model);
            _httpClientExtended.Put($"/Pension/Edit/{model.Id}", dto);
        }

        public void DeletePension(int id)
        {
            _httpClientExtended.Delete($"/Pension/Delete/{id}");
        }
    }
}