namespace PersonalFinanceManager.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 1, Name = "CB", CssClass = "primary" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 2, Name = "Cash", CssClass = "info" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 3, Name = "Direct Debit", CssClass = "success" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 4, Name = "Transfer", CssClass = "warning" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 5, Name = "Internal Transfer", CssClass = "danger" });
        }
    }
}
