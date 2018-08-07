using System.ComponentModel.DataAnnotations;

namespace PFM.DataAccessLayer.Entities
{
    public class Country : PersistedEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
