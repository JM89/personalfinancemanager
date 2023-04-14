namespace PFM.Services.DTOs.Account
{
    public class AccountList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string BankName { get; set; }

        public string BankIconPath { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal CurrentBalance { get; set; }
       
        public bool CanBeDeleted { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsSavingAccount { get; set; }
    }
}
