using Dapper;
using Microsoft.Data.SqlClient;
using PFM.MovementAggregator.Persistence.Entities;
using PFM.MovementAggregator.Settings;

namespace PFM.MovementAggregator.Persistence.Implementations
{
    public class MovementAggregatorRepository : IMovementAggregatorRepository
	{
		private readonly Serilog.ILogger _logger;
        private readonly ApplicationSettings _appSettings;

        public MovementAggregatorRepository(Serilog.ILogger logger, ApplicationSettings appSettings)
		{
			_logger = logger;
            _appSettings = appSettings;
		}

        private const string MergeSql =
			@"MERGE INTO MovementAggregation AS Target
				USING (
					SELECT
						@BankAccountId AS BankAccountId,
						@MonthYearIdentifier AS MonthYearIdentifier,
						@Type AS Type,
						@Category AS Category,
						@AggregatedAmount AS AggregatedAmount
				) AS Source
				ON Source.BankAccountId = Target.BankAccountId
					AND Source.MonthYearIdentifier = Target.MonthYearIdentifier
					AND Source.Type = Target.Type
					AND Source.Category = Target.Category
				WHEN MATCHED THEN
					UPDATE SET Target.AggregatedAmount = Target.AggregatedAmount + Source.AggregatedAmount
				WHEN NOT MATCHED THEN
					INSERT (BankAccountId, MonthYearIdentifier, Type, Category, AggregatedAmount) 
					VALUES (Source.BankAccountId, Source.MonthYearIdentifier, Source.Type, Source.Category, Source.AggregatedAmount);";


        public async Task<int> Merge(MovementAggregation movementAggregation)
		{
			try
			{
				await using var connection = new SqlConnection(_appSettings.DbConnection);

                var count = await connection.ExecuteAsync(MergeSql, movementAggregation);

                return count;
            }
			catch(Exception ex)
			{
				_logger.Error(ex, "Unhandled exception while merging movements");
				throw;
			}
        }
	}
}

