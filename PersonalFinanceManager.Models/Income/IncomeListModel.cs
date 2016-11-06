using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Models.Income
{
    public class IncomeListModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("IncomeAccount")]
        public string AccountName { get; set; }

        public string AccountCurrencySymbol { get; set; }

        [LocalizedDisplayName("IncomeCost")]
        public decimal Cost { get; set; }

        [LocalizedDisplayName("IncomeDescription")]
        public string Description { get; set; }

        public string DisplayedCost
        {
            get
            {
                return AccountCurrencySymbol + this.Cost;
            }
        }

        [LocalizedDisplayName("IncomeDateIncome")]
        [Required]
        public DateTime DateIncome { get; set; }

        public string DisplayedDateIncome
        {
            get
            {
                return this.DateIncome.ToString("dd/MM/yyyy");
            }
        }
        
    }
}