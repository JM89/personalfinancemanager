using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Helpers;
using PersonalFinanceManager.Services.Interfaces;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class BankController : BaseController
    {
        private readonly IBankService _bankService;
        private readonly ICountryService _countryService;

        public BankController(IBankService bankService, ICountryService countryService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._bankService = bankService;
            this._countryService = countryService;
        }

        /// <summary>
        /// Return the list of banks.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await _bankService.GetBanks();

            return View(model);
        }

        /// <summary>
        /// Populate the list of countries for the Create / Edit form. 
        /// </summary>
        /// <param name="bankModel"></param>
        private async Task PopulateDropDownLists(BankEditModel bankModel)
        {
            bankModel.AvailableCountries = (await _countryService.GetCountries()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var bankEditModel = new BankEditModel();
            bankEditModel.DisplayIconFlags = DisplayIcon.DisplayUploader;
            await PopulateDropDownLists(bankEditModel);

            return View(bankEditModel);
        }

        /// <summary>
        /// Create a new bank.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BankEditModel bankEditModel, HttpPostedFileBase UploadImage)
        {
            await PopulateDropDownLists(bankEditModel);

            if (ModelState.IsValid)
            {
                bankEditModel.DisplayIconFlags = DisplayIcon.DisplayUploader;

                var upload = false;
                if (string.IsNullOrEmpty(bankEditModel.IconPath))
                {
                    upload = true;
                }
                else if (UploadImage != null)
                {
                    bankEditModel.DisplayIconFlags = DisplayIcon.DisplayUploader | DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;
                    upload = true;
                }

                if (upload)
                {
                    bankEditModel.IconPath = FileUpload.UploadFileToServer(UploadImage, "IconPath", Config.BankIconBasePath, Config.BankIconMaxSize, Config.BankIconAllowedExtensions);
                }

                bankEditModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

                await _bankService.CreateBank(bankEditModel);

                return RedirectToAction("Index");
            }
            return View(bankEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Bank id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bankModel = await _bankService.GetById(id.Value);

            if (bankModel == null)
            {
                return HttpNotFound();
            }

            bankModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

            await PopulateDropDownLists(bankModel);

            return View(bankModel);
        }

        /// <summary>
        /// Update an existing bank.
        /// </summary>
        /// <param name="bankEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BankEditModel bankEditModel, HttpPostedFileBase UploadImage)
        {
            await PopulateDropDownLists(bankEditModel);

            bankEditModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

            if (ModelState.IsValid)
            {
                if (UploadImage != null && UploadImage.FileName != bankEditModel.FileName)
                {
                    bankEditModel.DisplayIconFlags = bankEditModel.DisplayIconFlags | DisplayIcon.DisplayUploader;
                    bankEditModel.IconPath = FileUpload.UploadFileToServer(UploadImage, "IconPath", Config.BankIconBasePath, Config.BankIconMaxSize, Config.BankIconAllowedExtensions);
                }

                bankEditModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

                await _bankService.EditBank(bankEditModel);

                return RedirectToAction("Index");
            }
            return View(bankEditModel);
        }

        /// <summary>
        /// Delete the bank after confirmation.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _bankService.DeleteBank(id);

            return Content(Url.Action("Index"));
        }
    }
}
