using Backend.Core.Entities;
using Backend.Core.Interfaces.Repositories;

namespace Backend.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ICollection<User> _users;
        private readonly ICollection<LoginCredential> _loginCredentials;
        private readonly ICollection<RefreshToken> _refreshTokens;

        public AuthenticationRepository()
        {
            _users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Email = "john.doe@example.com", Name = "John Doe" },
                new User { Id = Guid.NewGuid(), Email = "jane.smith@example.com", Name = "Jane Smith" }
            };

            _loginCredentials = new List<LoginCredential>
            {
                new LoginCredential { UserId = _users.ElementAt(0).Id, PasswordHash = "hashed_password_1" },
                new LoginCredential { UserId = _users.ElementAt(1).Id, PasswordHash = "hashed_password_2" }
            };

            _refreshTokens = new List<RefreshToken>
            {
                new RefreshToken { Id = Guid.NewGuid(), UserId = _users.ElementAt(0).Id, Token = "refresh_token_1", ExpiresAt = DateTime.UtcNow.AddDays(7) },
                new RefreshToken { Id = Guid.NewGuid(), UserId = _users.ElementAt(1).Id, Token = "refresh_token_2", ExpiresAt = DateTime.UtcNow.AddDays(7) }
            };
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public async Task<bool> RegisterSession(UserSession session)
        {
            // Simular éxito en guardar sesión
            return await Task.FromResult(true);
        }

        public async Task<LoginCredential> GetCredential(Guid userId)
        {
            var credential = _loginCredentials.FirstOrDefault(c => c.UserId == userId);
            return await Task.FromResult(credential ?? new LoginCredential());
        }

        public async Task<RefreshToken?> GetRefreshToken(Guid id)
        {
            return await Task.FromResult(_refreshTokens.FirstOrDefault(t => t.Id == id));
        }
    }

}
