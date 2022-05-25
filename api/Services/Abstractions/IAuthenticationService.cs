using common.Dtos;
using common.Models;

namespace api.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserDto> Authenticate(UserLoginRequest userDto);
    }
}
