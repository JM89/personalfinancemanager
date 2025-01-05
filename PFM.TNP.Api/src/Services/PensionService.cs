using System;
using AutoMapper;
using PFM.Pension.Api.Contracts.Pension;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;

namespace Services;

public interface IPensionService 
{
    Task<List<PensionList>> GetList(string userId);

    Task<bool> Create(PensionSaveRequest pensionDetails, string userId);

    Task<PensionDetails> GetById(Guid id);

    Task<bool> Edit(Guid id, PensionSaveRequest objDetails);

    Task<bool> Delete(Guid id);
}

public class PensionService(IPensionRepository pensionRepository) : IPensionService
{
    public async Task<List<PensionList>> GetList(string userId)
    {
        var entities = await pensionRepository.GetList(userId);
        var mapped = entities.Select(Mapper.Map<PensionList>).ToList();
        return mapped;
    }

    public async Task<bool> Create(PensionSaveRequest pensionDetails, string userId)
    {
        var pension = Mapper.Map<DataAccessLayer.Entities.Pension>(pensionDetails);
        pension.Id = Guid.NewGuid();
        pension.UserId = userId;
        pension.LastUpdated = DateTime.UtcNow;
        await pensionRepository.Create(pension);
        return true;
    }

    public async Task<PensionDetails> GetById(Guid id)
    {
        var pension = await pensionRepository.GetById(id);
        return pension == null ? null : Mapper.Map<PensionDetails>(pension);
    }

    public async Task<bool> Edit(Guid id, PensionSaveRequest objDetails)
    {
        // Automapper will only reset some of the properties.
        var existingEntity = await pensionRepository.GetById(id);
        existingEntity = Mapper.Map(objDetails, existingEntity);
        existingEntity.LastUpdated = DateTime.UtcNow;
        return await pensionRepository.Update(existingEntity);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await pensionRepository.Delete(id);;
    }
}