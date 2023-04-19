using PersonalFinanceManager.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    /// <summary>
    /// Tax Type MVC Controller.
    /// </summary>
    [Authorize]
    public class TaxTypeController : BaseController
    {
        private readonly ITaxTypeService _taxTypeService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taxTypeService"></param>
        /// <param name="bankAccountService"></param>
        public TaxTypeController(ITaxTypeService taxTypeService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._taxTypeService = taxTypeService;
        }

        /// <summary>
        /// Return the list of tax types.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await _taxTypeService.GetTaxTypes();
            return View(model);
        }
    }
}
