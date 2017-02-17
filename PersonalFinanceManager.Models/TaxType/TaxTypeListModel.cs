using PersonalFinanceManager.Models.Resources;

namespace PersonalFinanceManager.Models.TaxType
{
    public class TaxTypeListModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("TaxTypeName")]
        public string Name { get; set; }

        [LocalizedDisplayName("TaxTypeDescription")]
        public string Description { get; set; }
    }
}