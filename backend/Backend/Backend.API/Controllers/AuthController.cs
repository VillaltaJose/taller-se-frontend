using Backend.Core.CustomEntities;
using Backend.Core.DTOs;
using Backend.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(
            IAuthenticationService authenticationService
        )
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithCredential(LoginWithCredentialRequest request)
        {
            var result = await _authenticationService.AuthenticateWithCredentials(request);

            if (result.Success)
            {
                SetRefreshTokenCookie(result.Value.RefreshToken);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> RefreshSesion()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];

            var result = await _authenticationService.UpdateRefreshToken(oldRefreshToken);

            if (!result.Success)
            {
                return StatusCode(403, result);
            }

            SetRefreshTokenCookie(result.Value.RefreshToken);

            return Ok(result);
        }

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                // TODO: agregar dominio
            };

            HttpContext.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        [HttpGet]
        public IActionResult GeneratePassword([FromQuery] string password)
        {
            var result = _authenticationService.HashPassword(password);
            return Ok(result);
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("refreshToken");

            return Ok(Result.Ok());
        }
    }
}
