using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TrabajoFinalDyAW.Utils
{
    public class JWTUtils
    {
        public static Guid GetUserIdFromToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token.Replace("Bearer ", ""));

            var userId = jwtToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            return Guid.Parse(userId);
        }
    }
}
