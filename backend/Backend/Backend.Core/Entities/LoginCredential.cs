namespace Backend.Core.Entities
{
    public class LoginCredential
    {
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
