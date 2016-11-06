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
    public class IncomeEditModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("IncomeAccount")]
        public int AccountId { get; set; }

        [LocalizedDisplayName("IncomeCost")]
        [Required]
        public decimal Cost { get; set; }

        [Required]
        [LocalizedDisplayName("IncomeDescription")]
        public string Description { get; set; }
        
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