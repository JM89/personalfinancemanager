namespace PersonalFinanceManager.Api.Contracts.Bank
{
    public class BankBranchDetails
    {
        public int Id { get; set; }

        public int BankId { get; set; }

        public string Name { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string PostCode { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }
    }
}
