using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        public IList<PaymentMethodListModel> GetPaymentMethods()
        {
            IList<PaymentMethodListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.Services.DTOs.PaymentMethod.PaymentMethodList>($"/PaymentMethod/GetList");
                result = response.Select(AutoMapper.Mapper.Map<PaymentMethodListModel>).ToList();
            }
            return result;
        }
    }
}