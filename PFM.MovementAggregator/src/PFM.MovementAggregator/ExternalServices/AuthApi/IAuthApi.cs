using PFM.MovementAggregator.ExternalServices.AuthApi.Contracts;

namespace PFM.MovementAggregator.ExternalServices.AuthApi
{
	public interface IAuthApi
	{
        Task<ClientToken> Login(ClientInfo info);
    }
}

