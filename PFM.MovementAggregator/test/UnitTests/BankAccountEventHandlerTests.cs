using FakeItEasy;
using PFM.Bank.Event.Contracts;
using PFM.MovementAggregator.Handlers;
using PFM.MovementAggregator.Persistence;
using PFM.MovementAggregator.Persistence.Entities;
using Xunit;

namespace UnitTests;

public class BankAccountEventHandlerTests
{
    private readonly IMovementAggregatorRepository _repository = A.Fake<IMovementAggregatorRepository>();
    private readonly Serilog.ILogger _logger = A.Fake<Serilog.ILogger>();

    [Fact]
    public async Task Credit_records_the_balance_increase_as_income()
    {
        var evt = new BankAccountCredited
        {
            Id = 42, UserId = "user-1", PreviousBalance = 50m, CurrentBalance = 75m,
            OperationDate = new DateTime(2026, 2, 1)
        };
        var handler = new BankAccountCreditedHandler(_logger, _repository);

        var result = await handler.HandleEvent(evt);

        Assert.True(result);
        A.CallTo(() => _repository.Merge(A<MovementAggregation>.That.Matches(m =>
            m.BankAccountId == 42 && m.MonthYearIdentifier == "202602" &&
            m.Type == "Incomes" && m.Category == "Incomes" && m.AggregatedAmount == 25m)))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(null, "Food", "Expenses", "Food")]
    [InlineData(null, null, "Expenses", "Unknown")]
    [InlineData("target-account", "Food", "Savings", "Savings")]
    public async Task Debit_records_the_balance_decrease_with_the_correct_category(
        string? targetAccount, string? movementType, string expectedType, string expectedCategory)
    {
        var evt = new BankAccountDebited
        {
            Id = 42, UserId = "user-1", PreviousBalance = 75m, CurrentBalance = 50m,
            OperationDate = new DateTime(2026, 2, 1), TargetBankAccount = targetAccount,
            MovementType = movementType
        };
        var handler = new BankAccountDebitedHandler(_logger, _repository);

        var result = await handler.HandleEvent(evt);

        Assert.True(result);
        A.CallTo(() => _repository.Merge(A<MovementAggregation>.That.Matches(m =>
            m.BankAccountId == 42 && m.MonthYearIdentifier == "202602" &&
            m.Type == expectedType && m.Category == expectedCategory && m.AggregatedAmount == 25m)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Repository_failures_are_propagated()
    {
        var evt = new BankAccountCredited { Id = 42, UserId = "user-1" };
        var error = new InvalidOperationException("merge failed");
        A.CallTo(() => _repository.Merge(A<MovementAggregation>._)).ThrowsAsync(error);
        var handler = new BankAccountCreditedHandler(_logger, _repository);

        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleEvent(evt));

        Assert.Same(error, actual);
    }
}
