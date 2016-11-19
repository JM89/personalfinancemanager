using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Models.Helpers.Chart
{
    public class ChartData
    {
        public IList<string> Labels { get; set; }

        public IList<ChartDataset> ChartDatasets { get; set; }
    }
}