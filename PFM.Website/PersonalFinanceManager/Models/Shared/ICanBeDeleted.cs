﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.Shared
{
    public interface ICanBeDeleted
    {
        bool CanBeDeleted { get; set; }

        string TooltipResourceName { get; }
    }
}
