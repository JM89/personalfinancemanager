using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Pension;
using System.Collections.Generic;
using System;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class PensionService : IPensionService
    {
        public IList<PensionListModel> GetPensions(string userId)
        {
            IList<PensionListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PersonalFinanceManager.DTOs.Pension.PensionList>($"/Pension/GetList/{userId}");
                result = response.Select(AutoMapper.Mapper.Map<PensionListModel>).ToList();
            }
            return result;
        }

        public void CreatePension(PensionEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.Pension.PensionDetails>(model);
                httpClient.Post($"/Pension/Create", dto);
            }
        }

        public PensionEditModel GetById(int id)
        {
            PensionEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PersonalFinanceManager.DTOs.Pension.PensionDetails>($"/Pension/Get/{id}");
                result = AutoMapper.Mapper.Map<PensionEditModel>(response);
            }
            return result;
        }

        public void EditPension(PensionEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.Pension.PensionDetails>(model);
                httpClient.Put($"/Pension/Edit/{model.Id}", dto);
            }
        }

        public void DeletePension(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Pension/Delete/{id}");
            }
        }
    }
}