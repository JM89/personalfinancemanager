using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        public IList<PaymentMethodListModel> GetPaymentMethods()
        {
            throw new NotImplementedException();
        }
    }
}