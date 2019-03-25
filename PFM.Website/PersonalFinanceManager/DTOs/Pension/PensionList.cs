using System;

namespace PersonalFinanceManager.DTOs.Pension
{
    public class PensionList
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal CurrentPot { get; set; }

        public string CountryName { get; set; }
    }
}