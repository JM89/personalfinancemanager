using System;
using System.Configuration;
using System.Data.SqlClient;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public static class DatabaseChecker
    {
        private static string _connectionString;

        public static void Initialize()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PersonalFinanceDatabase"].ConnectionString;
        }

        public static decimal GetBankAccountAmount(int sourceAccountId)
        {
            return GetResult<decimal>($"SELECT CurrentBalance FROM Accounts WHERE Id={sourceAccountId}");
        }

        public static int CountIncomes()
        {
            return GetResult<int>($"SELECT COUNT(*) FROM Incomes");
        }

        public static int CountMovements()
        {
            return GetResult<int>($"SELECT COUNT(*) FROM HistoricMovements");

        }

        public static int CountSavings()
        {
            return GetResult<int>($"SELECT COUNT(*) FROM Savings");

        }

        public static decimal GetSavingCost(int id)
        {
            return GetResult<decimal>($"SELECT Amount FROM Incomes WHERE Id={id}");
        }

        public static int CountCountries()
        {
            return GetResult<int>($"SELECT COUNT(*) FROM Countries");
        }

        public static decimal GetIncomeCost(int id)
        {
            return GetResult<decimal>($"SELECT Cost FROM Incomes WHERE Id={id}");

        }

        public static int CountExpenditures()
        {
            return GetResult<int>($"SELECT COUNT(*) FROM Expenses");
        }

        public static decimal GetExpenditureCost(int id)
        {
            return GetResult<decimal>($"SELECT Cost FROM Expenses WHERE Id={id}");
        }

        public static int CountAtmWithdraws()
        {
            return GetResult<int>($"SELECT COUNT(*) FROM AtmWithdraws");
        }

        public static decimal GetAtmWithdrawInitialAmount(int id)
        {
            return GetResult<decimal>($"SELECT InitialAmount FROM AtmWithdraws WHERE Id={id}");
        }

        public static decimal GetAtmWithdrawCurrentAmount(int id)
        {
            return GetResult<decimal>($"SELECT CurrentAmount FROM AtmWithdraws WHERE Id={id}");
        }

        private static T GetResult<T>(string queryString)
        {
            T result = default(T);

            using (var connection = new SqlConnection(_connectionString))
            {
                // Create the Command and Parameter objects.
                var command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteScalar();

                    result = (T)reader;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return result; 
            }
        }
    }
}
