namespace PFM.Models
{
	public class PaymentMethodListModel
	{
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool HasBeenAlreadyDebitedOption { get; set; }

        public bool HasAtmWithdrawOption { get; set; }

        public string CssClass { get; set; } = string.Empty;

        public string IconClass { get; set; } = string.Empty;

        public bool HasInternalAccountOption { get; set; }
    }
}

