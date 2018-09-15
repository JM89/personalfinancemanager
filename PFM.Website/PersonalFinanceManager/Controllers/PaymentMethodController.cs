using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PaymentMethodController : BaseController
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Return the list of payment methods.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _paymentMethodService.GetPaymentMethods();

            return View(model);
        }
    }
}
