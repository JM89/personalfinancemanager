using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Interfaces;
using System;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            IList<AtmWithdrawListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawList>($"/AtmWithdraw/GetList/{accountId}");
                result = response.Select(AutoMapper.Mapper.Map<AtmWithdrawListModel>).ToList();
            }
            return result;
        }

        public void CreateAtmWithdraws(List<AtmWithdrawEditModel> models)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = models.Select(AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawDetails>).ToList();
                httpClient.Post($"/AtmWithdraw/CreateAtmWithdraws", dto);
            }
        }

        public void CreateAtmWithdraw(AtmWithdrawEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawDetails>(model);
                httpClient.Post($"/AtmWithdraw/Create", dto);
            }
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            AtmWithdrawEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawDetails>($"/AtmWithdraw/Get/{id}");
                result = AutoMapper.Mapper.Map<AtmWithdrawEditModel>(response);
            }
            return result;
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawDetails>(model);
                httpClient.Put($"/AtmWithdraw/Edit/{model.Id}", dto);
            }
        }
        
        public void CloseAtmWithdraw(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Post($"/AtmWithdraw/CloseAtmWithdraw/{id}");
            }
        }

        public void DeleteAtmWithdraw(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/AtmWithdraw/Delete/{id}");
            }
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/AtmWithdraw/ChangeDebitStatus/{id}/{debitStatus}");
            }
        }
    }
}