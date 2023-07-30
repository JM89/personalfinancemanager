﻿using System;
using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class ExpenseTypeInMemory : IExpenseTypeApi
    {
        private IList<ExpenseTypeDetails> _storage;
        private static readonly string[] Colors = new[]
        {
            "3399FF", "33CC33", "FF0000"
        };

        public ExpenseTypeInMemory()
        {
            var rng = new Random();
            _storage = Enumerable.Range(1, 5).Select(index => new ExpenseTypeDetails
            {
                Id = index,
                Name = $"Expense Type #{index}",
                GraphColor = Colors[rng.Next(Colors.Length)],
                ShowOnDashboard = false
            }).ToList();
        }

        public async Task<ApiResponse> Create(ExpenseTypeDetails obj)
        {
            obj.Id = _storage.Max(x => x.Id) + 1;
            _storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var item = _storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            _storage.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int id, ExpenseTypeDetails obj)
        {
            var existing = _storage.SingleOrDefault(x => x.Id == id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.GraphColor = obj.GraphColor;
            existing.ShowOnDashboard = obj.ShowOnDashboard;
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Get()
        {
            var result = JsonConvert.SerializeObject(_storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }
    }
}

