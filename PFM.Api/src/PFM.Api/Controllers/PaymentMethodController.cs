using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.PaymentMethod;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _PaymentMethodService;

        public PaymentMethodController(IPaymentMethodService PaymentMethodService)
        {
            _PaymentMethodService = PaymentMethodService;
        }

        [HttpGet("GetList")]
        public IEnumerable<PaymentMethodList> GetList()
        {
            return _PaymentMethodService.GetPaymentMethods();
        }
    }
}
