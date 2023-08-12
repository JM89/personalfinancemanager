namespace PFM.MovementAggregator.Services.Interfaces
{
    public interface IAuthTokenStore
    {
        Task<string> GetToken();
    }
}
