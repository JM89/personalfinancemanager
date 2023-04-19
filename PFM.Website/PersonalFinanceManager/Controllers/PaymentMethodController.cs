using PersonalFinanceManager.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PaymentMethodController : BaseController
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Return the list of payment methods.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await _paymentMethodService.GetPaymentMethods();

            return View(model);
        }
    }
}
