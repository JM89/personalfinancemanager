using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Currency : PersistedEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }
    }
}