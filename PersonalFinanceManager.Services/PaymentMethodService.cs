using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.PaymentMethod;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private ApplicationDbContext _db;

        public PaymentMethodService(ApplicationDbContext db)
        {
            this._db = db;
        }
       
        public IList<PaymentMethodListModel> GetPaymentMethods()
        {
            var expenditures = _db.ExpenditureModels;

            var paymentMethods = _db.PaymentMethodModels.ToList();

            var paymentMethodsModel = paymentMethods.Select(x => Mapper.Map<PaymentMethodListModel>(x)).ToList();

            paymentMethodsModel.ForEach(paymentMethod =>
            {
                paymentMethod.HasBeenAlreadyDebitedOption = paymentMethod.Id == (int)PaymentMethod.Transfer || paymentMethod.Id == (int)PaymentMethod.CB;
                paymentMethod.HasAtmWithdrawOption = paymentMethod.Id == (int)PaymentMethod.Cash;
                paymentMethod.HasInternalAccountOption = paymentMethod.Id == (int)PaymentMethod.InternalTransfer;
            });

            return paymentMethodsModel;
        }
    }
}