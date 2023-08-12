namespace PFM.BankAccountUpdater.ExternalServices.AuthApi.Contracts
{
	public class ClientInfo
	{
		public string ClientId { get; }

        public string ClientSecret { get; }

		public ClientInfo(string clientId, string clientSecret)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
		}
    }
}

