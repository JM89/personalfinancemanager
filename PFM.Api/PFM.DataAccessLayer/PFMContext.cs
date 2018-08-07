using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer
{
    public class PFMContext: DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<BudgetPlan> BudgetPlans { get; set; }
        public DbSet<AtmWithdraw> AtmWithdraws { get; set; }
        public DbSet<HistoricMovement> HistoricMovements { get; set; }
        public DbSet<BudgetByExpenseType> BudgetByExpenseTypes { get; set; }
        public DbSet<BankBranch> BankBranches { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryDeduction> SalaryDeductions { get; set; }
        public DbSet<Pension> Pensions { get; set; }
        public DbSet<FrequenceOption> FrequenceOptions { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<Tax> Taxes { get; set; }

        public PFMContext(DbContextOptions<PFMContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salary>().HasOne(u => u.Tax).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Saving>().HasOne(u => u.TargetInternalAccount).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
