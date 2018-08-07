using PFM.Utils.Helpers;
using System;
using System.Collections.Generic;

namespace PFM.DTOs.AccountManagement
{
    public class ImportMovementList
    {
        public int AccountId { get; set; }

        public decimal AccountCurrentBalance { get; set; }

        public string DisplayedAccountCurrentBalance
            => DecimalFormatHelper.GetDisplayDecimalValue(AccountCurrentBalance, AccountCurrencySymbol);

        public string AccountCurrencySymbol { get; set; }

        public DateTime? LastMovementRegistered { get; set; }

        public string DisplayedLastMovementRegistered
            => DateTimeFormatHelper.GetDisplayDateValue(LastMovementRegistered);

        public List<SelectListItem> PaymentMethods { get; set; }

        public List<SelectListItem> ImportTypes { get; set; }

        public List<SelectListItem> ExpenseTypes { get; set; }

        public List<string> SelectedValues { get; set; }

        public List<MovementPropertyDefinition> MovementPropertyDefinitions { get; set; }
    }
}
