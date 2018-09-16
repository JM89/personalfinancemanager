using System.Collections.Generic;
using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IPaymentMethodService : IBaseService
    {
        IList<PaymentMethodListModel> GetPaymentMethods();
    }
}