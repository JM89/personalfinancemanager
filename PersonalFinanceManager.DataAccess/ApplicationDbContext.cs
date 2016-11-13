using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
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

        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.ExpenditureModel> ExpenditureModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.AccountModel> AccountModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.ExpenditureTypeModel> ExpenditureTypeModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.PaymentMethodModel> PaymentMethodModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.CurrencyModel> CurrencyModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.FrequencyModel> FrequencyModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.PeriodicOutcomeModel> PeriodicOutcomeModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.CountryModel> CountryModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.IncomeModel> IncomeModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.BankModel> BankModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.BudgetPlanModel> BudgetPlanModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.AtmWithdrawModel> AtmWithdrawModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.HistoricMovementModel> HistoricMovementModels { get; set; }
        public System.Data.Entity.DbSet<PersonalFinanceManager.Entities.BudgetByExpenditureTypeModel> BudgetByExpenditureTypeModels { get; set; }
    }
}
