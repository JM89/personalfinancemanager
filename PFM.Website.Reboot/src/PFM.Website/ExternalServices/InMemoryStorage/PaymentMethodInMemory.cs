using PFM.Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.PaymentMethod;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class PaymentMethodInMemory : IPaymentMethodApi
    {
        internal readonly IList<PaymentMethodList> Storage = new List<PaymentMethodList>() {
            new () { Id = 1, Name = "CB", CssClass = "primary", IconClass = "fa fa-credit-card", HasBeenAlreadyDebitedOption = true },
            new () { Id = 2, Name = "Cash", CssClass = "info", IconClass = "fa fa fa-money", HasBeenAlreadyDebitedOption = false },
            new () { Id = 3, Name = "Direct Debit", CssClass = "success", IconClass = "fa fa-credit-card-alt", HasBeenAlreadyDebitedOption = false },
            new () { Id = 4, Name = "Transfer", CssClass = "warning", IconClass = "fa fa-external-link", HasBeenAlreadyDebitedOption = false },
            new () { Id = 5, Name = "Internal Transfer", CssClass = "danger", IconClass = "fa fa-refresh", HasBeenAlreadyDebitedOption = false }
        };

        public async Task<ApiResponse> GetList()
        {
            var result = JsonConvert.SerializeObject(Storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }
    }
}

