using PFM.TNP.Api.Contracts.IncomeTaxReport;
using PFM.TNP.Api.Contracts.Pension;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Examples;

public class SaveIncomeTaxReportExample: IMultipleExamplesProvider<IncomeTaxReportSaveRequest>
{
    public IEnumerable<SwaggerExample<IncomeTaxReportSaveRequest>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Simple Example with initial port and contribution",
            "Set only mandatory properties with pot and contribution",
            new IncomeTaxReportSaveRequest()
            {
                PayDay =  DateOnly.FromDateTime(DateTime.UtcNow),
                CurrencyId = 1,
                CountryId = 1,
                IncomeTaxPaid = 200,
                NationalInsurancePaid = 300,
                TaxableIncome = 2000
            });
    }
}