using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._currencyService = currencyService;
        }

        /// <summary>
        /// Return the list of currencies.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await _currencyService.GetCurrencies();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a new currency.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CurrencyEditModel currencyModel)
        {
            if (ModelState.IsValid)
            {
                await _currencyService.CreateCurrency(currencyModel);

                return RedirectToAction("Index");
            }

            return View(currencyModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Currency id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currencyModel = await _currencyService.GetById(id.Value);

            if (currencyModel == null)
            {
                return HttpNotFound();
            }

            return View(currencyModel);
        }

        /// <summary>
        /// Update an existing currency.
        /// </summary>
        /// <param name="currencyEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CurrencyEditModel currencyEditModel)
        {
            if (ModelState.IsValid)
            {
                await _currencyService.EditCurrency(currencyEditModel);

                return RedirectToAction("Index");
            }
            return View(currencyEditModel);
        }

        /// <summary>
        /// Delete the currency after confirmation.
        /// </summary>
        /// <param name="id">Currency id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _currencyService.DeleteCurrency(id);

            return Content(Url.Action("Index"));
        }
    }
}
