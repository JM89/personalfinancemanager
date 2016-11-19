using System.Collections.Generic;

namespace PersonalFinanceManager.Models.Helpers.Chart
{
    public class ChartDataset
    {
        public IList<string> Values { get; set; }

        public ChartDataset()
        {
            Values = new List<string>();
        }
    }
}