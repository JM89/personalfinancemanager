using AutoMapper;
using PFM.Api.Contracts.PaymentMethod;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class PaymentMethodService: CoreService
    {
        private readonly IPaymentMethodApi _api;

        public PaymentMethodService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, IPaymentMethodApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<PaymentMethodListModel>> GetAll()
        {
            var apiResponse = await _api.GetList();
            var response = ReadApiResponse<List<PaymentMethodList>>(apiResponse) ?? new List<PaymentMethodList>();
            return response.Select(_mapper.Map<PaymentMethodListModel>).ToList();
        }
    }
}

