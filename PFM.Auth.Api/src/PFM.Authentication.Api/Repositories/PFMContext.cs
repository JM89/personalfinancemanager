using Microsoft.EntityFrameworkCore;
using PFM.Authentication.Api.Entities;

namespace PFM.Authentication.Api.Repositories
{
    public class PFMContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public PFMContext(DbContextOptions<PFMContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
