using Backend.Core.Entities;

namespace Backend.Core.Interfaces.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(Guid id);
        Task<bool> RegisterSession(UserSession session);
        Task<LoginCredential> GetCredential(Guid userId);
        Task<RefreshToken?> GetRefreshToken(Guid id);
    }
}
