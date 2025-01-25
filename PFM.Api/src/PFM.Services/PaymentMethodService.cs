using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Enumerations;
using PFM.Api.Contracts.PaymentMethod;

namespace PFM.Services;

public interface IPaymentMethodService : IBaseService
{
    IList<PaymentMethodList> GetPaymentMethods();
}
    
public class PaymentMethodService(IMapper mapper, IPaymentMethodRepository paymentMethodRepository) : IPaymentMethodService
{
    public IList<PaymentMethodList> GetPaymentMethods()
    {
        var paymentMethods = paymentMethodRepository.GetList().ToList();

        var mappedPaymentMethods = paymentMethods.Select(mapper.Map<PaymentMethodList>).ToList() ?? throw new ArgumentNullException("paymentMethods.Select(x => mapper.Map<PaymentMethodList>(x)).ToList()");

        mappedPaymentMethods.ForEach(paymentMethod =>
        {
            paymentMethod.HasBeenAlreadyDebitedOption = paymentMethod.Id == (int)PaymentMethod.Transfer || paymentMethod.Id == (int)PaymentMethod.CB;
            paymentMethod.HasAtmWithdrawOption = paymentMethod.Id == (int)PaymentMethod.Cash;
            paymentMethod.HasInternalAccountOption = paymentMethod.Id == (int)PaymentMethod.InternalTransfer;
        });

        return mappedPaymentMethods;
    }
}