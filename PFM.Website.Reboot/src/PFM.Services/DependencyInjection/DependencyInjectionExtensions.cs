using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace PFM.Services.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        return services
            .AddSingleton<ExpenseTypeService>()
            .AddSingleton<BankService>()
            .AddSingleton<CountryService>()
            .AddSingleton<BankAccountService>()
            .AddSingleton<CurrencyService>()
            .AddSingleton<IncomeService>()
            .AddSingleton<SavingService>()
            .AddSingleton<AtmWithdrawService>()
            .AddSingleton<ExpenseService>()
            .AddSingleton<PaymentMethodService>()
            .AddSingleton<MovementSummaryService>()
            .AddSingleton<BudgetPlanService>();
    }
}