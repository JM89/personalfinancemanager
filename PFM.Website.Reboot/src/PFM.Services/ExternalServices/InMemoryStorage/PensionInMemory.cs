using Newtonsoft.Json;
using PFM.Api.Contracts.Shared;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.Pension;

namespace PFM.Services.ExternalServices.InMemoryStorage;

public class PensionInMemory : IPensionApi
{
    private readonly List<PensionDetails> _storage =
    [
        new()
        {
            Id = Guid.NewGuid(),
            UserId = "Local developer",
            SchemeName = "Workplace pension",
            LoginUrl = "https://example.com/pension",
            CurrencyId = 1,
            CountryId = 1,
            CurrentPot = 32500,
            CurrentContribution = 350,
            LastUpdated = DateTime.UtcNow.AddDays(-7)
        }
    ];

    public Task<ApiResponse> GetList(string userId)
    {
        var result = _storage
            .Where(item => item.UserId == userId)
            .Select(item => new PensionList
            {
                CanBeDeleted = true,
                Id = item.Id,
                SchemeName = item.SchemeName,
                CurrentPot = item.CurrentPot,
                CurrentContribution = item.CurrentContribution,
                LastUpdated = item.LastUpdated
            })
            .ToList();
        return Task.FromResult(new ApiResponse((object)JsonConvert.SerializeObject(result)));
    }

    public Task<ApiResponse> Get(Guid id)
    {
        var result = _storage.SingleOrDefault(item => item.Id == id);
        return Task.FromResult(new ApiResponse((object)JsonConvert.SerializeObject(result)));
    }

    public Task<ApiResponse> Create(string userId, PensionSaveRequest request)
    {
        _storage.Add(new PensionDetails
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            SchemeName = request.SchemeName,
            LoginUrl = request.LoginUrl,
            CurrencyId = request.CurrencyId,
            CountryId = request.CountryId,
            CurrentPot = request.CurrentPot,
            CurrentContribution = request.CurrentContribution,
            LastUpdated = DateTime.UtcNow
        });
        return Task.FromResult(new ApiResponse(true));
    }

    public Task<ApiResponse> Edit(Guid id, PensionSaveRequest request)
    {
        var item = _storage.SingleOrDefault(pension => pension.Id == id);
        if (item is null) return Task.FromResult(new ApiResponse(false));
        item.SchemeName = request.SchemeName;
        item.LoginUrl = request.LoginUrl;
        item.CurrencyId = request.CurrencyId;
        item.CountryId = request.CountryId;
        item.CurrentPot = request.CurrentPot;
        item.CurrentContribution = request.CurrentContribution;
        item.LastUpdated = DateTime.UtcNow;
        return Task.FromResult(new ApiResponse(true));
    }

    public Task<ApiResponse> Delete(Guid id)
    {
        var item = _storage.SingleOrDefault(pension => pension.Id == id);
        if (item is null) return Task.FromResult(new ApiResponse(false));
        _storage.Remove(item);
        return Task.FromResult(new ApiResponse(true));
    }
}
