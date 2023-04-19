using PersonalFinanceManager.Models.PaymentMethod;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IPaymentMethodService : IBaseService
    {
        Task<IList<PaymentMethodListModel>> GetPaymentMethods();
    }
}