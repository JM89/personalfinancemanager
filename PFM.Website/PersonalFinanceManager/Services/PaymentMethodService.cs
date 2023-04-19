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

        public PaymentMethodService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<PaymentMethodListModel> GetPaymentMethods()
        {
            IList<PaymentMethodListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.PaymentMethod.PaymentMethodList>($"/PaymentMethod/GetList");
                result = response.Select(AutoMapper.Mapper.Map<PaymentMethodListModel>).ToList();
            }
            return result;
        }
    }
}