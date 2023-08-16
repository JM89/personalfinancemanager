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

        protected PagedModel<ExpenseTypeDiffsModel> Models = new PagedModel<ExpenseTypeDiffsModel>(new List<ExpenseTypeDiffsModel>(), 0);
        protected PagingFooterBase<ExpenseTypeDiffsModel> PagingFooter { get; set; }
        protected ExpenseTypeModel? SelectedExpenseType;
        protected int PageSize = 5;

        private IEnumerable<string> Months;
        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;

        [Inject]
        protected MovementSummaryService MovementSummaryService { get; set; } = default!;

        [Inject]
        protected ExpenseTypeService ExpenseTypeService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);

            if (ExpenseTypeId.HasValue)
            {
                SelectedExpenseType = await ExpenseTypeService.GetById(ExpenseTypeId.Value);
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

            var model = await MovementSummaryService.GetExpenseTypePaged(skip, PageSize,
                new MovementSummarySearchParamModel(AccountId.Value, Months)
                {
                    OptionalType = "Expenses"
                });

            Models = model.PagedList;

            this.StateHasChanged();

            PagingFooter.RefreshModel(Models);
        }
    }
}

