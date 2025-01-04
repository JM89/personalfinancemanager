using DataAccessLayer.Configurations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Internal;

namespace DataAccessLayer.Repositories;

public interface IPensionRepository : IBaseRepository<Pension>
{
}

public class PensionRepository(DatabaseOptions dbOptions, Serilog.ILogger logger) : BaseRepository<Pension>(dbOptions, logger), IPensionRepository
{
    public override string TableName => "Pensions";
}