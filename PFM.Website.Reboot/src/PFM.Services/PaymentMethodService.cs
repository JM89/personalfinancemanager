using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Api.Contracts.PaymentMethod;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class PaymentMethodService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IPaymentMethodApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<PaymentMethodListModel>> GetAll()
        {
            var apiResponse = await api.GetList();
            var response = ReadApiResponse<List<PaymentMethodList>>(apiResponse) ?? new List<PaymentMethodList>();
            return response.Select(Mapper.Map<PaymentMethodListModel>).ToList();
        }
    }
}

