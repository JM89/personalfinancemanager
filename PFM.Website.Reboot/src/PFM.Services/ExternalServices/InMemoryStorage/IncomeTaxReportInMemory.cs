using Newtonsoft.Json;
using PFM.Api.Contracts.Shared;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.IncomeTaxReport;

namespace PFM.Services.ExternalServices.InMemoryStorage;

public class IncomeTaxReportInMemory : IIncomeTaxReportApi
{
    private readonly List<IncomeTaxReportDetails> _storage =
    [
        new()
        {
            Id = Guid.NewGuid(),
            UserId = "Local developer",
            CurrencyId = 1,
            CountryId = 1,
            TaxableIncome = 42000,
            IncomeTaxPaid = 6200,
            NationalInsurancePaid = 3200,
            PayDay = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1))
        }
    ];

    public Task<ApiResponse> GetList(string userId)
    {
        var result = _storage
            .Where(item => item.UserId == userId)
            .Select(item => new IncomeTaxReportList
            {
                CanBeDeleted = true,
                Id = item.Id,
                TaxableIncome = item.TaxableIncome,
                IncomeTaxPaid = item.IncomeTaxPaid,
                NationalInsurancePaid = item.NationalInsurancePaid,
                PayDay = item.PayDay
            })
            .ToList();
        return Task.FromResult(new ApiResponse((object)JsonConvert.SerializeObject(result)));
    }

    public Task<ApiResponse> Get(Guid id)
    {
        var result = _storage.SingleOrDefault(item => item.Id == id);
        return Task.FromResult(new ApiResponse((object)JsonConvert.SerializeObject(result)));
    }

    public Task<ApiResponse> Create(string userId, IncomeTaxReportSaveRequest request)
    {
        _storage.Add(new IncomeTaxReportDetails
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CurrencyId = request.CurrencyId,
            CountryId = request.CountryId,
            TaxableIncome = request.TaxableIncome,
            IncomeTaxPaid = request.IncomeTaxPaid,
            NationalInsurancePaid = request.NationalInsurancePaid,
            PayDay = request.PayDay
        });
        return Task.FromResult(new ApiResponse(true));
    }

    public Task<ApiResponse> Edit(Guid id, IncomeTaxReportSaveRequest request)
    {
        var item = _storage.SingleOrDefault(report => report.Id == id);
        if (item is null) return Task.FromResult(new ApiResponse(false));
        item.CurrencyId = request.CurrencyId;
        item.CountryId = request.CountryId;
        item.TaxableIncome = request.TaxableIncome;
        item.IncomeTaxPaid = request.IncomeTaxPaid;
        item.NationalInsurancePaid = request.NationalInsurancePaid;
        item.PayDay = request.PayDay;
        return Task.FromResult(new ApiResponse(true));
    }

    public Task<ApiResponse> Delete(Guid id)
    {
        var item = _storage.SingleOrDefault(report => report.Id == id);
        if (item is null) return Task.FromResult(new ApiResponse(false));
        _storage.Remove(item);
        return Task.FromResult(new ApiResponse(true));
    }
}
