using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PersonalFinanceManager.Models.Resources;

namespace PersonalFinanceManager.Models.PeriodicOutcome
{
    public class PeriodicOutcomeEditModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeAccount")]
        [Required]
        public int AccountId { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeCost")]
        [Required]
        public decimal Cost { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeTypeExpenditure")]
        [Required]
        public int TypeExpenditureId { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeDescription")]
        [Required]
        public string Description { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeFrequency")]
        [Required]
        public int FrequencyId { get; set; }

        public IList<SelectListItem> AvailableAccounts { get; set; }

        public IList<SelectListItem> AvailableExpenditureTypes { get; set; }

        public IList<SelectListItem> AvailableFrequencies { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeStartDate")]
        [Required]
        public DateTime StartDate { get; set; }

        public string DisplayedStartDate
        {
            get
            {
                return this.StartDate.ToString("dd/MM/yyyy");
            }
        }
        
        //public string ExcludeMonths { get; set; }
    }
}