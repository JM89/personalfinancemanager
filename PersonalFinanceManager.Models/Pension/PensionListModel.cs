using System;
using System.ComponentModel.DataAnnotations;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;

namespace PersonalFinanceManager.Models.Pension
{
    public class PensionListModel
    {
        [LocalizedDisplayName("PensionId")]
        public int Id { get; set; }

        [LocalizedDisplayName("PensionDescription")]
        public string Description { get; set; }

        [LocalizedDisplayName("PensionStartDate")]
        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        [LocalizedDisplayName("PensionEndDate")]
        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        [LocalizedDisplayName("PensionCurrentPot")]
        public decimal CurrentPot { get; set; }
    }
}