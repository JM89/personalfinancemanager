using PFM.DTOs.Shared;

namespace PFM.DTOs.ExpenseType
{
    public class ExpenseTypeList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GraphColor { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "ExpenseTypeCantBeDeleted";

        public bool ShowOnDashboard { get; set; }
    }
}