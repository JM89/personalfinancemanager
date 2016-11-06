using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Country
{
    public class CountryEditModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("CountryName")]
        [Required]
        public string Name { get; set; }
   } 
}
