using PFM.DataAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFM.DataAccessLayer.Entities
{
    public class HistoricMovement : PersistedEntity
    {
        public int SourceId { get; set; }

        public ObjectType SourceType { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal SourceOldAmount { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal SourceNewAmount { get; set; }

        public int? DestinationId { get; set; }

        public ObjectType DestinationType { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal DestinationOldAmount { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal DestinationNewAmount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Cost { get; set; }
    }
}