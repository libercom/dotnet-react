using api.Services.Abstractions;
using core.Dtos;
using core.Repositories.Abstractions;

namespace api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _users;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IUsersRepository users, ILogger<AuthenticationService> logger)
        {
            _users = users;
            _logger = logger;
        }

        public async Task<UserDto> Authenticate(UserLoginDto userDto)
        {
            try
            {
                var user = await _users.GetByEmail(userDto.Email);

                if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                {
                    throw new InvalidCredentialsException();
                }

                return user;
            }
            catch (Exception ex) when (
                ex is InvalidCredentialsException 
                || ex is EntityNotFoundException
            )
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
