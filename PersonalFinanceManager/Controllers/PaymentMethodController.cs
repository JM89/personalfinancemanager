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
        private readonly PaymentMethodService _paymentMethodService;

        public PaymentMethodController(PaymentMethodService paymentMethodService)
        {
            this._paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Return the list of payment methods.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = _paymentMethodService.GetPaymentMethods();

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _paymentMethodService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
