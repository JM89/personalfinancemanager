using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Country : PersistedEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
