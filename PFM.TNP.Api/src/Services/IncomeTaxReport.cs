using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using PFM.TNP.Api.Contracts.IncomeTaxReport;

namespace Services;

public interface IIncomeTaxReportService 
{
    Task<List<IncomeTaxReportList>> GetList(string userId);

    Task<bool> Create(IncomeTaxReportSaveRequest request, string userId);

    Task<IncomeTaxReportDetails> GetById(Guid id);

    Task<bool> Edit(Guid id, IncomeTaxReportSaveRequest request);

    Task<bool> Delete(Guid id);
}

public class IncomeTaxReportService(
    IMapper mapper,
    IIncomeTaxReportRepository repository) : IIncomeTaxReportService
{
    public async Task<List<IncomeTaxReportList>> GetList(string userId)
    {
        var entities = await repository.GetList(userId);
        var mapped = entities.Select(mapper.Map<IncomeTaxReportList>).ToList();
        return mapped;
    }

    public async Task<bool> Create(IncomeTaxReportSaveRequest request, string userId)
    {
        var pension = mapper.Map<DataAccessLayer.Entities.IncomeTaxReport>(request);
        pension.Id = Guid.NewGuid();
        pension.UserId = userId;
        await repository.Create(pension);
        return true;
    }

    public async Task<IncomeTaxReportDetails> GetById(Guid id)
    {
        var entity = await repository.GetById(id);
        return entity == null ? null : mapper.Map<IncomeTaxReportDetails>(entity);
    }

    public async Task<bool> Edit(Guid id, IncomeTaxReportSaveRequest request)
    {
        // Automapper will only reset some of the properties.
        var current = await repository.GetById(id);
        current = mapper.Map(request, current);
        return await repository.Update(current);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await repository.Delete(id);
    }
}