namespace PFM.BankAccountUpdater.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<bool> UpdateBalance(int bankAccountId, string userId, decimal newBalance);
    }
}
