using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Models.PaymentMethod;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PaymentMethodController : BaseController
    {
        private PaymentMethodService paymentMethodService = new PaymentMethodService();

        /// <summary>
        /// Return the list of payment methods.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = paymentMethodService.GetPaymentMethods();

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                paymentMethodService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
