﻿using System.Drawing;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using PFM.Website.Models;
using PFM.Website.Services;
using PFM.Website.Utils;

namespace PFM.Website.Components.Dashboard
{
	public class LineChartExpenseOvertimeBase : ComponentBase
	{
        [Parameter]
        public int? AccountId { get; set; }

        private IEnumerable<string> Months;

        protected LineConfig? ChartConfig { get; set; }

        [Inject]
        protected MovementSummaryService MovementSummaryService { get; set; } = default!;

        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);
            await FetchData();
        }

        protected async Task FetchData()
        {
            if (!AccountId.HasValue)
                return;

            var model = await MovementSummaryService.GetExpenseOvertime(
                new MovementSummarySearchParamModel(AccountId.Value, Months)
                {
                    OptionalType = "Expenses"
                });

            if (model == null)
                return;

            if (ChartConfig == null)
            {
                ChartConfig = new LineConfig
                {
                    Options = new LineOptions
                    {
                        Responsive = true,
                        Title = new OptionsTitle
                        {
                            Display = true,
                            Text = "Expenses - Over 12 Months"
                        }
                    }
                };

                foreach (var m in MonthYearHelper.GetXLastMonths(12))
                {
                    ChartConfig.Data.Labels.Add(m);
                }
            }

            var dataset = new BarDataset<decimal>(model.Aggregates.Values.Select(x =>x.Actual))
            {
                Label = "Actual",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, ColorUtils.Blue)),
                BorderColor = ColorUtil.FromDrawingColor(ColorUtils.Blue),
                BorderWidth = 1
            };

            ChartConfig.Data.Datasets.Clear();
            ChartConfig.Data.Datasets.Add(dataset);
        }
    }
}

