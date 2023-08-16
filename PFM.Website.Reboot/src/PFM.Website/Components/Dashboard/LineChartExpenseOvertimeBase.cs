using System.Drawing;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
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

        [Inject]
        protected BankAccountService BankAccountService { get; set; } = default!;

        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);

            if (!AccountId.HasValue)
            {
                AccountId = (await BankAccountService.GetCurrentAccount(AccountId))?.Id;
            }

            if (AccountId.HasValue)
            {
                await FetchData();
            }
        }

        protected async Task FetchData()
        {
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
                        Legend = new Legend
                        {
                            Position = Position.Bottom
                        },
                        Title = new OptionsTitle
                        {
                            Display = false
                        }
                    }
                };

                foreach (var m in MonthYearHelper.GetXLastMonths(12))
                {
                    ChartConfig.Data.Labels.Add(m);
                }
            }

            var modelWithMissingValues = Months
                .GroupJoin(model.Aggregates, month => month, aggr => aggr.Key, (x, y) => y.DefaultIfEmpty());

            var actual = modelWithMissingValues
                .SelectMany(x => x.Select(y => y.Value?.Actual ?? 0).ToList());

            var dataset1 = new LineDataset<decimal>(actual)
            {
                Label = "Actual",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, ColorUtils.Blue)),
                BorderColor = ColorUtil.FromDrawingColor(ColorUtils.Blue),
                BorderWidth = 1
            };

            var expected = modelWithMissingValues
                .SelectMany(x => x.Select(y => y.Value?.Expected ?? 0).ToList());

            var dataset2 = new BarDataset<decimal>(expected)
            {
                Label = "Expected",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, ColorUtils.Grey)),
                BorderColor = ColorUtil.FromDrawingColor(ColorUtils.Grey),
                BorderWidth = 1
            };

            ChartConfig.Data.Datasets.Clear();
            ChartConfig.Data.Datasets.Add(dataset1);
            ChartConfig.Data.Datasets.Add(dataset2);
        }
    }
}

