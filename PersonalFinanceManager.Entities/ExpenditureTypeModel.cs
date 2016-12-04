using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Entities
{
    public class ExpenditureTypeModel : PersistedEntity
    {
        public string Name { get; set; }

        [DisplayName("Graph Color")]
        public string GraphColor { get; set; }

        public bool ShowOnDashboard { get; set; }
    }
}