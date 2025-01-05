using Microsoft.EntityFrameworkCore;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer
{
    public class PFMContext: DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<BudgetPlan> BudgetPlans { get; set; }
        public DbSet<AtmWithdraw> AtmWithdraws { get; set; }
        public DbSet<BudgetByExpenseType> BudgetByExpenseTypes { get; set; }
        public DbSet<Saving> Savings { get; set; }

        public PFMContext(DbContextOptions<PFMContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 1, Name = "CB", CssClass = "primary", IconClass = "fa fa-credit-card", HasBeenAlreadyDebitedOption = true });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 2, Name = "Cash", CssClass = "info", IconClass = "fa fa-money", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 3, Name = "Direct Debit", CssClass = "success", IconClass = "fa fa-credit-card-alt", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 4, Name = "Transfer", CssClass = "warning", IconClass = "fa fa-external-link", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 5, Name = "Internal Transfer", CssClass = "danger", IconClass = "fa fa-refresh", HasBeenAlreadyDebitedOption = false });
        }
    }
}
