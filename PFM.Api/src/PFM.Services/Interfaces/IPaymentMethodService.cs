using System.Collections.Generic;
using PFM.Api.Contracts.PaymentMethod;

namespace PFM.Services.Interfaces
{
    public interface IPaymentMethodService : IBaseService
    {
        IList<PaymentMethodList> GetPaymentMethods();
    }
}