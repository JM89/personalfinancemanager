using Microsoft.EntityFrameworkCore;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer
{
    public class PFMContext: DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public PFMContext(DbContextOptions<PFMContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
