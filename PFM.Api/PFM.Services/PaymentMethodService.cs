using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DTOs.PaymentMethod;
using PFM.DataAccessLayer.Enumerations;

namespace PFM.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            this._paymentMethodRepository = paymentMethodRepository;
        }

        public IList<PaymentMethodList> GetPaymentMethods()
        {
            var paymentMethods = _paymentMethodRepository.GetList().ToList();

            var mappedPaymentMethods = paymentMethods.Select(x => Mapper.Map<PaymentMethodList>(x)).ToList();

            mappedPaymentMethods.ForEach(paymentMethod =>
            {
                paymentMethod.HasBeenAlreadyDebitedOption = paymentMethod.Id == (int)PaymentMethod.Transfer || paymentMethod.Id == (int)PaymentMethod.CB;
                paymentMethod.HasAtmWithdrawOption = paymentMethod.Id == (int)PaymentMethod.Cash;
                paymentMethod.HasInternalAccountOption = paymentMethod.Id == (int)PaymentMethod.InternalTransfer;
            });

            return mappedPaymentMethods;
        }
    }
}