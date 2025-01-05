using DataAccessLayer.Configurations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Internal;

namespace DataAccessLayer.Repositories;

public interface IIncomeTaxReportRepository: IBaseRepository<IncomeTaxReport>
{
}

public class IncomeTaxReportRepository(DatabaseOptions dbOptions, Serilog.ILogger logger) : BaseRepository<IncomeTaxReport>(dbOptions, logger), IIncomeTaxReportRepository
{
    protected override string TableName => "IncomeTaxReports";
}