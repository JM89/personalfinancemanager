using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.Pension;

namespace PFM.Services;

public class PensionService(
    Serilog.ILogger logger,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,
    IPensionApi api)
    : CoreService(logger, mapper, httpContextAccessor)
{
    public async Task<List<PensionList>> GetAll()
    {
        var response = await api.GetList(GetCurrentUserId());
        return ReadApiResponse<List<PensionList>>(response) ?? [];
    }

    public async Task<PensionDetails?> GetById(Guid id)
    {
        var response = await api.Get(id);
        return ReadApiResponse<PensionDetails>(response);
    }

    public async Task<bool> Create(PensionSaveRequest request)
    {
        var response = await api.Create(GetCurrentUserId(), request);
        return ReadApiResponse<bool>(response);
    }

    public async Task<bool> Edit(Guid id, PensionSaveRequest request)
    {
        var response = await api.Edit(id, request);
        return ReadApiResponse<bool>(response);
    }

    public async Task<bool> Delete(Guid id)
    {
        var response = await api.Delete(id);
        return ReadApiResponse<bool>(response);
    }
}
