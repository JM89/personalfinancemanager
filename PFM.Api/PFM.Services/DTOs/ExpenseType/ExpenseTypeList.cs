using PFM.Services.DTOs.Shared;

namespace PFM.Services.DTOs.ExpenseType
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