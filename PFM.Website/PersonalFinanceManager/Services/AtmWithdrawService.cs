using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        private readonly Serilog.ILogger _logger;

        public AtmWithdrawService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            IList<AtmWithdrawListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList>($"/AtmWithdraw/GetList/{accountId}");
                result = response.Select(AutoMapper.Mapper.Map<AtmWithdrawListModel>).ToList();
            }
            return result;
        }

        public void CreateAtmWithdraws(List<AtmWithdrawEditModel> models)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>).ToList();
                httpClient.Post($"/AtmWithdraw/CreateAtmWithdraws", dto);
            }
        }

        public void CreateAtmWithdraw(AtmWithdrawEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>(model);
                httpClient.Post($"/AtmWithdraw/Create", dto);
            }
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            AtmWithdrawEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>($"/AtmWithdraw/Get/{id}");
                result = AutoMapper.Mapper.Map<AtmWithdrawEditModel>(response);
            }
            return result;
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>(model);
                httpClient.Put($"/AtmWithdraw/Edit/{model.Id}", dto);
            }
        }
        
        public void CloseAtmWithdraw(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Post($"/AtmWithdraw/CloseAtmWithdraw/{id}");
            }
        }

        public void DeleteAtmWithdraw(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/AtmWithdraw/Delete/{id}");
            }
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/AtmWithdraw/ChangeDebitStatus/{id}/{debitStatus}");
            }
        }
    }
}