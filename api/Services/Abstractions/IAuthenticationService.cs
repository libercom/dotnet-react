using core.Dtos;
using domain.Models;

namespace api.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserDto> Authenticate(UserLoginDto userDto);
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid credentials")
        {
        }
    }
}
