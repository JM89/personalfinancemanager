using System;
using System.Collections.Generic;

namespace PFM.DTOs.Tax
{
    public class TaxDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string UserId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public int CountryId { get; set; }

        public int FrequenceOptionId { get; set; }

        public int? Frequence { get; set; }

        public int TaxTypeId { get; set; }
    }
}