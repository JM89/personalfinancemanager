using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Models.Bank;
using System.IO;
using PersonalFinanceManager.Helpers;
using System.Collections.Generic;
using PersonalFinanceManager.Utils;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class BankController : BaseController
    {
        private readonly IBankService _bankService;
        private readonly ICountryService _countryService;

        public BankController(IBankService bankService, ICountryService countryService, IBankAccountService bankAccountService) : base(bankAccountService)
        {
            this._bankService = bankService;
            this._countryService = countryService;
        }

        private const int MaxAttempt = 5;

        /// <summary>
        /// Return the list of banks.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _bankService.GetBanks();

            return View(model);
        }

        /// <summary>
        /// Populate the list of countries for the Create / Edit form. 
        /// </summary>
        /// <param name="accountModel"></param>
        private void PopulateDropDownLists(BankEditModel bankModel)
        {
            bankModel.AvailableCountries = _countryService.GetCountries().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var bankEditModel = new BankEditModel();
            bankEditModel.DisplayIconFlags = DisplayIcon.DisplayUploader;
            PopulateDropDownLists(bankEditModel);

            return View(bankEditModel);
        }

        /// <summary>
        /// Create a new bank.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankEditModel bankEditModel, HttpPostedFileBase UploadImage)
        {
            PopulateDropDownLists(bankEditModel);

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

                _bankService.CreateBank(bankEditModel);

                return RedirectToAction("Index");
            }
            return View(bankEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Bank id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bankModel = _bankService.GetById(id.Value);

            if (bankModel == null)
            {
                return HttpNotFound();
            }

            bankModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

            PopulateDropDownLists(bankModel);

            return View(bankModel);
        }

        /// <summary>
        /// Update an existing bank.
        /// </summary>
        /// <param name="bankEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BankEditModel bankEditModel, HttpPostedFileBase UploadImage)
        {
            PopulateDropDownLists(bankEditModel);

            bankEditModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

            if (ModelState.IsValid)
            {
                if (UploadImage != null && UploadImage.FileName != bankEditModel.FileName)
                {
                    bankEditModel.DisplayIconFlags = bankEditModel.DisplayIconFlags | DisplayIcon.DisplayUploader;
                    bankEditModel.IconPath = FileUpload.UploadFileToServer(UploadImage, "IconPath", Config.BankIconBasePath, Config.BankIconMaxSize, Config.BankIconAllowedExtensions);
                }

                bankEditModel.DisplayIconFlags = DisplayIcon.DisplayExistingIcon | DisplayIcon.DisplayIconPathPreview;

                _bankService.EditBank(bankEditModel);

                return RedirectToAction("Index");
            }
            return View(bankEditModel);
        }

        /// <summary>
        /// Show the details of the bank you are about to delete.
        /// </summary>
        /// <param name="id">Bank id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BankEditModel bankModel = _bankService.GetById(id.Value);

            if (bankModel == null)
            {
                return HttpNotFound();
            }
            return View(bankModel);
        }

        /// <summary>
        /// Delete the bank after confirmation.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _bankService.DeleteBank(id);

            return RedirectToAction("Index");
        }
    }
}
