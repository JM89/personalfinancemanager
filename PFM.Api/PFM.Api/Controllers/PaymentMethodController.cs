using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.PaymentMethod;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PaymentMethod")]
    public class PaymentMethodController : Controller
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
