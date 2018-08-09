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
        public void CreateSaving(SavingEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Saving.SavingDetails>(model);
                httpClient.Post($"/Saving/Create", dto);
            }
        }

        public void DeleteSaving(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Saving/Delete/{id}");
            }
        }

        public void EditSaving(SavingEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Saving.SavingDetails>(model);
                httpClient.Put($"/Saving/Edit/{model.Id}", dto);
            }
        }

        public SavingEditModel GetById(int id)
        {
            SavingEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Saving.SavingDetails>($"/Saving/Get/{id}");
                result = AutoMapper.Mapper.Map<SavingEditModel>(response);
            }
            return result;
        }

        public IList<SavingListModel> GetSavingsByAccountId(int accountId)
        {
            IList<SavingListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.DTOs.Saving.SavingList>($"/Saving/GetList/{accountId}");
                result = response.Select(AutoMapper.Mapper.Map<SavingListModel>).ToList();
            }
            return result;
        }
    }
}