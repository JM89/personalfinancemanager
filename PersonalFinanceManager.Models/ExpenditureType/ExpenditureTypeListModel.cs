using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Models.ExpenditureType
{
    public class ExpenditureTypeListModel : ICanBeDeleted
    {
        public int Id { get; set; }

        [LocalizedDisplayName("ExpenditureTypeName")]
        public string Name { get; set; }

        [LocalizedDisplayName("ExpenditureTypeGraphColor")]
        public string GraphColor { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName
        {
            get
            {
                return "ExpenditureTypeCantBeDeleted";
            }
            set
            {

            }
        }


        [LocalizedDisplayName("ExpenditureTypeShowOnDashboard")]
        public bool ShowOnDashboard { get; set; }
    }
}