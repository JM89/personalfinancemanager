using PFM.Models;
using PFM.Services;
using Microsoft.AspNetCore.Routing.Patterns;

namespace PFM.Website.Frontend;

/// <summary>
/// Enforces record-level authorization for the live website using the signed-in
/// user's record lists. It does not audit requests or protect direct backend API calls.
/// </summary>
/// <remarks>
/// If the backend needs to reuse these rules, prefer ASP.NET Core resource-based authorization handlers.
/// </remarks>
internal static class FrontendRecordAuthorizationFilter
{
    /// <summary>
    /// Validates applicable route IDs and references in bound request models before
    /// invoking the endpoint. Returns 404 for missing or inaccessible records.
    /// </summary>
    /// <param name="context">The authenticated request, including route values and bound arguments.</param>
    /// <param name="next">The next filter or endpoint, invoked only if the checks pass or none apply.</param>
    /// <returns>The downstream result, or a 404 response if access is denied.</returns>
    public static async ValueTask<object?> AuthorizeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;
        var services = http.RequestServices;
        int? accountId = ReadId(http, "accountId");
        int? id = ReadId(http, "id");
        Guid? recordId = ReadGuid(http, "id");
        var route = (RouteEndpoint)http.GetEndpoint()!;
        var resource = ((RoutePatternLiteralPart)route.RoutePattern.PathSegments[2].Parts[0]).Content;
        if (accountId is null && id is null && recordId is null && resource != "accounts")
            return await next(context);

        if (recordId is Guid tnpId)
        {
            var allowed = resource switch
            {
                "pensions" => (await services.GetRequiredService<PensionService>().GetAll()).Any(row => row.Id == tnpId),
                "income-tax-reports" => (await services.GetRequiredService<IncomeTaxReportService>().GetAll()).Any(row => row.Id == tnpId),
                _ => false
            };
            return allowed ? await next(context) : Results.NotFound();
        }

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

    private static Guid? ReadGuid(HttpContext context, string name) =>
        Guid.TryParse(context.Request.RouteValues[name]?.ToString(), out var id) ? id : null;
}
