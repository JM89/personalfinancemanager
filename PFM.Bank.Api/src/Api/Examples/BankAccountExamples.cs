using PFM.Bank.Api.Contracts.Account;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Examples;

public class BankAccountExample: IMultipleExamplesProvider<AccountDetails>
{
    public IEnumerable<SwaggerExample<AccountDetails>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Simple Example",
            "Set only mandatory properties",
            new AccountDetails
            {
                Id = 0,
                Name = "Bank Account",
                BankId = 1,
                CurrencyId = 1,
                CurrencyName = "GBP",
                CurrencySymbol = "£",
                InitialBalance = 0,
                CurrentBalance = 1990,
                IsSavingAccount = false
            });
    }
}