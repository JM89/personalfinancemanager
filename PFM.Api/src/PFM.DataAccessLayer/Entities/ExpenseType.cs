using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}