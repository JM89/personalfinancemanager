namespace PFM.Models
{
	public class PaymentMethodListModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public bool HasBeenAlreadyDebitedOption { get; set; }

        public bool HasAtmWithdrawOption { get; set; }

        public string CssClass { get; set; }

        public string IconClass { get; set; }

        public bool HasInternalAccountOption { get; set; }
    }
}

