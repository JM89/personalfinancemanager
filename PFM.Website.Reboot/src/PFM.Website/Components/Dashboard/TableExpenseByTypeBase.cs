using Microsoft.AspNetCore.Components;
using PFM.Website.Models;
using PFM.Website.Services;

namespace PFM.Website.Components.Dashboard
{
	public class TableExpenseByTypeBase : ComponentBase
	{
        [Parameter]
        public int? ExpenseTypeId { get; set; }

        [Parameter]
        public int? AccountId { get; set; }

        protected PagedModel<ExpenseListModel> models = new PagedModel<ExpenseListModel>(new List<ExpenseListModel>(), 0);
        protected PagingFooterBase<ExpenseListModel> PagingFooter { get; set; }
        protected ExpenseTypeModel? SelectedExpenseType;
        protected int PageSize = 5;

        [Inject]
        protected ExpenseService ExpenseService { get; set; } = default!;

        [Inject]
        protected ExpenseTypeService ExpenseTypeService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            if (ExpenseTypeId.HasValue)
            {
                SelectedExpenseType = await ExpenseTypeService.GetById(ExpenseTypeId.Value);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchExpenses(0);
            }
        }

        protected async Task FetchExpenses(int skip)
        {
            if (ExpenseTypeId == null)
            {
                return;
            }

            models = await ExpenseService.GetPaged(skip, PageSize, new ExpenseSearchParamModel()
            {
                AccountId = AccountId,
                ExpenseTypeId = ExpenseTypeId
            });
            this.StateHasChanged();

            PagingFooter.RefreshModel(models);
        }

        public async Task ReloadComponent(int expenseTypeId)
        {
            SelectedExpenseType = await ExpenseTypeService.GetById(expenseTypeId);
            await FetchExpenses(0);
        }
    }
}

