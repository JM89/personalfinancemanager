using System.Collections.Generic;

namespace PersonalFinanceManager.Models.Helpers.Chart
{
    public class ChartDataset
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<ChartValue> Values { get; set; }

        public string SumValues { get; set; }

        public ChartDataset()
        {
            Values = new List<ChartValue>();
        }
    }
}