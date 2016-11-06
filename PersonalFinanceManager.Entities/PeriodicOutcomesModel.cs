using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PersonalFinanceManager.Entities
{
    public class PeriodicOutcomeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountModel Account { get; set; }
        
        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int TypeExpenditureId { get; set; }

        [ForeignKey("TypeExpenditureId")]
        public ExpenditureTypeModel TypeExpenditure { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int FrequencyId { get; set; }

        [ForeignKey("FrequencyId")]
        public FrequencyModel Frequency { get; set;}

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsEnabled { get; set; }

        //[DisplayName("Last Run Date")]
        //public DateTime? LastRunDate { get; set; }

        //[NotMapped]
        //public string DisplayedLastRunDate
        //{
        //    get
        //    {
        //        return (this.LastRunDate != null ? this.LastRunDate.Value.ToString("dd/MM/yyyy") : String.Empty);
        //    }
        //}

        //// For the direct debit which takes on 10 months not 12
        //public string ExcludeMonths { get; set; }

    }
}