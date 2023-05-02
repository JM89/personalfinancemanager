namespace PFM.Api.Contracts.Account
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

        public string BankBranchName { get; set; }

        public string BankBranchAddressLine1 { get; set; }

        public string BankBranchAddressLine2 { get; set; }

        public string BankBranchPostCode { get; set; }

        public string BankBranchCity { get; set; }

        public string BankBranchPhoneNumber { get; set; }
    }
}