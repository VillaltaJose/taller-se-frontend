namespace Backend.Core.Options
{
    public class SecurityOptions
    {
        public int JwtMinutesTime { get; set; }
        public int RefreshTokenMinutesTime { get; set; }

        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int SessionDuration { get; set; }
    }
}
