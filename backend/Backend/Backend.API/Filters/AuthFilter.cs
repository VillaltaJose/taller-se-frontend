using Backend.Core.App;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.API.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        private readonly Sesion _sesion;

        public AuthFilter(
            Sesion sesion
        )
        {
            _sesion = sesion;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = await GetTokenInformation(context.HttpContext);

            var idUsuario = GetClaim(token, ClaimTypes.Sid);
            var idInstitucion = Convert.ToInt32(GetClaim(token, ClaimTypes.Locality));

            _sesion.IdUsuario = Guid.Parse(idUsuario);
            _sesion.IdInstitucion = idInstitucion;
        }

        private async Task<IEnumerable<Claim>> GetTokenInformation(HttpContext httpContext)
        {
            var jwt = await httpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            return token.Claims;
        }

        private string GetClaim(IEnumerable<Claim> token, string claimName)
        {
            var claim = token
                .Where(c => c.Type == claimName)
                .Select(c => c.Value).SingleOrDefault();

            return claim.ToString();
        }
    }
}
