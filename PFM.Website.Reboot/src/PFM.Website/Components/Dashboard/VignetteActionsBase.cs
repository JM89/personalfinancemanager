using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using PFM.Website.Models;
using PFM.Website.Services;

namespace PFM.Website.Components.Dashboard
{
    public class VignetteActionsBase : ComponentBase
    {
        protected IList<BankAccountListModel> AvailableBankAccounts = new List<BankAccountListModel>();
        protected BankAccountListModel? SelectedBankAccount;

        [Parameter]
        public int? AccountId { get; set; }

        [Parameter]
        public EventCallback<int> AccountSelected { get; set; }

        [Inject]
        protected BankAccountService BankAccountService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            SelectedBankAccount = await BankAccountService.GetCurrentAccount(AccountId);
            AvailableBankAccounts = await BankAccountService.GetAll();
        }

        public async Task UpdateSelected(ChangeEventArgs referenced)
        {
            var id = Convert.ToInt32(referenced.Value);
            await AccountSelected.InvokeAsync(id);
        }
    }
}

