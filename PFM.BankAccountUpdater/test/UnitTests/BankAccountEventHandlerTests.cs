using FakeItEasy;
using PFM.Bank.Event.Contracts;
using PFM.BankAccountUpdater.Handlers;
using PFM.BankAccountUpdater.Services.Interfaces;
using Xunit;

namespace UnitTests;

public class BankAccountEventHandlerTests
{
    private readonly IBankAccountService _service = A.Fake<IBankAccountService>();
    private readonly Serilog.ILogger _logger = A.Fake<Serilog.ILogger>();

    [Fact]
    public async Task Credit_updates_the_account_with_the_current_balance()
    {
        var evt = new BankAccountCredited { Id = 42, UserId = "user-1", PreviousBalance = 50m, CurrentBalance = 75m };
        var handler = new BankAccountCreditedHandler(_service, _logger);

        var result = await handler.HandleEvent(evt);

        Assert.True(result);
        A.CallTo(() => _service.UpdateBalance(42, "user-1", 75m)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Debit_updates_the_account_with_the_current_balance()
    {
        var evt = new BankAccountDebited { Id = 42, UserId = "user-1", PreviousBalance = 75m, CurrentBalance = 50m };
        var handler = new BankAccountDebitedHandler(_service, _logger);

        var result = await handler.HandleEvent(evt);

        Assert.True(result);
        A.CallTo(() => _service.UpdateBalance(42, "user-1", 50m)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Balance_update_failures_are_propagated()
    {
        var evt = new BankAccountCredited { Id = 42, UserId = "user-1", CurrentBalance = 75m };
        var error = new InvalidOperationException("update failed");
        A.CallTo(() => _service.UpdateBalance(42, "user-1", 75m)).ThrowsAsync(error);
        var handler = new BankAccountCreditedHandler(_service, _logger);

        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleEvent(evt));

        Assert.Same(error, actual);
    }
}
