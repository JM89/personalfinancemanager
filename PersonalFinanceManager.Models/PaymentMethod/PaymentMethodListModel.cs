using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Models.PaymentMethod
{
    public class PaymentMethodListModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("PaymentMethodName")]
        public string Name { get; set; }

        [LocalizedDisplayName("PaymentMethodHasBeenAlreadyDebitedOption")]
        public bool HasBeenAlreadyDebitedOption { get; set; }

        [LocalizedDisplayName("PaymentMethodHasAtmWithdrawOption")]
        [Required]
        public bool HasAtmWithdrawOption { get; set; }

        [LocalizedDisplayName("PaymentMethodWidgetCssClass")]
        public string CssClass { get; set; }

        public bool HasInternalAccountOption { get; set; }
    }
}