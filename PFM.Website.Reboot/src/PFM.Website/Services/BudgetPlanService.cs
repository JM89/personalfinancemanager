using System;
using AutoMapper;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
	public class BudgetPlanService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ApplicationSettings settings,
        IBudgetPlanApi api)
        : CoreService(logger, mapper, httpContextAccessor, settings)
    {
        public async Task<List<BudgetPlanListModel>> GetAll(int accountId)
        {
            var apiResponse = await api.GetList(accountId);
            var response = ReadApiResponse<List<BudgetPlanList>>(apiResponse) ?? new List<BudgetPlanList>();
            var models = response.Select(Mapper.Map<BudgetPlanListModel>).ToList();
            return models;
        }

        public async Task<BudgetPlanEditModel> GetById(int id)
        {
            var apiResponse = await api.Get(id);
            var item = ReadApiResponse<BudgetPlanDetails>(apiResponse);
            return Mapper.Map<BudgetPlanEditModel>(item);
        }

        public async Task<bool> Create(int accountId, BudgetPlanEditModel model)
        {
            var request = Mapper.Map<BudgetPlanDetails>(model);
            var apiResponse = await api.Create(accountId, request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int accountId, BudgetPlanEditModel model)
        {
            var request = Mapper.Map<BudgetPlanDetails>(model);
            var apiResponse = await api.Edit(accountId, request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Start(int id, int accountId)
        {
            var apiResponse = await api.Start(id, accountId);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Stop(int id)
        {
            var apiResponse = await api.Stop(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }
    }
}

