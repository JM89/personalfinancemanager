using System;
using AutoMapper;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
	public class BudgetPlanService: CoreService
	{
        private readonly IBudgetPlanApi _api;

        public BudgetPlanService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, IBudgetPlanApi api) : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<BudgetPlanListModel>> GetAll(int accountId)
        {
            var apiResponse = await _api.GetList(accountId);
            var response = ReadApiResponse<List<BudgetPlanList>>(apiResponse) ?? new List<BudgetPlanList>();
            var models = response.Select(_mapper.Map<BudgetPlanListModel>).ToList();
            return models;
        }

        public async Task<BudgetPlanEditModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<BudgetPlanDetails>(apiResponse);
            return _mapper.Map<BudgetPlanEditModel>(item);
        }

        public async Task<bool> Create(int accountId, BudgetPlanEditModel model)
        {
            var request = _mapper.Map<BudgetPlanDetails>(model);
            var apiResponse = await _api.Create(accountId, request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int accountId, BudgetPlanEditModel model)
        {
            var request = _mapper.Map<BudgetPlanDetails>(model);
            var apiResponse = await _api.Edit(accountId, request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Start(int id, int accountId)
        {
            var apiResponse = await _api.Start(id, accountId);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Stop(int id)
        {
            var apiResponse = await _api.Stop(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }
    }
}

