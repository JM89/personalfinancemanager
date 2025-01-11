using PFM.Bank.Api.Contracts.Bank;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Examples;

public class BankExample: IMultipleExamplesProvider<BankDetails>
{
    public IEnumerable<SwaggerExample<BankDetails>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Simple Example",
            "Set only mandatory properties",
            new BankDetails
            {
                Name = "Bank",
                CountryId = 1,
                Website = "https://www.bank.com",
                GeneralEnquiryPhoneNumber = "555-555-5555",
                DisplayIconFlags = DisplayIcon.None,
                IconPath = "."
            });
    }
}