using System.ComponentModel.DataAnnotations;
using PFM.Models;
using PFM.Services;
using PFM.Services.Utils;

namespace PFM.Website.Frontend;

public static class FrontendEndpoints
{
    public static void MapFrontendEndpoints(this WebApplication app, bool offline)
    {
        var api = app.MapGroup("/api/frontend").RequireAuthorization();
        if (!offline)
            api.AddEndpointFilter(FrontendRecordAuthorizationFilter.AuthorizeAsync);

        api.MapGet("/session", (HttpContext context) => Results.Ok(new { name = context.User.Identity?.Name }));
        app.MapGet("/api/frontend/mode", () => Results.Ok(new { offline }));
        api.MapGet("/accounts", (BankAccountService service) => service.GetAll());
        api.MapPost("/accounts", async (BankAccountEditModel model, BankAccountService service) =>
            await Save(model, () => service.Create(model)));
        api.MapDelete("/accounts/{id:int}", async (int id, BankAccountService service) => Result(await service.Delete(id)));
        api.MapPost("/accounts/{id:int}/favorite", async (int id, BankAccountService service) => Result(await service.SetAsFavorite(id)));

        api.MapGet("/expense-types", (ExpenseTypeService service) => service.GetAll());
        api.MapGet("/expense-types/{id:int}", (int id, ExpenseTypeService service) => service.GetById(id));
        api.MapPost("/expense-types", async (ExpenseTypeModel model, ExpenseTypeService service) =>
            await Save(model, () => service.Create(model)));
        api.MapPut("/expense-types/{id:int}", async (int id, ExpenseTypeModel model, ExpenseTypeService service) =>
            await Save(model, () => service.Edit(id, model)));
        api.MapDelete("/expense-types/{id:int}", async (int id, ExpenseTypeService service, BankAccountService accounts, ExpenseService expenses) =>
        {
            foreach (var account in await accounts.GetAll())
                if ((await expenses.GetAll(new ExpenseSearchParamModel { AccountId = account.Id, ExpenseTypeId = id })).Count != 0)
                    return Results.Conflict("This expense type is in use.");
            return Result(await service.Delete(id));
        });

        api.MapGet("/banks", (BankService service) => service.GetAll());
        api.MapGet("/banks/{id:int}", (int id, BankService service) => service.GetById(id));
        api.MapPost("/banks", async (HttpRequest request, BankService service) => await SaveBank(request, service, null));
        api.MapPut("/banks/{id:int}", async (int id, HttpRequest request, BankService service) => await SaveBank(request, service, id));
        api.MapDelete("/banks/{id:int}", async (int id, BankService service, BankAccountService accounts) =>
        {
            foreach (var account in await accounts.GetAll())
                if ((await accounts.GetById(account.Id)).BankId == id)
                    return Results.Conflict("This bank is in use by a bank account.");
            return Result(await service.Delete(id));
        });
        api.MapGet("/countries", (CountryService service) => service.GetAll());
        api.MapGet("/currencies", (CurrencyService service) => service.GetAll());
        api.MapGet("/payment-methods", (PaymentMethodService service) => service.GetAll());

        api.MapGet("/accounts/{accountId:int}/expenses", (int accountId, ExpenseService service) =>
            service.GetAll(new ExpenseSearchParamModel { AccountId = accountId }));
        api.MapPost("/accounts/{accountId:int}/expenses", async (int accountId, ExpenseEditModel model, ExpenseService service) =>
            await Save(model, () => service.Create(accountId, model)));
        api.MapDelete("/expenses/{id:int}", async (int id, ExpenseService service) => Result(await service.Delete(id)));
        api.MapPost("/expenses/{id:int}/debit", async (int id, DebitStatus status, ExpenseService service) =>
            Result(await service.ChangeDebitStatus(id, status.Current)));

        api.MapGet("/accounts/{accountId:int}/incomes", (int accountId, IncomeService service) => service.GetAll(accountId));
        api.MapPost("/accounts/{accountId:int}/incomes", async (int accountId, IncomeEditModel model, IncomeService service) =>
            await Save(model, () => service.Create(accountId, model)));
        api.MapDelete("/incomes/{id:int}", async (int id, IncomeService service) => Result(await service.Delete(id)));

        api.MapGet("/accounts/{accountId:int}/savings", (int accountId, SavingService service) => service.GetAll(accountId));
        api.MapPost("/accounts/{accountId:int}/savings", async (int accountId, SavingEditModel model, SavingService service) =>
            await Save(model, () => service.Create(accountId, model)));
        api.MapDelete("/savings/{id:int}", async (int id, SavingService service) => Result(await service.Delete(id)));

        api.MapGet("/accounts/{accountId:int}/withdrawals", (int accountId, AtmWithdrawService service) => service.GetAll(accountId));
        api.MapPost("/accounts/{accountId:int}/withdrawals", async (int accountId, AtmWithdrawEditModel model, AtmWithdrawService service) =>
            await Save(model, () => service.Create(accountId, model)));
        api.MapDelete("/withdrawals/{id:int}", async (int id, AtmWithdrawService service) => Result(await service.Delete(id)));
        api.MapPost("/withdrawals/{id:int}/debit", async (int id, DebitStatus status, AtmWithdrawService service) =>
            Result(await service.ChangeDebitStatus(id, status.Current)));
        api.MapPost("/withdrawals/{id:int}/close", async (int id, AtmWithdrawService service) => Result(await service.CloseAtmWithdraw(id)));

        api.MapGet("/accounts/{accountId:int}/budgets", (int accountId, BudgetPlanService service) => service.GetAll(accountId));
        api.MapGet("/budgets/{id:int}", (int id, BudgetPlanService service) => service.GetById(id));
        api.MapPost("/accounts/{accountId:int}/budgets", async (int accountId, BudgetPlanEditModel model, BudgetPlanService service) =>
            await Save(model, () => service.Create(accountId, model)));
        api.MapPut("/accounts/{accountId:int}/budgets/{id:int}", async (int accountId, int id, BudgetPlanEditModel model, BudgetPlanService service) =>
        {
            model.Id = id;
            return await Save(model, () => service.Edit(accountId, model));
        });
        api.MapPost("/accounts/{accountId:int}/budgets/{id:int}/start", async (int accountId, int id, BudgetPlanService service) =>
            Result(await service.Start(id, accountId)));
        api.MapPost("/budgets/{id:int}/stop", async (int id, BudgetPlanService service) => Result(await service.Stop(id)));
        api.MapGet("/accounts/{accountId:int}/budget-context", async (int accountId, int? excludeId, BudgetPlanService budgets, MovementSummaryService summaries) =>
        {
            var plans = await budgets.GetAll(accountId);
            var cutoff = plans.FirstOrDefault(p => p.Id == excludeId)?.StartDate ?? DateTime.UtcNow;
            var previous = plans.Where(p => p.Id != excludeId && p.StartDate.HasValue && p.StartDate < cutoff)
                .OrderByDescending(p => p.StartDate).FirstOrDefault();
            return Results.Ok(new
            {
                previous = previous is null ? null : await budgets.GetById(previous.Id),
                average = await summaries.GetExpenseTypeSummary(new MovementSummarySearchParamModel(accountId, MonthYearHelper.GetXLastMonths(6, false, false))),
                lastMonth = await summaries.GetExpenseTypeSummary(new MovementSummarySearchParamModel(accountId, MonthYearHelper.GetXLastMonths(1, false, false)))
            });
        });

        api.MapGet("/accounts/{accountId:int}/dashboard", async (int accountId, MovementSummaryService service) =>
        {
            var search = new MovementSummarySearchParamModel(accountId, MonthYearHelper.GetXLastMonths(12, true, false));
            return Results.Ok(new
            {
                categories = await service.GetExpenseTypeSummary(search),
                movements = await service.GetMovementTypeOverTimeModel(search),
                summary = await service.GetMovementTypeSummary(search),
                expenses = await service.GetExpenseOvertime(search),
                categoryDiffs = await service.GetExpenseTypePaged(0, 1000, new MovementSummarySearchParamModel(accountId, MonthYearHelper.GetXLastMonths(12, true, false)) { OptionalType = "Expenses" })
            });
        });
        api.MapGet("/accounts/{accountId:int}/dashboard/category", async (int accountId, string name, MovementSummaryService service, ExpenseService expenses) =>
        {
            var search = new MovementSummarySearchParamModel(accountId, MonthYearHelper.GetXLastMonths(12, true, false))
                { OptionalCategory = name, OptionalType = "Expenses" };
            var overtime = await service.GetExpenseTypeOvertime(search);
            var items = await expenses.GetAll(new ExpenseSearchParamModel { AccountId = accountId });
            return Results.Ok(new { aggregates = overtime.Aggregates, expenses = items.Where(x => x.ExpenseTypeName == name) });
        });
    }

    private static async Task<IResult> Save<T>(T model, Func<Task<bool>> save)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model!, new ValidationContext(model!), errors, true))
            return Results.ValidationProblem(errors.GroupBy(e => e.MemberNames.FirstOrDefault() ?? "")
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage ?? "Invalid value.").ToArray()));
        return Result(await save());
    }

    private static IResult Result(bool success) => success ? Results.NoContent() : Results.Problem("The operation could not be completed.", statusCode: 400);

    private static async Task<IResult> SaveBank(HttpRequest request, BankService service, int? id)
    {
        if (!request.HasFormContentType) return Results.BadRequest("Expected form data.");
        var form = await request.ReadFormAsync();
        if (!int.TryParse(form["countryId"], out var countryId)) return Results.BadRequest("A country is required.");
        var model = new BankEditModel
        {
            Name = form["name"].ToString(),
            CountryId = countryId,
            Website = form["website"].ToString(),
            GeneralEnquiryPhoneNumber = form["generalEnquiryPhoneNumber"].ToString(),
            IconPath = form["iconPath"].ToString()
        };
        var file = form.Files.GetFile("icon");
        if (!id.HasValue && file is null) return Results.BadRequest("A bank icon is required.");
        if (file is { Length: > 10485760 }) return Results.BadRequest("The bank icon must not exceed 10 MB.");
        using var icon = file?.OpenReadStream();
        return await Save(model, () => id.HasValue ? service.Edit(id.Value, model, icon) : service.Create(model, icon!));
    }

    private sealed record DebitStatus(bool Current);

}
