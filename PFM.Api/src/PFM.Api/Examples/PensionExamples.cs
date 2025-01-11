using PFM.TNP.Api.Contracts.Pension;
using Swashbuckle.AspNetCore.Filters;

namespace PFM.Api.Examples;

public class SavePensionExample: IMultipleExamplesProvider<PensionSaveRequest>
{
    public IEnumerable<SwaggerExample<PensionSaveRequest>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Simple Example with initial port and contribution",
            "Set only mandatory properties with pot and contribution",
            new PensionSaveRequest()
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