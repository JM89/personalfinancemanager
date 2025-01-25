using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.PaymentMethod;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController(IPaymentMethodService api) : ControllerBase
    {
        [HttpGet("GetList")]
        public IEnumerable<PaymentMethodList> GetList()
        {
            return api.GetPaymentMethods();
        }
    }
}
