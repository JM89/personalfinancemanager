using Microsoft.AspNetCore.Components;
using PFM.Website.Models;
using PFM.Website.Services;
using PFM.Website.Utils;

namespace PFM.Website.Components.Dashboard
{
	public class TableExpenseTypeDiffsBase : ComponentBase
	{
        [Parameter]
        public int? ExpenseTypeId { get; set; }

        [Parameter]
        public int? AccountId { get; set; }

        [Parameter]
        public EventCallback<ExpenseTypeModel> CategorySelected { get; set; }

        protected PagedModel<ExpenseTypeDiffsModel> Models = new PagedModel<ExpenseTypeDiffsModel>(new List<ExpenseTypeDiffsModel>(), 0);
        protected PagingFooterBase<ExpenseTypeDiffsModel> PagingFooter { get; set; }
        protected List<ExpenseTypeModel> ExpenseTypes;
        protected int PageSize = 5;
        protected string CurrencySymbol = "";

        private IEnumerable<string> Months;
        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;

        [Inject]
        protected MovementSummaryService MovementSummaryService { get; set; } = default!;

        [Inject]
        protected ExpenseTypeService ExpenseTypeService { get; set; } = default!;

        [Inject]
        protected BankAccountService BankAccountService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);
            ExpenseTypes = (await ExpenseTypeService.GetAll()).Where(x => x.ShowOnDashboard).ToList();
            if (AccountId.HasValue)
            {
                CurrencySymbol = (await BankAccountService.GetById(AccountId.Value))?.CurrencySymbol ?? "";
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchData(0);
            }
        }

        protected async Task FetchData(int skip)
        {
            if (!AccountId.HasValue)
            {
                return;
            }

            var excludeCategories = (await ExpenseTypeService.GetAll())
                .Where(x => !x.ShowOnDashboard)
                .Select(x => x.Name)
                .ToList();

            var model = await MovementSummaryService.GetExpenseTypePaged(skip, PageSize,
                new MovementSummarySearchParamModel(AccountId.Value, Months)
                {
                    OptionalType = "Expenses",
                    ExcludedCategories = excludeCategories
             });

            Models = model.PagedList;

            this.StateHasChanged();

            PagingFooter.RefreshModel(Models);
        }

        protected string GetFontColorBasedOnDiff(decimal diff)
        {
            var color = "E0E0E0";

            if (diff > 0)
            {
                color = "FF3333";
            }

            if (diff < 0)
            {
                color = "009900";
            }

            return $"color:#{color}";
        }

        protected async Task CategoryClicked(string expenseTypeName)
        {
            var selectedCategory = ExpenseTypes.FirstOrDefault(x => x.Name == expenseTypeName);
            if (selectedCategory != null && selectedCategory.Id.HasValue)
            {
                await CategorySelected.InvokeAsync(selectedCategory);
            }
        }
    }
}

