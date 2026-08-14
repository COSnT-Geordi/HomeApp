using HomeApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HomeApp.Helpers
{
    public class ControllerHelper
    {

        public static Principal GetTokenFromRequest(HttpRequest request)
        {
            var authHeader = request.Headers.Authorization.ToString().Replace("Bearer ", "");


            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(authHeader);

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            long.TryParse(userId, out long userIDParsed);
            return new Principal() { UserID = userIDParsed, UserName = userName ?? "", Password = "" };

        }
    }
}
