namespace PFM.Services.Core.Exceptions
{
    public static class BusinessExceptionMessage
    {
        public static string AccountDuplicateName => "Duplicate account name for the same bank. Choose another name.";
        public static string BankDuplicateName => "Duplicate bank name. Choose another name.";
        public static string CountryDuplicateName => "Duplicate country name. Choose another name.";
        public static string CurrencyDuplicateName => "Duplicate currency name. Choose another name.";
    }
}
