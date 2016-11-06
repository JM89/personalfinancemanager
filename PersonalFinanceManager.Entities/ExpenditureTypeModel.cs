using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class ExpenditureTypeModel
    {
        [Required(ErrorMessage = "The Expenditure Type field is required.")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Graph Color")]
        public string GraphColor { get; set; }

        public bool ShowOnDashboard { get; set; }
    }
}