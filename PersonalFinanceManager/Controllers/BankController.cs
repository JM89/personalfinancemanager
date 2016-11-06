using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Models.Bank;
using AutoMapper;
using System.IO;
using PersonalFinanceManager.Utils.Exceptions;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class BankController : BaseController
    {
        private BankService bankService = new BankService();
        private CountryService countryService = new CountryService();
        private const int MaxAttempt = 5;

        /// <summary>
        /// Return the list of banks.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = bankService.GetBanks();

            return View(model);
        }

        /// <summary>
        /// Populate the list of countries for the Create / Edit form. 
        /// </summary>
        /// <param name="accountModel"></param>
        private void PopulateDropDownLists(BankEditModel bankModel)
        {
            bankModel.AvailableCountries = countryService.GetCountries().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var bankEditModel = new BankEditModel();

            PopulateDropDownLists(bankEditModel);

            return View(bankEditModel);
        }

        /// <summary>
        /// Create a new bank.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CountryId,file,AttemptNumber,UrlPreview")] BankEditModel bankEditModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(bankEditModel.UrlPreview))
                {
                    UploadImageForPreview(bankEditModel, file);

                    PopulateDropDownLists(bankEditModel);

                    return View(bankEditModel);
                }

                try
                {
                    bankService.CreateBank(bankEditModel, Server.MapPath("~/"));
                }
                catch (BusinessException ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Description);

                    PopulateDropDownLists(bankEditModel);

                    return View(bankEditModel);
                }

                CleanPreviewFolder();

                return RedirectToAction("Index");
            }

            PopulateDropDownLists(bankEditModel);

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

            var bankModel = bankService.GetById(id.Value);

            if (bankModel == null)
            {
                return HttpNotFound();
            }

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
        public ActionResult Edit([Bind(Include = "Id,Name,CountryId,file,AttemptNumber,UrlPreview")] BankEditModel bankEditModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(bankEditModel.UrlPreview))
                {
                    UploadImageForPreview(bankEditModel, file);

                    PopulateDropDownLists(bankEditModel);

                    return View(bankEditModel);
                }

                bankService.EditBank(bankEditModel, Server.MapPath("~/"));

                CleanPreviewFolder();

                return RedirectToAction("Index");
            }

            PopulateDropDownLists(bankEditModel);

            return View(bankEditModel);
        }

        /// <summary>
        /// Upload the file in a preview location. 
        /// </summary>
        /// <param name="bankEditModel"></param>
        /// <param name="file"></param>
        public void UploadImageForPreview(BankEditModel bankEditModel, HttpPostedFileBase file)
        {
            if (bankEditModel.AttemptNumber == MaxAttempt)
            {
                bankEditModel.ErrorPreview = "Max attempt reached. Preview folder is now empty.";
                bankEditModel.UrlPreview = "";
                bankEditModel.FileName = "";

                CleanPreviewFolder();
               
            }
            else
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentType != "image/png" && file.ContentType != "image/jpeg")
                    {
                        bankEditModel.ErrorPreview = "Invalid file type";
                    }
                    else
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Resources/preview"), fileName);
                        file.SaveAs(path);
                        bankEditModel.FileName = fileName.Replace(".png", "").Replace(".jpg", "");
                        bankEditModel.UrlPreview = "/Resources/preview/" + fileName;
                        bankEditModel.AttemptNumber += 1;
                    }
                }
                else
                {
                    bankEditModel.ErrorPreview = "File is empty.";
                }
            }
        }

        private void CleanPreviewFolder()
        {
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Server.MapPath("~/Resources/preview"));

            foreach (FileInfo previewFile in downloadedMessageInfo.GetFiles())
            {
                previewFile.Delete();
            }
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

            BankEditModel bankModel = bankService.GetById(id.Value);

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
            bankService.DeleteBank(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bankService.Dispose();
                countryService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
