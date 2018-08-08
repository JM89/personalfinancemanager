using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Currency
{
    public class CurrencyEditModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("CurrencyName")]
        [Required]
        [MaxLength(3)]
        public string Name { get; set; }

        [LocalizedDisplayName("CurrencySymbol")]
        [Required]
        [MaxLength(3)]
        public string Symbol { get; set; }
   } 
}
