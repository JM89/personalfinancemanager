﻿using PersonalFinanceManager.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class SplitByTypeOverTimeValueModel
    {
        public string MonthName { get; set; }

        public decimal Value { get; set; }
    }
}
