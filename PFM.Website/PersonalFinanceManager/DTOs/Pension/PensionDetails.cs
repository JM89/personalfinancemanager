using System;
using System.Collections.Generic;

namespace PersonalFinanceManager.DTOs.Pension
{
    public class PensionDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal ContributionPercentage { get; set; }

        public decimal CompanyContributionPercentage { get; set; }

        public decimal CurrentPot { get; set; }

        public decimal Interest { get; set; }

        public string UserId { get; set; }

        public int CurrencyId { get; set; }

        public int CountryId { get; set; }
    }
}