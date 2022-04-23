using api.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly byte[] _key;

        public JwtTokenService(byte[] key)
        {
            _key = key;
        }

        public string Generate(int id, string role)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(_key);
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
            var symmetricSecurityKey = new SymmetricSecurityKey(_key);

            new JwtSecurityTokenHandler().ValidateToken(jwt, new TokenValidationParameters
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
