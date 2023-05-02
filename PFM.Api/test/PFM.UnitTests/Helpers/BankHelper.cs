namespace PFM.UnitTests.Helpers
{
    public static class BankHelper
    {
        public static PFM.DataAccessLayer.Entities.Bank CreateBankModel()
        {
            var entity = new PFM.DataAccessLayer.Entities.Bank();
            return entity;
        }
    }
}
