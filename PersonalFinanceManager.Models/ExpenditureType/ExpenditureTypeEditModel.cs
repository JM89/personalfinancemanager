using PersonalFinanceManager.Models.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Models.ExpenditureType
{
    public class ExpenditureTypeEditModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("ExpenditureTypeName")]
        [Required]
        public string Name { get; set; }

        [LocalizedDisplayName("ExpenditureTypeGraphColor")]
        [Required]
        public string GraphColor { get; set; }

        [LocalizedDisplayName("ExpenditureTypeShowOnDashboard")]
        [Required]
        public bool ShowOnDashboard { get; set; }
    }
}