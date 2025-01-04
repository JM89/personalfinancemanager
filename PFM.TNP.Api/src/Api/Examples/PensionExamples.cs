using PFM.Pension.Api.Contracts.Pension;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Examples;

public class CreatePensionExample: IMultipleExamplesProvider<PensionCreateRequest>
{
    public IEnumerable<SwaggerExample<PensionCreateRequest>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Simple Example with initial port and contribution",
            "Set only mandatory properties with pot and contribution",
            new PensionCreateRequest()
            {
                SchemeName = "Simple Example",
                CurrencyId = 1,
                CountryId = 1,
                CurrentPot = 100,
                CurrentContribution = 20,
                LoginUrl = "http://example.com"
            });
    }
}