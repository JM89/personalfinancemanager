using System.Collections.Generic;
using PFM.Services.DTOs.PaymentMethod;

namespace PFM.Services.Interfaces
{
    public interface IPaymentMethodService : IBaseService
    {
        IList<PaymentMethodList> GetPaymentMethods();
    }
}