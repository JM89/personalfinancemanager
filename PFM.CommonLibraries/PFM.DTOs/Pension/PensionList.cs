using System;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Pension
{
    public class PensionList
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(StartDate);

        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(EndDate);

        public decimal CurrentPot { get; set; }

        public string CountryName { get; set; }
    }
}