using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PersonalFinanceManager.Models.Resources;

namespace PersonalFinanceManager.Models.PeriodicOutcome
{
    public class PeriodicOutcomeListModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeAccount")]
        public string AccountName { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeDescription")]
        public string Description { get; set; }
       
        [LocalizedDisplayName("PeriodicOutcomeTypeExpenditure")]
        public string TypeExpenditureName { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeFrequency")]
        public string FrequencyName { get; set; }

        [LocalizedDisplayName("PeriodicOutcomeCost")]
        public string DisplayedCost
        {
            get
            {
                return this.AccountCurrencySymbol + this.Cost;
            }
        }

        [LocalizedDisplayName("PeriodicOutcomeStartDate")]
        public string DisplayedStartDate
        {
            get
            {
                return this.StartDate.ToString("dd/MM/yyyy");
            }
        }

        [LocalizedDisplayName("PeriodicOutcomeEndDate")]
        public string DisplayedEndDate
        {
            get
            {
                return (this.EndDate.HasValue ? this.EndDate.Value.ToString("dd/MM/yyyy") : String.Empty);
            }
        }

        [LocalizedDisplayName("PeriodicOutcomeIsEnabled")]
        public bool IsEnabled { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Cost { get; set; }
        public string AccountCurrencySymbol { get; set; }
        public int TypeExpenditureId { get; set; }


        //// For the direct debit which takes on 10 months not 12
        //public string ExcludeMonths { get; set; }
    }
}