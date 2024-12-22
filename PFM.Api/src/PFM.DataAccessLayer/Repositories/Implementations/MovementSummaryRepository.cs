using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.SearchParameters;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
	public class MovementSummaryRepository(Serilog.ILogger logger, DataSettings dataSettings)
        : IMovementSummaryRepository
    {
        private const string SelectSql =
            @"SELECT
				BankAccountId,
				MonthYearIdentifier,
				Type,
				Category,
				AggregatedAmount
			  FROM MovementAggregation
              WHERE BankAccountId = @BankAccountId
              AND MonthYearIdentifier IN @MonthYearIdentifiers
              AND (@OptionalType IS NULL OR Type = @OptionalType)
              AND (@OptionalCategory IS NULL OR Category = @OptionalCategory)
              AND Category NOT IN @ExcludedCategories
            ";


        public async Task<IEnumerable<MovementSummary>> GetMovementSummaryOverTime(MovementSummarySearchParameters search)
        {
            try
            {
                using var connection = new SqlConnection(dataSettings.DbConnection);

                var entities = await connection.QueryAsync<MovementSummary>(SelectSql, search);

                return entities;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unhandled exception while getting the movements");
                throw;
            }
        }
    }
}

