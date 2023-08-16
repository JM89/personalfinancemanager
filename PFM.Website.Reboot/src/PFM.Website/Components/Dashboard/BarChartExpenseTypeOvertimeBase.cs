using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using Microsoft.AspNetCore.Components;
using PFM.Website.Models;
using PFM.Website.Services;
using PFM.Website.Utils;

namespace PFM.Website.Components.Dashboard
{
	public class BarChartExpenseTypeOvertimeBase : ComponentBase
	{
        [Parameter]
        public int? ExpenseTypeId { get; set; }

        [Parameter]
        public int? AccountId { get; set; }

        private ExpenseTypeModel? SelectedExpenseType;
        private IEnumerable<string> Months;

        protected BarConfig? ChartConfig { get; set; }

        [Inject]
        protected ExpenseTypeService ExpenseTypeService { get; set; } = default!;

        [Inject]
        protected MovementSummaryService MovementSummaryService { get; set; } = default!;

        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);

            if (ExpenseTypeId.HasValue)
            {
                SelectedExpenseType = await ExpenseTypeService.GetById(ExpenseTypeId.Value);
                await FetchData();
            }
        }

        protected async Task FetchData()
        {
            if (!AccountId.HasValue && SelectedExpenseType == null)
                return;

            var model = await MovementSummaryService.GetExpenseTypeOvertime(
                new MovementSummarySearchParamModel(AccountId.Value, Months)
                {
                    OptionalCategory = SelectedExpenseType.Name,
                    OptionalType = "Expenses"
                });

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
                            Display = false
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

            var dataset = new BarDataset<decimal>(model.Aggregates.Values)
            {
                Label = SelectedExpenseType.Name,
                BackgroundColor = ColorUtils.ConvertColor(SelectedExpenseType.GraphColor, 150),
                BorderColor = ColorUtils.ConvertColor(SelectedExpenseType.GraphColor),
                BorderWidth = 1
            };

            ChartConfig.Data.Datasets.Clear();
            ChartConfig.Data.Datasets.Add(dataset);

            this.StateHasChanged();
        }

        public async Task ReloadComponent(int expenseTypeId)
        {
            SelectedExpenseType = await ExpenseTypeService.GetById(expenseTypeId);
            await FetchData();
        }
    }
}

