using System.Net;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._currencyService = currencyService;
        }

        /// <summary>
        /// Return the list of currencies.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _currencyService.GetCurrencies();

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
        public ActionResult Create(CurrencyEditModel currencyModel)
        {
            if (ModelState.IsValid)
            {
                _currencyService.CreateCurrency(currencyModel);

                return RedirectToAction("Index");
            }

            return View(currencyModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Currency id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currencyModel = _currencyService.GetById(id.Value);

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
        public ActionResult Edit(CurrencyEditModel currencyEditModel)
        {
            if (ModelState.IsValid)
            {
                _currencyService.EditCurrency(currencyEditModel);

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
        public ActionResult DeleteConfirmed(int id)
        {
            _currencyService.DeleteCurrency(id);

            return Content(Url.Action("Index"));
        }
    }
}
