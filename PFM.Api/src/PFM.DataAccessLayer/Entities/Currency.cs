using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.DataAccessLayer.Entities
{
    public class Currency : PersistedEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }
    }
}