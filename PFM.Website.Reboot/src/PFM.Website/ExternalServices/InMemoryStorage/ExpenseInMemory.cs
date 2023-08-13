using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.ExpenseType;
using PFM.Api.Contracts.PaymentMethod;
using PFM.Api.Contracts.SearchParameters;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
	public class ExpenseInMemory : IExpenseApi
	{
        internal IList<ExpenseDetails> _storage;
        private IList<AtmWithdrawDetails> _atmWithdraws = new AtmWithdrawInMemory()._storage.ToList();
        private IList<PaymentMethodList> _paymentMethods = new PaymentMethodInMemory()._storage.ToList();
        private IList<ExpenseTypeDetails> _expenseTypes = new ExpenseTypeInMemory()._storage.ToList();

        public ExpenseInMemory()
        {
            _storage = new List<ExpenseDetails>();
            var rng = new Random();

            // Common expenses
            for (int i = 1; i <= 5; i++)
            {
                var filteredPaymentMethods = _paymentMethods.Where(x => x.Id == 1 || x.Id == 3 || x.Id == 4).ToList();
                var expenseType = _expenseTypes.ElementAt(rng.Next(_expenseTypes.Count));
                var paymentMethod = filteredPaymentMethods.ElementAt(rng.Next(filteredPaymentMethods.Count));
                var item = new ExpenseDetails()
                {
                    Id = i,
                    Description = $"Expense {i}",
                    AccountId = 1,
                    HasBeenAlreadyDebited = true,
                    Cost = 100 * i,
                    PaymentMethodId = paymentMethod.Id,
                    PaymentMethodHasBeenAlreadyDebitedOption = paymentMethod.HasBeenAlreadyDebitedOption,
                    DateExpense = DateTime.UtcNow.AddDays(-i),
                    ExpenseTypeId = expenseType.Id,
                    ExpenseTypeName = expenseType.Name
                };
                _storage.Add(item);
            }

            // Cash expenses
            var cashMethod = _paymentMethods.Single(x => x.Id == 2);

            for (int i = 6; i <= 10; i++)
            {
                var expenseType = _expenseTypes.ElementAt(rng.Next(_expenseTypes.Count));
                var item = new ExpenseDetails()
                {
                    Id = i,
                    Description = $"Expense {i}",
                    AccountId = 1,
                    HasBeenAlreadyDebited = true,
                    Cost = 100 * i,
                    PaymentMethodId = cashMethod.Id,
                    PaymentMethodHasBeenAlreadyDebitedOption = cashMethod.HasBeenAlreadyDebitedOption,
                    DateExpense = DateTime.UtcNow.AddDays(-i),
                    AtmWithdrawId = _atmWithdraws.ElementAt(rng.Next(_atmWithdraws.Count())).Id,
                    ExpenseTypeId = expenseType.Id,
                    ExpenseTypeName = expenseType.Name
                };
                _storage.Add(item);
            }

            // Internal transfer expenses
            var internalTransferMethod = _paymentMethods.Single(x => x.Id == 2);

            for (int i = 11; i <= 15; i++)
            {
                var expenseType = _expenseTypes.ElementAt(rng.Next(_expenseTypes.Count));
                var item = new ExpenseDetails()
                {
                    Id = i,
                    Description = $"Expense {i}",
                    AccountId = 1,
                    HasBeenAlreadyDebited = true,
                    Cost = 100 * i,
                    PaymentMethodId = internalTransferMethod.Id,
                    PaymentMethodHasBeenAlreadyDebitedOption = internalTransferMethod.HasBeenAlreadyDebitedOption,
                    DateExpense = DateTime.UtcNow.AddDays(-i),
                    TargetInternalAccountId = 2,
                    ExpenseTypeId = expenseType.Id,
                    ExpenseTypeName = expenseType.Name
                };
                _storage.Add(item);
            }
        }

        public async Task<ApiResponse> Create(ExpenseDetails obj)
        {
            obj.Id = _storage.Any() ? _storage.Max(x => x.Id) + 1 : 1;
            obj.ExpenseTypeName = _expenseTypes.Single(x => x.Id == obj.ExpenseTypeId).Name;
            obj.PaymentMethodHasBeenAlreadyDebitedOption = _paymentMethods.Single(x => x.Id == obj.PaymentMethodId).HasBeenAlreadyDebitedOption;

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

        public async Task<ApiResponse> GetList(ExpenseGetListSearchParameters search)
        {
            var result = JsonConvert.SerializeObject(_storage.Where(x => x.AccountId == search.AccountId).ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> ChangeDebitStatus(int id, bool debitStatus)
        {
            var item = _storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            item.HasBeenAlreadyDebited = !debitStatus;

            return await Task.FromResult(new ApiResponse(true));
        }
    }
}

