using System.IdentityModel.Tokens.Jwt;

namespace api.Services.Abstractions
{
    public interface IJwtTokenService
    {
        public string Generate(int id, string role);
        public JwtSecurityToken Validate(string jwt);
        public int GetIssuer(string jwt);
    }
}
