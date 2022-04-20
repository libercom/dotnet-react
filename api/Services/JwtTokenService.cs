using api.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly string key = "what's up danger?";

        public string Generate(int id, string role)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, role));

            var payload = new JwtPayload(id.ToString(), null, claims, null, DateTime.Now.AddHours(1));
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public JwtSecurityToken Validate(string jwt)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            _ = new JwtSecurityTokenHandler().ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken) validatedToken;
        }
    }
}
