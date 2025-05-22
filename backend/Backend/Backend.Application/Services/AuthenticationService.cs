using Backend.Core.CustomEntities;
using Backend.Core.DTOs;
using Backend.Core.Entities;
using Backend.Core.Helpers;
using Backend.Core.Interfaces.Repositories;
using Backend.Core.Interfaces.Services;
using Backend.Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordService _passwordService;
        private readonly SecurityOptions _securityOptions;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IPasswordService passwordService,
            IOptions<SecurityOptions> options,
            ILogger<AuthenticationService> logger
        )
        {
            _authenticationRepository = authenticationRepository;
            _passwordService = passwordService;
            _logger = logger;
            _securityOptions = options.Value;
        }

        public async Task<Result<SesionResponse>> AuthenticateWithCredentials(LoginWithCredentialRequest credentialRequest)
        {
            using var scope = TransactionScopeHelper.StartTransaction();

            _logger.LogError("Test");
            var user = await _authenticationRepository.GetUserByEmail(credentialRequest.Email);

            var time = new Random().Next(0, 10);

            await Task.Delay(time * 1000);
            
            if (user is null)
            {
                _logger.LogError("Credenciales: {@Params}", new
                {
                    Correo = credentialRequest.Email
                });

                return Result<SesionResponse>.Fail("Incorrect username or password");
            }

            user.Credential = await _authenticationRepository.GetCredential(user.Id);

            //var passwordValid = _passwordService.Check(user.Credential.PasswordHash, credentialRequest.Password);

            /*var passwordValid = true;

            if (!passwordValid)
            {
                // TODO: Log log failure
                scope.Complete();
                return Result<SesionResponse>.Fail("Incorrect username or password");
            }*/

            var sesionResponse = await CompleteLogin(user);
            scope.Complete();

            return Result<SesionResponse>.Ok(sesionResponse);
        }

        private async Task<SesionResponse> CompleteLogin(User user)
        {
            using var scope = TransactionScopeHelper.StartTransaction();

            var sessionId = Guid.NewGuid();

            var jwt = GenerateToken(user, _securityOptions.JwtMinutesTime, sessionId: sessionId.ToString());
            var refreshToken = GenerateToken(user, _securityOptions.RefreshTokenMinutesTime, sessionId: sessionId.ToString());

            var isSessionRegistered = await _authenticationRepository.RegisterSession(new UserSession
            {
                Id = sessionId,
                RefreshToken = refreshToken,
                Revoked = false,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(_securityOptions.SessionDuration),
                UpdatedAt = DateTime.Now,
                UserId = user.Id,
            });

            if (!isSessionRegistered)
            {
                Result<SesionResponse>.Fail("An error occurred while saving the session");
            }

            scope.Complete();

            return new SesionResponse
            {
                UserProfile = new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Lastname = user.Lastname
                },
                JWT = jwt,
                RefreshToken = refreshToken,
            };
        }

        public async Task<Result<SesionResponse>> UpdateRefreshToken(string? oldRefreshToken)
        {
            if (string.IsNullOrWhiteSpace(oldRefreshToken))
            {
                return Result<SesionResponse>.Fail("Invalid refresh token");
            }

            using var scope = TransactionScopeHelper.StartTransaction();

            Guid sessionId;
            var sessionIdClaim = GetClaimValue(oldRefreshToken, "SessionId") ?? "";

            if (!Guid.TryParse(sessionIdClaim, out sessionId))
            {
                return Result<SesionResponse>.Fail("Invalid refresh token");
            }

            var session = await _authenticationRepository.GetRefreshToken(sessionId);

            if (session is null)
            {
                return Result<SesionResponse>.Fail("Session expired or revoked");
            }

            var shouldUpdateToken = false;
            if (IsTokenExpired(oldRefreshToken))
            {
                shouldUpdateToken = true;

                if (session.ExpiresAt < DateTime.Now)
                {
                    return Result<SesionResponse>.Fail("Session expired or revoked");
                }
            }

            var user = new User
            {
                Id = session.UserId,
                Name = GetClaimValue(oldRefreshToken, "Name") ?? "",
                Lastname = GetClaimValue(oldRefreshToken, "Lastname") ?? "",
                Email = GetClaimValue(oldRefreshToken, "Email") ?? "",
            };

            var jwt = GenerateToken(user, _securityOptions.JwtMinutesTime, sessionId: sessionId.ToString());
            var newRefreshToken = GenerateToken(user, _securityOptions.RefreshTokenMinutesTime, sessionId: sessionId.ToString());

            scope.Complete();
            return Result<SesionResponse>.Ok(new SesionResponse
            {
                UserProfile = new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Lastname = user.Lastname
                },
                JWT = jwt,
                RefreshToken = newRefreshToken,
            });
        }

        public Result<string> HashPassword(string password)
        {
            return Result<string>.Ok(_passwordService.Hash(password));
        }

        private string GenerateToken(User user, int? minutes = null, string? sessionId = "")
        {
            minutes ??= _securityOptions.JwtMinutesTime;

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_securityOptions.SecretKey)
            );

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var claims = new List<Claim>
            {
                new ("Name", $"{user.Name}"),
                new ("Lastname", $"{user.Name}"),
                new ("Email", user.Email),
                new ("UId", user.Id.ToString()),
                new ("SessionId", Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                claims.Add(new Claim("SessionId", sessionId));
            }

            var payload = new JwtPayload
            (
                _securityOptions.Issuer,
                _securityOptions.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes((float)minutes)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string? GetClaimValue(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

            return claim?.Value;
        }

        private bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var expirationDateUnix = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            if (expirationDateUnix == null)
            {
                return false;
            }

            var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationDateUnix)).UtcDateTime;
            return expirationDateTime < DateTime.UtcNow;
        }
    }
}
