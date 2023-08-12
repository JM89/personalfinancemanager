namespace PFM.MovementAggregator.Caches.Interfaces
{
    public interface ITokenCache
    {
        Task<string> GetToken(string clientId, string clientSecret);
    }
}
