using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class ExpenditureTypeController : BaseController
    {
        private readonly IExpenditureTypeService _expenditureTypeService;

        public ExpenditureTypeController(IExpenditureTypeService expenditureTypeService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._expenditureTypeService = expenditureTypeService;
        }

        /// <summary>
        /// Return the list of expenditure types.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await _expenditureTypeService.GetExpenditureTypes();

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
        /// Create a new expenditure type.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExpenditureTypeEditModel expenditureTypeModel)
        {
            if (ModelState.IsValid)
            {
                await _expenditureTypeService.CreateExpenditureType(expenditureTypeModel);

                return RedirectToAction("Index");
            }

            return View(expenditureTypeModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Expenditure Type id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var expenditureTypeModel = await _expenditureTypeService.GetById(id.Value);

            if (expenditureTypeModel == null)
            {
                return HttpNotFound();
            }
            return View(expenditureTypeModel);
        }

        /// <summary>
        /// Update an existing expenditure type.
        /// </summary>
        /// <param name="expenditureTypeEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            if (ModelState.IsValid)
            {
                await _expenditureTypeService.EditExpenditureType(expenditureTypeEditModel);

                return RedirectToAction("Index");
            }
            return View(expenditureTypeEditModel);
        }

        /// <summary>
        /// Delete the expenditure type after confirmation.
        /// </summary>
        /// <param name="id">Expenditure type id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _expenditureTypeService.DeleteExpenditureType(id);

            return Content(Url.Action("Index"));
        }
    }
}
