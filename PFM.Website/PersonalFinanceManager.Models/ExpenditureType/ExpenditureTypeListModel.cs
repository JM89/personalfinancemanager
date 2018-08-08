using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Models.Shared;

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

        public string TooltipResourceName => "ExpenditureTypeCantBeDeleted";

        [LocalizedDisplayName("ExpenditureTypeShowOnDashboard")]
        public bool ShowOnDashboard { get; set; }
    }
}