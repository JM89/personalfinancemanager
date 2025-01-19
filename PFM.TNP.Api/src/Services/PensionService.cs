using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using PFM.TNP.Api.Contracts.Pension;

namespace Services;

public interface IPensionService 
{
    Task<List<PensionList>> GetList(string userId);

    Task<bool> Create(PensionSaveRequest request, string userId);

    Task<PensionDetails> GetById(Guid id);

    Task<bool> Edit(Guid id, PensionSaveRequest request);

    Task<bool> Delete(Guid id);
}

public class PensionService(
    IMapper mapper, 
    IPensionRepository pensionRepository) : IPensionService
{
    public async Task<List<PensionList>> GetList(string userId)
    {
        var entities = await pensionRepository.GetList(userId);
        var mapped = entities.Select(mapper.Map<PensionList>).ToList();
        return mapped;
    }

    public async Task<bool> Create(PensionSaveRequest request, string userId)
    {
        var entity = mapper.Map<DataAccessLayer.Entities.Pension>(request);
        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.LastUpdated = DateTime.UtcNow;
        await pensionRepository.Create(entity);
        return true;
    }

    public async Task<PensionDetails> GetById(Guid id)
    {
        var pension = await pensionRepository.GetById(id);
        return pension == null ? null : mapper.Map<PensionDetails>(pension);
    }

    public async Task<bool> Edit(Guid id, PensionSaveRequest request)
    {
        // Automapper will only reset some of the properties.
        var current = await pensionRepository.GetById(id);
        current = mapper.Map(request, current);
        current.LastUpdated = DateTime.UtcNow;
        return await pensionRepository.Update(current);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await pensionRepository.Delete(id);
    }
}