using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public PaymentMethodService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public IList<PaymentMethodListModel> GetPaymentMethods()
        {
            IList<PaymentMethodListModel> result = null;
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.PaymentMethod.PaymentMethodList>($"/PaymentMethod/GetList");
            result = response.Select(AutoMapper.Mapper.Map<PaymentMethodListModel>).ToList();
            return result;
        }
    }
}