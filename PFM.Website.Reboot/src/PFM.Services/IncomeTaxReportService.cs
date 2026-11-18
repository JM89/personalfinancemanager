using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.IncomeTaxReport;

namespace PFM.Services;

public class IncomeTaxReportService(
    Serilog.ILogger logger,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,
    IIncomeTaxReportApi api)
    : CoreService(logger, mapper, httpContextAccessor)
{
    public async Task<List<IncomeTaxReportList>> GetAll()
    {
        var response = await api.GetList(GetCurrentUserId());
        return ReadApiResponse<List<IncomeTaxReportList>>(response) ?? [];
    }

    public async Task<IncomeTaxReportDetails?> GetById(Guid id)
    {
        var response = await api.Get(id);
        return ReadApiResponse<IncomeTaxReportDetails>(response);
    }

    public async Task<bool> Create(IncomeTaxReportSaveRequest request)
    {
        var response = await api.Create(GetCurrentUserId(), request);
        return ReadApiResponse<bool>(response);
    }

    public async Task<bool> Edit(Guid id, IncomeTaxReportSaveRequest request)
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
