using System.Data.Common;
using DataAccessLayer.Configurations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using MySqlConnector;

namespace DataAccessLayer.Repositories.Implementations;

public class PensionRepository(DatabaseOptions dbOptions, Serilog.ILogger logger) : BaseRepository<Pension>(dbOptions, logger), IPensionRepository
{
    public override string TableName => "Pensions";
}