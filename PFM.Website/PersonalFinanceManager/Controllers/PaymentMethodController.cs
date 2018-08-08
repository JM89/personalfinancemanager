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
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class PaymentMethodController : BaseController
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService, IBankAccountService bankAccountService) : base(bankAccountService)
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
    }
}
