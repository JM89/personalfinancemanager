namespace PFM.Authentication.Api.Entities
{
    public class User : PersistedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
    }
}