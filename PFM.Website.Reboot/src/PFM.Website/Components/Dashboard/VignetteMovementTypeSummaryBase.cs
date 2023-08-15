using Microsoft.AspNetCore.Components;
using PFM.Website.Models;
using PFM.Website.Services;
using PFM.Website.Utils;

namespace PFM.Website.Components.Dashboard
{
	public class VignetteMovementTypeSummaryBase : ComponentBase
	{
        [Parameter]
        public int? AccountId { get; set; }

        [Inject]
        protected MovementSummaryService MovementSummaryService { get; set; } = default!;

        [Inject]
        protected BankAccountService BankAccountService { get; set; } = default!;

        private const int Duration = 12;
        private const bool IncludeCurrentMonth = true;
        private IEnumerable<string> Months;
        protected DashboardMovementTypeSummaryModel Model = new DashboardMovementTypeSummaryModel(0,0,0,0);
        protected string CurrencySymbol = "";

        protected override async Task OnInitializedAsync()
        {
            Months = MonthYearHelper.GetXLastMonths(Duration, IncludeCurrentMonth, false);

            if (AccountId.HasValue)
            {
                CurrencySymbol = (await BankAccountService.GetById(AccountId.Value))?.CurrencySymbol ?? "";

                Model = await MovementSummaryService.GetMovementTypeSummary(new MovementSummarySearchParamModel()
                {
                    BankAccountId = AccountId.Value,
                    MonthYearIdentifiers = Months.ToList()
                });
            }
        }
    }
}

