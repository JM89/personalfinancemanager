using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PFM.Utils.Helpers;

namespace PersonalFinanceManager.Models.AccountManagement
{
    public class ImportMovementModel
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
