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
        protected BankAccountListModel? SelectedBankAccount;
        protected ExpenseTypeModel? SelectedExpenseType;
        protected int PageSize = 5;

        [Inject]
        protected ExpenseService ExpenseService { get; set; } = default!;

        [Inject]
        protected ExpenseTypeService ExpenseTypeService { get; set; } = default!;

        [Inject]
        protected BankAccountService BankAccountService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            SelectedBankAccount = await BankAccountService.GetCurrentAccount(AccountId);
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
            models = await ExpenseService.GetPaged(skip, PageSize, new ExpenseSearchParamModel()
            {
                AccountId = AccountId,
                ExpenseTypeId = ExpenseTypeId
            });
            this.StateHasChanged();

            PagingFooter.RefreshModel(models);
        }

        public async Task ReloadComponent()
        {
            if (ExpenseTypeId.HasValue)
            {
                SelectedExpenseType = await ExpenseTypeService.GetById(ExpenseTypeId.Value);
                await FetchExpenses(0);
            }
        }
    }
}

