using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class AtmWithdrawController : BaseController
    {
        private readonly IAtmWithdrawService _atmWithdrawService;

        public AtmWithdrawController(IAtmWithdrawService atmWithdrawService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._atmWithdrawService = atmWithdrawService;
        }

        /// <summary>
        /// Return the list of ATM withdraws.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var accountId = await GetCurrentAccount();

            await AccountBasicInfo();

            var model = (await _atmWithdrawService.GetAtmWithdrawsByAccountId(accountId)).OrderByDescending(x => x.DateExpenditure).ThenByDescending(x => x.Id).ToList();

            return View(model);
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            await AccountBasicInfo();

            var atmWithdrawModel = new AtmWithdrawEditModel();

            atmWithdrawModel.DateExpenditure = DateTime.Today;

            return View(atmWithdrawModel);
        }

        /// <summary>
        /// Create a new ATM withdraw.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = await GetCurrentAccount();
                atmWithdrawEditModel.AccountId = accountId;

                await _atmWithdrawService.CreateAtmWithdraw(atmWithdrawEditModel);

                return RedirectToAction("Index");
            }

            return View(atmWithdrawEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">ATM Withdraw id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            await AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var atmWithdrawModel = await _atmWithdrawService.GetById(id.Value);
            
            if (atmWithdrawModel == null)
            {
                return HttpNotFound();
            }

            return View(atmWithdrawModel);
        }

        /// <summary>
        /// Update an existing ATM withdraw.
        /// </summary>
        /// <param name="atmWithdrawEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = await GetCurrentAccount();
                atmWithdrawEditModel.AccountId = accountId;

                await _atmWithdrawService.EditAtmWithdraw(atmWithdrawEditModel);
                
                return RedirectToAction("Index");
            }
            return View(atmWithdrawEditModel);
        }

        /// <summary>
        /// Close an ATM withdraw: set current amount to 0 and don't display it anymore.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Close(int id)
        {
            await _atmWithdrawService.CloseAtmWithdraw(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete the ATM withdraw after confirmation.
        /// </summary>
        /// <param name="id">ATM withdraw id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _atmWithdrawService.DeleteAtmWithdraw(id);

            return Content(Url.Action("Index"));
        }

        public async Task<ActionResult> UndoDebit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _atmWithdrawService.ChangeDebitStatus(id.Value, false);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Debit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _atmWithdrawService.ChangeDebitStatus(id.Value, true);

            var accountId = GetCurrentAccount();

            return RedirectToAction("Index", new { accountId });
        }
    }
}
