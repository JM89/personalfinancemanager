using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<Entities.ApplicationUser>
    {
        public ApplicationDbContext()
            : base("PersonalFinanceDatabase", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Entities.ExpenditureModel> ExpenditureModels { get; set; }
        public System.Data.Entity.DbSet<Entities.AccountModel> AccountModels { get; set; }
        public System.Data.Entity.DbSet<Entities.ExpenditureTypeModel> ExpenditureTypeModels { get; set; }
        public System.Data.Entity.DbSet<Entities.PaymentMethodModel> PaymentMethodModels { get; set; }
        public System.Data.Entity.DbSet<Entities.CurrencyModel> CurrencyModels { get; set; }
        public System.Data.Entity.DbSet<Entities.CountryModel> CountryModels { get; set; }
        public System.Data.Entity.DbSet<Entities.IncomeModel> IncomeModels { get; set; }
        public System.Data.Entity.DbSet<Entities.BankModel> BankModels { get; set; }
        public System.Data.Entity.DbSet<Entities.BudgetPlanModel> BudgetPlanModels { get; set; }
        public System.Data.Entity.DbSet<Entities.AtmWithdrawModel> AtmWithdrawModels { get; set; }
        public System.Data.Entity.DbSet<Entities.HistoricMovementModel> HistoricMovementModels { get; set; }
        public System.Data.Entity.DbSet<Entities.BudgetByExpenditureTypeModel> BudgetByExpenditureTypeModels { get; set; }
        public System.Data.Entity.DbSet<Entities.BankBrandModel> BankBranchModels { get; set; }
        public System.Data.Entity.DbSet<Entities.UserProfileModel> UserProfileModels { get; set; }
        public System.Data.Entity.DbSet<Entities.SavingModel> SavingModels { get; set; }
    }
}
