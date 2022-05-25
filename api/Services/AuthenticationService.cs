using api.Services.Abstractions;
using common.Exceptions;
using common.Dtos;
using common.Models;
using core.Repositories.Abstractions;

namespace api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _users;

        public AuthenticationService(IUsersRepository users)
        {
            _users = users;
        }

        public async Task<UserDto> Authenticate(UserLoginRequest userDto)
        {
            var user = await _users.GetByEmail(userDto.Email);

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            return user;
        }
    }
}
