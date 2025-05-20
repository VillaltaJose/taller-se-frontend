using Backend.Core.CustomEntities;
using Backend.Core.DTOs;

namespace Backend.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<Result<SesionResponse>> AuthenticateWithCredentials(LoginWithCredentialRequest credentialRequest);

        Task<Result<SesionResponse>> UpdateRefreshToken(string? oldRefreshToken);

        Result<string> HashPassword(string password);
    }
}
