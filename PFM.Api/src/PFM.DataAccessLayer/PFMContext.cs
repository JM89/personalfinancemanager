﻿using Microsoft.EntityFrameworkCore;
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Salary>().HasOne(u => u.Tax).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaxType>().HasData(new TaxType() { Id = 1, Name = "Local Tax", Description = "A tax assessed and levied by a local authority such as a county or municipality. A local tax is usually collected in the form of property taxes, and is used to fund a wide range of civic services from garbage collection to sewer maintenance. The amount of local taxes may vary widely from one jurisdiction to the next." });
            modelBuilder.Entity<TaxType>().HasData(new TaxType() { Id = 2, Name = "Property Tax", Description = "Property tax is a tax assessed on real estate. The tax is usually based on the value of the property (including the land) you own and is often assessed by local or municipal governments." });
            modelBuilder.Entity<TaxType>().HasData(new TaxType() { Id = 3, Name = "Income Tax", Description = "An income tax is a tax that governments impose on financial income generated by all entities within their jurisdiction. By law, businesses and individuals must file an income tax return every year to determine whether they owe any taxes or are eligible for a tax refund. Income tax is a key source of funds that the government uses to fund its activities and serve the public." });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 1, Name = "CB", CssClass = "primary", IconClass = "fa fa-credit-card", HasBeenAlreadyDebitedOption = true });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 2, Name = "Cash", CssClass = "info", IconClass = "fa fa-money", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 3, Name = "Direct Debit", CssClass = "success", IconClass = "fa fa-credit-card-alt", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 4, Name = "Transfer", CssClass = "warning", IconClass = "fa fa-external-link", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod() { Id = 5, Name = "Internal Transfer", CssClass = "danger", IconClass = "fa fa-refresh", HasBeenAlreadyDebitedOption = false });
            modelBuilder.Entity<FrequenceOption>().HasData(new FrequenceOption() { Id = 1, Name = "Once" });
            modelBuilder.Entity<FrequenceOption>().HasData(new FrequenceOption() { Id = 2, Name = "Every X Months" });
        }
    }
}
