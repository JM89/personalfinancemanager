using AutoMapper;
using PFM.Api.Contracts.PaymentMethod;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class PaymentMethodService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ApplicationSettings settings,
        IPaymentMethodApi api)
        : CoreService(logger, mapper, httpContextAccessor, settings)
    {
        public async Task<List<PaymentMethodListModel>> GetAll()
        {
            var apiResponse = await api.GetList();
            var response = ReadApiResponse<List<PaymentMethodList>>(apiResponse) ?? new List<PaymentMethodList>();
            return response.Select(Mapper.Map<PaymentMethodListModel>).ToList();
        }
    }
}

