using System.ComponentModel.DataAnnotations;

namespace PFM.DataAccessLayer.Entities
{
    public class ExpenseType : PersistedEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string GraphColor { get; set; }

        [Required]
        public bool ShowOnDashboard { get; set; }

        public string User_Id { get; set; }
    }
}