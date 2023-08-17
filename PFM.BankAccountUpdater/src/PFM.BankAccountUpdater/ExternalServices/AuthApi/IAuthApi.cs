using PFM.BankAccountUpdater.ExternalServices.AuthApi.Contracts;

namespace PFM.BankAccountUpdater.ExternalServices.AuthApi
{
	public interface IAuthApi
	{
        Task<ClientToken> Login(ClientInfo info);
    }
}

