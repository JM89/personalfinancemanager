using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PFM.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Api.Configuration
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PFMContext>
    {
        public PFMContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PFMContext>();
            var connectionString = configuration.GetConnectionString("PFMConnection");
            builder.UseSqlServer(connectionString);
            return new PFMContext(builder.Options);
        }
    }
}
