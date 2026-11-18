using PFM.Models;
using PFM.Services;
using Microsoft.AspNetCore.Routing.Patterns;

namespace PFM.Website.Frontend;

internal static class FrontendRecordAccess
{
    public static async ValueTask<object?> Check(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;
        var services = http.RequestServices;
        int? accountId = ReadId(http, "accountId");
        int? id = ReadId(http, "id");
        var route = (RouteEndpoint)http.GetEndpoint()!;
        var resource = ((RoutePatternLiteralPart)route.RoutePattern.PathSegments[2].Parts[0]).Content;
        if (accountId is null && id is null && resource != "accounts")
            return await next(context);

        if (resource is "banks" or "expense-types")
        {
            var allowed = resource == "banks"
                ? (await services.GetRequiredService<BankService>().GetAll()).Any(row => row.Id == id)
                : (await services.GetRequiredService<ExpenseTypeService>().GetAll()).Any(row => row.Id == id);
            return allowed ? await next(context) : Results.NotFound();
        }

        var accounts = await services.GetRequiredService<BankAccountService>().GetAll();
        bool OwnsAccount(int target) => accounts.Any(account => account.Id == target);
        if (accountId is int source && !OwnsAccount(source)) return Results.NotFound();
        if (context.Arguments.OfType<SavingEditModel>().Any(model =>
                model.TargetInternalAccountId is int target && !OwnsAccount(target)) ||
            context.Arguments.OfType<ExpenseEditModel>().Any(model =>
                model.TargetInternalAccountId is int target && !OwnsAccount(target)))
            return Results.NotFound();
        if (accountId is int expenseAccount && context.Arguments.OfType<ExpenseEditModel>().FirstOrDefault() is { } expense &&
            (!(await services.GetRequiredService<ExpenseTypeService>().GetAll()).Any(row => row.Id == expense.ExpenseTypeId) ||
             expense.AtmWithdrawId is int withdrawal && !(await services.GetRequiredService<AtmWithdrawService>().GetAll(expenseAccount)).Any(row => row.Id == withdrawal)))
            return Results.NotFound();
        if (context.Arguments.OfType<BankAccountEditModel>().FirstOrDefault() is { } create &&
            !(await services.GetRequiredService<BankService>().GetAll()).Any(row => row.Id == create.BankId))
            return Results.NotFound();
        if (id is null) return await next(context);
        if (resource == "accounts" && accountId is null)
            return OwnsAccount(id.Value) ? await next(context) : Results.NotFound();
        foreach (var account in accounts.Where(account => accountId is null || account.Id == accountId))
        {
            var allowed = resource switch
            {
                "expenses" => (await services.GetRequiredService<ExpenseService>().GetAll(new ExpenseSearchParamModel { AccountId = account.Id })).Any(row => row.Id == id),
                "incomes" => (await services.GetRequiredService<IncomeService>().GetAll(account.Id)).Any(row => row.Id == id),
                "savings" => (await services.GetRequiredService<SavingService>().GetAll(account.Id)).Any(row => row.Id == id),
                "withdrawals" => (await services.GetRequiredService<AtmWithdrawService>().GetAll(account.Id)).Any(row => row.Id == id),
                "accounts" or "budgets" => (await services.GetRequiredService<BudgetPlanService>().GetAll(account.Id)).Any(row => row.Id == id),
                _ => false
            };
            if (allowed) return await next(context);
        }
        return Results.NotFound();
    }

    private static int? ReadId(HttpContext context, string name) =>
        int.TryParse(context.Request.RouteValues[name]?.ToString(), out var id) ? id : null;
}
