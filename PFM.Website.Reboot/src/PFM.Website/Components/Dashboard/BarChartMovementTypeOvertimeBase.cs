using System;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using PFM.Website.Models;
using PFM.Website.Services;
using PFM.Website.Utils;
using System.Drawing;
using ChartJs.Blazor.Common.Enums;

namespace PFM.Website.Components.Dashboard
{
	public class BarChartMovementTypeOvertimeBase : ComponentBase
    {
        [Parameter]
        public int? AccountId { get; set; }

        private IEnumerable<string> Months;

        protected BarConfig? ChartConfig { get; set; }

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

            var model = await MovementSummaryService.GetMovementTypeOverTimeModel(
                new MovementSummarySearchParamModel(AccountId.Value, Months));

            if (model == null)
                return;

            if (ChartConfig == null)
            {
                ChartConfig = new BarConfig
                {
                    Options = new BarOptions
                    {
                        Responsive = true,
                        Legend = new Legend
                        {
                            Position = Position.Top
                        },
                        Title = new OptionsTitle
                        {
                            Display = true,
                            Text = "Incomes/Outcomes - Over 6 Months"
                        }
                    }
                };

                foreach (var m in MonthYearHelper.GetXLastMonths(12))
                {
                    ChartConfig.Data.Labels.Add(m);
                }
            }

            var incomeDataset = model.Aggregates.Select(x => x.Value.IncomesAmount);
            var dataset1 = new BarDataset<decimal>(incomeDataset)
            {
                Label = "Incomes",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, ColorUtils.Green)),
                BorderColor = ColorUtil.FromDrawingColor(ColorUtils.Green),
                BorderWidth = 1
            };

            var outcomeDataset = model.Aggregates.Select(x => x.Value.ExpensesAmount);
            var dataset2 = new BarDataset<decimal>(outcomeDataset)
            {
                Label = "Outcomes",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, ColorUtils.Blue)),
                BorderColor = ColorUtil.FromDrawingColor(ColorUtils.Blue),
                BorderWidth = 1
            };

            var savingDataset = model.Aggregates.Select(x => x.Value.SavingsAmount);
            var dataset3 = new BarDataset<decimal>(savingDataset)
            {
                Label = "Savings",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, ColorUtils.Golden)),
                BorderColor = ColorUtil.FromDrawingColor(ColorUtils.Golden),
                BorderWidth = 1
            };

            ChartConfig.Data.Datasets.Clear();

            ChartConfig.Data.Datasets.Add(dataset1);
            ChartConfig.Data.Datasets.Add(dataset2);
            ChartConfig.Data.Datasets.Add(dataset3);
        }
    }
}

