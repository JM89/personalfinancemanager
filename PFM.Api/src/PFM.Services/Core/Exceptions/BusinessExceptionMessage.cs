using System;
using System.Collections.Generic;
using System.Text;

namespace PFM.Services.Core.Exceptions
{
    public static class BusinessExceptionMessage
    {
        public static string AccountDuplicateName => "Duplicate account name for the same bank. Choose another name.";
        public static string BankDuplicateName => "Duplicate bank name. Choose another name.";
        public static string CountryDuplicateName => "Duplicate country name. Choose another name.";
        public static string CurrencyDuplicateName => "Duplicate currency name. Choose another name.";
        public static string ExpenditureExceededAtmWithdrawBalance => "Movement is not allowed. The cost of the expenditure is more expensive than the available amount on the ATM withdraw current balance. Select another ATM Withdraw.";
        public static string ExpenditureTypeDuplicateGraphColor => "Duplicate expenditure type color. Choose another name.";
        public static string ExpenditureTypeDuplicateName => "Duplicate expenditure type name. Choose another name.";
        public static string FileUploadEmpty => "The file is empty.";
        public static string FileUploadHasNotBeenSelected => "No file has been selected. ";
        public static string FileUploadMaxSize => "The file should not exceed {0} bytes.";
        public static string FileUploadSameName => "A file with the same name already exists.";
        public static string FileUploadWrongExtensions => "The file extension {0} does not match the allowed extensions ({1}).";
        public static string PaymentMethodDuplicateName => "Duplicate payment method name. Choose another name.";
    }
}
