namespace PFM.BankAccountUpdater.Services.Interfaces
{
    public interface IAuthTokenStore
    {
        Task<string> GetToken();
    }
}
