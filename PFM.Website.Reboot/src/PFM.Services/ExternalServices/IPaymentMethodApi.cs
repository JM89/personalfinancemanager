using PFM.Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices
{
	public interface IPaymentMethodApi
	{
        [Get("/api/PaymentMethod/GetList")]
        Task<ApiResponse> GetList();
    } 
}

