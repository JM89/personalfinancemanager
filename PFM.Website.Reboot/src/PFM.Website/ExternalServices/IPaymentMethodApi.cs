using Api.Contracts.Shared;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IPaymentMethodApi
	{
        [Get("/api/PaymentMethod/GetList")]
        Task<ApiResponse> GetList();
    } 
}

