using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class IncomeModel
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
        public string Description { get; set; }

        //[Required]
        //public int FrequencyId { get; set; }

        //// By month, by year
        //[ForeignKey("FrequencyId")]
        //public FrequencyModel Frequency { get; set; }

        [Required]
        public DateTime DateIncome { get; set; }

        //[Required]
        //public DateTime StartDate { get; set; }

        //[Required]
        //public DateTime? LastRunDate { get; set; }

        //public DateTime? EndDate { get; set; }

        //[DisplayName("Enabled")]
        //public bool IsEnabled { get; set; }
    }
}