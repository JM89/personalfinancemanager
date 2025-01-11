// ReSharper disable once CheckNamespace
namespace PFM.Bank.Api.Contracts.Account
{
    public class AccountDetails
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int BankId { get; set; }
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public bool IsSavingAccount { get; set; }

        public string BankWebsite { get; set; }

        public string BankIconPath { get; set; }

        public string OwnerId { get; set; }

        public bool IsFavorite { get; set; }
    }
}