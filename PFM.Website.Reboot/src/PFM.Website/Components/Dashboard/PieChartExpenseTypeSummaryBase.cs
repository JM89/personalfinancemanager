using System.Linq;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Handlers;
using ChartJs.Blazor.Interop;
using ChartJs.Blazor.PieChart;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using PFM.Models;
using PFM.Services;
using PFM.Utils;
using PFM.Website.Utils;

namespace PFM.Website.Components.Dashboard
{
	public class PieChartExpenseTypeSummaryBase : ComponentBase
	{
        [Parameter]
        public int? AccountId { get; set; }

        protected PieConfig? ChartConfig { get; set; }

        [Inject]
        protected ExpenseTypeService ExpenseTypeService { get; set; } = default!;

        [Inject]
        protected MovementSummaryService MovementSummaryService { get; set; } = default!;

        [Parameter]
        public EventCallback<ExpenseTypeModel> CategorySelected { get; set; }

        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;
        private IEnumerable<string> Months;
        private IEnumerable<ExpenseTypeModel> Categories;

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);
            Categories = (await ExpenseTypeService.GetAll()).Where(x => x.ShowOnDashboard);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchData();
            }
        }

        protected async Task FetchData()
        {
            if (!AccountId.HasValue)
                return;

            var model = await MovementSummaryService.GetExpenseTypeSummary(
                new MovementSummarySearchParamModel(AccountId.Value, Months)
                {
                    OptionalType = "Expenses"
                });

            if (model == null)
                return;

            if (ChartConfig == null)
            {
                ChartConfig = new PieConfig
                {
                    Options = new PieOptions
                    {
                        Responsive = true,
                        Title = new OptionsTitle
                        {
                            Display = false
                        },
                        Legend = new Legend() { Display = false },
                        OnClick = new DelegateHandler<ChartMouseEvent>(async (e, j) => await OnClickHandler(e, j)),
                        AspectRatio = 1.25
                    }
                };
            }

            var modelWithMetadata = model.Aggregates.Join(Categories, x => x.Key, y => y.Name, (x,y) => new {
                Name = x.Key,
                Amount = x.Value,
                BorderColor = ColorUtils.ConvertColor(y.GraphColor),
                BackgroundColor = ColorUtils.ConvertColor(y.GraphColor, 150)
            });

            if (!modelWithMetadata.Any())
            {
                ChartConfig.Data.Labels.Add("No data");
                ChartConfig.Data.Datasets.Add(new PieDataset<decimal>(new List<decimal>() { 1 }));
                return;
            }

            foreach (string cat in modelWithMetadata.Select(x => x.Name))
            {
                ChartConfig.Data.Labels.Add(cat);
            }

            var dataset = new PieDataset<decimal>(modelWithMetadata.Select(x => x.Amount))
            {
                BackgroundColor = modelWithMetadata.Select(x => x.BackgroundColor).ToArray(),
                BorderColor = modelWithMetadata.Select(x => x.BorderColor).ToArray(),
            };

            ChartConfig.Data.Datasets.Add(dataset);

            this.StateHasChanged();
        }

        public async Task OnClickHandler(JObject mouseEvent, JArray activeElements)
        {
            foreach (JObject elem in activeElements)
            {
                foreach (JProperty prop in elem.GetValue("_model"))
                {
                    if (prop.Name.Equals("label"))
                    {
                        var selectedCategory = Categories.FirstOrDefault(x => x.Name == prop.Value.ToString());
                        if (selectedCategory != null && selectedCategory.Id.HasValue)
                        {
                            await CategorySelected.InvokeAsync(selectedCategory);
                        }
                    }
                }
            }
        }
    }
}

