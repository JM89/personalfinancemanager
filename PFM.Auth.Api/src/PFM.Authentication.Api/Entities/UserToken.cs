namespace PFM.Authentication.Api.Entities
{
    public class UserToken : PersistedEntity
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}