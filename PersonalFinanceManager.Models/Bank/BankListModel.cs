using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Bank
{
    public class BankListModel : ICanBeDeleted
    {
        public int Id { get; set; }

        [Required]
        [LocalizedDisplayName("BankName")]
        public string Name { get; set; }

        [Required]
        [LocalizedDisplayName("BankCountry")]
        public string CountryName { get; set; }

        public string IconPath { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName
        {
            get
            {
                return "BankCantBeDeleted";
            }
            set
            {

            }
        }
    }
}
