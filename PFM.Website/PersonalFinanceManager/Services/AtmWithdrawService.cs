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
        private readonly IHttpClientExtended _httpClientExtended;

        public AtmWithdrawService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList>($"/AtmWithdraw/GetList/{accountId}");
            return response.Select(AutoMapper.Mapper.Map<AtmWithdrawListModel>).ToList();
        }

        public void CreateAtmWithdraws(List<AtmWithdrawEditModel> models)
        {
            var dto = models.Select(AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>).ToList();
            _httpClientExtended.Post($"/AtmWithdraw/CreateAtmWithdraws", dto);
        }

        public void CreateAtmWithdraw(AtmWithdrawEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>(model);
            _httpClientExtended.Post($"/AtmWithdraw/Create", dto);
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>($"/AtmWithdraw/Get/{id}");
            return AutoMapper.Mapper.Map<AtmWithdrawEditModel>(response);
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>(model);
            _httpClientExtended.Put($"/AtmWithdraw/Edit/{model.Id}", dto);
        }
        
        public void CloseAtmWithdraw(int id)
        {
            _httpClientExtended.Post($"/AtmWithdraw/CloseAtmWithdraw/{id}");
        }

        public void DeleteAtmWithdraw(int id)
        {
            _httpClientExtended.Delete($"/AtmWithdraw/Delete/{id}");
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            _httpClientExtended.Delete($"/AtmWithdraw/ChangeDebitStatus/{id}/{debitStatus}");
        }
    }
}