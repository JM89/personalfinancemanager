using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._countryService = countryService;
        }

        /// <summary>
        /// Return the list of countries.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await _countryService.GetCountries();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var countryEditModel = new CountryEditModel();

            return View(countryEditModel);
        }

        /// <summary>
        /// Create a new country.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CountryEditModel countryEditModel)
        {
            if (ModelState.IsValid)
            {
                await _countryService.CreateCountry(countryEditModel);

                return RedirectToAction("Index");
            }

            return View(countryEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Country id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var countryModel = await _countryService.GetById(id.Value);
            
            if (countryModel == null)
            {
                return HttpNotFound();
            }

            return View(countryModel);
        }

        /// <summary>
        /// Update an existing country.
        /// </summary>
        /// <param name="countryEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CountryEditModel countryEditModel)
        {
            if (ModelState.IsValid)
            {
                await _countryService.EditCountry(countryEditModel);
                
                return RedirectToAction("Index");
            }
            return View(countryEditModel);
        }

        /// <summary>
        /// Delete the country after confirmation.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _countryService.DeleteCountry(id);

            return Content(Url.Action("Index"));
        }
    }
}
