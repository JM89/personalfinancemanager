using System.Collections.Generic;
using PFM.DTOs.PaymentMethod;
using PFM.Services.Core;

namespace PFM.Services.Interfaces
{
    public interface IPaymentMethodService : IBaseService
    {
        IList<PaymentMethodList> GetPaymentMethods();
    }
}