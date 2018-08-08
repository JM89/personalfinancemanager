using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;

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
        public TaxTypeController(ITaxTypeService taxTypeService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._taxTypeService = taxTypeService;
        }

        /// <summary>
        /// Return the list of tax types.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _taxTypeService.GetTaxTypes();
            return View(model);
        }
    }
}
