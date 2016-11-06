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
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 1, Name = "CB" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 2, Name = "Cash" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 3, Name = "Direct Debit" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 4, Name = "Transfer" });
            context.PaymentMethodModels.AddOrUpdate(new Entities.PaymentMethodModel() { Id = 5, Name = "Internal Transfer" });
        }
    }
}
