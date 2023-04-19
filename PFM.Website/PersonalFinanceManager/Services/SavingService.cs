using System;
using System.Collections.Generic;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Saving;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class SavingService : ISavingService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public SavingService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public void CreateSaving(SavingEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Saving.SavingDetails>(model);
            _httpClientExtended.Post($"/Saving/Create", dto);
        }

        public void DeleteSaving(int id)
        {
            _httpClientExtended.Delete($"/Saving/Delete/{id}");
        }

        public void EditSaving(SavingEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Saving.SavingDetails>(model);
            _httpClientExtended.Put($"/Saving/Edit/{model.Id}", dto);
        }

        public SavingEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Saving.SavingDetails>($"/Saving/Get/{id}");
            return AutoMapper.Mapper.Map<SavingEditModel>(response);
        }

        public IList<SavingListModel> GetSavingsByAccountId(int accountId)
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Saving.SavingList>($"/Saving/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<SavingListModel>).ToList();
        }
    }
}