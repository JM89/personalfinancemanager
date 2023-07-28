using PFM.Api.Contracts.ExpenseType;

namespace PFM.Website.Services
{
	public class ExpenseTypeService
	{
		private IList<ExpenseTypeDetails> _expenseTypes;

		public ExpenseTypeService()
		{
			_expenseTypes = new List<ExpenseTypeDetails>()
			{
				new ExpenseTypeDetails()
				{
					Id = 1,
					Name = "Food",
					GraphColor = "3399FF",
					ShowOnDashboard = false
				},
				new ExpenseTypeDetails()
                {
                    Id = 2,
                    Name = "Energy",
                    GraphColor = "33CC33",
                    ShowOnDashboard = false
                },
				new ExpenseTypeDetails()
                {
                    Id = 3,
                    Name = "Transport",
                    GraphColor = "FF0000",
                    ShowOnDashboard = false
                }
            };
        }

        public async Task<ExpenseTypeDetails[]> GetAll()
        {
            return await Task.FromResult(_expenseTypes.ToArray());
        }

        public async Task<ExpenseTypeDetails?> GetById(int id)
        {
            return await Task.FromResult(_expenseTypes.SingleOrDefault(x => x.Id == id));
        }

        public async Task<bool> Create(ExpenseTypeDetails expenseType)
        {
            expenseType.Id = _expenseTypes.Max(x => x.Id) + 1;
            _expenseTypes.Add(expenseType);
            return await Task.FromResult(true);
        }

        public async Task<bool> Edit(int id, ExpenseTypeDetails expenseType)
        {
            var existing = await GetById(id);

            if (existing == null)
                return false;

            existing.Name = expenseType.Name;
            existing.GraphColor = expenseType.GraphColor;
            existing.ShowOnDashboard = expenseType.ShowOnDashboard;
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var existing = await GetById(id);

            if (existing == null)
                return false;

            _expenseTypes.Remove(existing);
            return true;
        }
    }
}

