using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Entities
{
    public class ChallengeModel : PersistedEntity
    {
        public string Objective { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? ObjectivePrice { get; set; }
        public decimal? ResultPrice { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool Achieved { get; set; }
        public IList<CommentModel> Comments { get; set; }
    }
}
