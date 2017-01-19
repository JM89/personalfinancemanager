using PersonalFinanceManager.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Entities
{
    public class HistoricMovementModel : PersistedEntity
    {
        public int SourceId { get; set; }

        public ObjectType SourceType { get; set; }

        public decimal SourceOldAmount { get; set; }

        public decimal SourceNewAmount { get; set; }

        public int? DestinationId { get; set; }

        public ObjectType DestinationType { get; set; }

        public decimal DestinationOldAmount { get; set; }

        public decimal DestinationNewAmount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Cost { get; set; }
    }
}