﻿using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using PFM.Pension.Api.Contracts.Pension;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services;

public class PensionService(IPensionRepository pensionRepository) : IPensionService
{
    public async Task<List<PensionList>> GetList(string userId)
    {
        var entities = await pensionRepository.GetList(userId);
        var mapped = entities.Select(Mapper.Map<PensionList>).ToList();
        return mapped;
    }

    public async Task<bool> Create(PensionDetails pensionDetails, string userId)
    {
        var pension = Mapper.Map<DataAccessLayer.Entities.Pension>(pensionDetails);
        await pensionRepository.Create(pension);
        return true;
    }

    public Task<PensionDetails> GetById(int id)
    {
        var pension = pensionRepository.GetById(id);
        if (pension == null)
        {
            return null;
        }
        return Task.FromResult(Mapper.Map<PensionDetails>(pension));
    }

    public async Task<bool> Edit(PensionDetails objDetails, string userId)
    {
        // ReSharper disable once RedundantAssignment
        // Automapper will only reset some of the properties.
        var existingEntity = (await pensionRepository.GetListAsNoTracking()).SingleOrDefault(x => x.Id == objDetails.Id);
        existingEntity = Mapper.Map<DataAccessLayer.Entities.Pension>(objDetails);

        await pensionRepository.Update(existingEntity);

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var existingEntity = await pensionRepository.GetById(id);
        return await pensionRepository.Delete(existingEntity);;
    }
}