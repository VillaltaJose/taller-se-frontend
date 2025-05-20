namespace Backend.Core.DTOs
{
    public class SesionResponse
    {
        public UserProfileResponse UserProfile { get; set; }
        public string JWT { get; set; }
        public string RefreshToken { get; set; }
    }
}
