using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class PersistedEntity
    {
        [Required]
        public Guid Id { get; set; }
    }
}
