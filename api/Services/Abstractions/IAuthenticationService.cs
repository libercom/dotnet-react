using core.Dtos;
using core.Models;

namespace api.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<User> Authenticate(UserLoginDto userDto);
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid credentials")
        {
        }
    }
}
